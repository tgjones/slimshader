using System;
using System.Collections.Generic;
using System.Linq;
using SlimShader.Chunks.Shex;
using SlimShader.VirtualMachine.Analysis.ControlFlow;
using SlimShader.VirtualMachine.Analysis.ExplicitBranching;
using SlimShader.VirtualMachine.Execution;

namespace SlimShader.VirtualMachine.Analysis.ExecutableInstructions
{
	public static class ExecutableInstructionRewriter
	{
		public static IEnumerable<ExecutableInstruction> Rewrite(ControlFlowGraph controlFlowGraph)
		{
			foreach (var explicitBranchingInstruction in controlFlowGraph.AllInstructions)
			{
				int thisPC = GetInstructionIndex(controlFlowGraph.AllInstructions, explicitBranchingInstruction);

				if (explicitBranchingInstruction is BranchingInstruction && !explicitBranchingInstruction.IsUnconditionalBranch)
				{
					var branchingInstruction = (BranchingInstruction) explicitBranchingInstruction;
					yield return new DivergentExecutableInstruction(explicitBranchingInstruction.InstructionToken)
					{
						ReconvergencePC = GetReconvergencePoint(controlFlowGraph, branchingInstruction),
						NextPCs = new List<int>
						{
							GetInstructionIndex(controlFlowGraph.AllInstructions, ((BranchingInstruction) explicitBranchingInstruction).BranchTarget),
							thisPC + 1
						},

						OpcodeType = ExecutableOpcodeType.BranchC
					};
				}
				else
				{
					ExecutableOpcodeType opcodeType;
					int nextPC;
					if (explicitBranchingInstruction.IsUnconditionalBranch)
					{
						opcodeType = ExecutableOpcodeType.Branch;
						nextPC = GetInstructionIndex(controlFlowGraph.AllInstructions, ((BranchingInstruction) explicitBranchingInstruction).BranchTarget);
					}
					else
					{
						opcodeType = MapOpcodeType(explicitBranchingInstruction.InstructionToken.Header.OpcodeType);
						nextPC = thisPC + 1;
					}

					yield return new NonDivergentExecutableInstruction(explicitBranchingInstruction.InstructionToken)
					{
						NextPC = nextPC,
						OpcodeType = opcodeType,
					};
				}
			}
		}

		private static int GetReconvergencePoint(ControlFlowGraph cfg, BranchingInstruction branchingInstruction)
		{
			// Find BasicBlock that contains the branching instruction.
			var sourceBasicBlock = cfg.BasicBlocks.Single(x => x.Instructions.Contains(branchingInstruction));

			// Get first instruction in the immediate post dominator block.
			var firstInstruction = sourceBasicBlock.ImmediatePostDominator.Instructions.First();

			// Return index of this instruction.
			return GetInstructionIndex(cfg.AllInstructions, firstInstruction);
		}

		private static int GetInstructionIndex(IList<InstructionBase> instructions, InstructionBase instruction)
		{
			return instructions.IndexOf(instruction);
		}

		private static ExecutableOpcodeType MapOpcodeType(OpcodeType opcodeType)
		{
			return (ExecutableOpcodeType) Enum.Parse(typeof (ExecutableOpcodeType), opcodeType.ToString());
		}
	}
}