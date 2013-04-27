using System.Collections.Generic;
using System.Linq;
using SlimShader.VirtualMachine.Analysis.ExplicitBranching;

namespace SlimShader.VirtualMachine.Analysis
{
	public class BasicBlock
	{
		public int Position { get; private set; }
		public IList<InstructionBase> Instructions { get; private set; }
		public List<BasicBlock> Successors { get; private set; }
		
		public IEnumerable<BasicBlock> AllSuccessors
		{
			get
			{
				var allSuccessors = new List<BasicBlock>();
				GetSuccessorsRecursive(this, this, allSuccessors);
				return allSuccessors;
			}
		}

		public List<BasicBlock> PostDominators { get; private set; }

		public BasicBlock ImmediatePostDominator
		{
			get { return PostDominators.FirstOrDefault(x => x != this); }
		}

		public BasicBlock(int position, IList<InstructionBase> instructions)
		{
			Position = position;
			Instructions = instructions;
			Successors = new List<BasicBlock>();
			PostDominators = new List<BasicBlock>();
		}

		private static void GetSuccessorsRecursive(BasicBlock startingBlock, BasicBlock block, List<BasicBlock> allSuccessors)
		{
			foreach (var successor in block.Successors)
				if (successor != startingBlock && !allSuccessors.Contains(successor))
				{
					allSuccessors.Add(successor);
					GetSuccessorsRecursive(startingBlock, successor, allSuccessors);
				}
		}

		public override string ToString()
		{
			return "Position: " + Position;
		}
	}
}