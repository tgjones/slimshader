using System.Collections.Generic;
using System.Linq;
using SlimShader.VirtualMachine.Analysis.ExplicitBranching;

namespace SlimShader.VirtualMachine.Analysis.ControlFlow
{
	public class ControlFlowGraph
	{
		private readonly IList<InstructionBase> _allInstructions;

		public static ControlFlowGraph FromInstructions(IEnumerable<InstructionBase> instructions)
		{
			var instructionsList = instructions.ToList();
			var result = new ControlFlowGraph(instructionsList);

			// The following algorithm is from "Analyzing Control Flow in Java Bytecode" by Jianjun Zhao.
			// Determine the basic blocks by finding the set of leaders:
			// - The first instruction is a leader.
			// - Each instruction that is the target of an unconditional branch is a leader.
			// - Each instruction that is the target of a conditional branch is a leader.
			// - Each instruction that immediately follows a conditional or unconditional branch is a leader.
			// - TODO: switch statements.
			// Each leader gives rise to a basic block consisting of all instructions up to the next leader
			// or the end of the bytecode.

			var blockInstructions = new List<InstructionBase>();
			int position = 0;
			for (int i = 0; i < instructionsList.Count; i++)
			{
				if (IsLeader(instructionsList, i) && blockInstructions.Any())
				{
					result.BasicBlocks.Add(new BasicBlock(position++, blockInstructions));
					blockInstructions = new List<InstructionBase>();
				}
				blockInstructions.Add(instructionsList[i]);
			}
			result.BasicBlocks.Add(new BasicBlock(position, blockInstructions));

			// Now we can use the following rules to construct the CFG.
			// Given that u and v are basic blocks:
			// - Add an edge (u,v) if v follows u in the bytecode and u does not terminate in an unconditional branch.
			// - Add an edge (u,v) if the last instruction of u is a conditional or unconditional branch to the first
			//   instruction of v.
			// - TODO: switch statements.
			foreach (var u in result.BasicBlocks)
			{
				foreach (var v in result.BasicBlocks.Where(x => x != u))
				{
					if ((v.Position == u.Position + 1 && !u.Instructions.Last().IsUnconditionalBranch)
						|| u.Instructions.Last().BranchesTo(v.Instructions.First()))
						u.Successors.Add(v);
				}
			}

			result.ComputePostDominators();

			return result;
		}

		private static bool IsLeader(IList<InstructionBase> instructions, int index)
		{
			if (index == 0)
				return true;

			var instruction = instructions[index];
			if (instructions.OfType<BranchingInstruction>().Any(x => x.BranchTarget == instruction))
				return true;

			if (index > 0 && instructions[index - 1] is BranchingInstruction)
				return true;

			return false;
		}

		public List<BasicBlock> BasicBlocks { get; private set; }

		public IList<InstructionBase> AllInstructions
		{
			get { return _allInstructions; }
		}

		public ControlFlowGraph(IList<InstructionBase> allInstructions)
		{
			_allInstructions = allInstructions;
			BasicBlocks = new List<BasicBlock>();
		}

		public void ComputePostDominators()
		{
			// Algorithm adapted from Wikipedia - http://en.wikipedia.org/wiki/Dominator_(graph_theory)

			// TODO: Ensure there is a single exit node.

			// Post-dominator of the exit node is the exit node itself.
			var exitBlock = BasicBlocks.Single(x => !x.Successors.Any());
			exitBlock.PostDominators.Add(exitBlock);

			// For all other nodes, set all nodes as the post-dominators.
			var blocksWithoutExitBlock = BasicBlocks.Where(x => x != exitBlock).ToList();
			foreach (var block in blocksWithoutExitBlock)
				block.PostDominators.AddRange(BasicBlocks);

			// Iteratively eliminate nodes that are not post-dominators.
			bool anyChanges;
			do
			{
				anyChanges = false;
				foreach (var block in blocksWithoutExitBlock)
				{
					var successorPostDominators = block.Successors.First().PostDominators.AsEnumerable();
					foreach (var successor in block.Successors.Skip(1))
						successorPostDominators = successorPostDominators.Intersect(successor.PostDominators);
					var postDominators = successorPostDominators.Union(new[] { block }).ToList();

					if (block.PostDominators.Intersect(postDominators).Count() != block.PostDominators.Count)
					{
						block.PostDominators.Clear();
						block.PostDominators.AddRange(postDominators.OrderBy(x => x.Position));
						anyChanges = true;
					}
				}
			} while (anyChanges);
		}
	}
}