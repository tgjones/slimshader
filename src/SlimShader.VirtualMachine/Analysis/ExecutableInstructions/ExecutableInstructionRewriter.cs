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
			foreach (var explicitBranchingInsturction in controlFlowGraph.AllInstructions)
			{
				int thisPC = GetInstructionIndex(controlFlowGraph.AllInstructions, explicitBranchingInsturction);

				if (explicitBranchingInsturction is BranchingInstruction && !explicitBranchingInsturction.IsUnconditionalBranch)
				{
					var branchingInstruction = (BranchingInstruction) explicitBranchingInsturction;
					yield return new DivergentExecutableInstruction
					{
						ReconvergencePC = GetReconvergencePoint(controlFlowGraph, branchingInstruction),
						NextPCs = new List<int>
						{
							GetInstructionIndex(controlFlowGraph.AllInstructions, ((BranchingInstruction) explicitBranchingInsturction).BranchTarget),
							thisPC + 1
						},

						OpcodeType = MapOpcodeType(branchingInstruction.InstructionToken.Header.OpcodeType),
						Operands = explicitBranchingInsturction.InstructionToken.Operands,
						Saturate = explicitBranchingInsturction.InstructionToken.Saturate,
						TestBoolean = explicitBranchingInsturction.InstructionToken.TestBoolean
					};
				}
				else
				{
					int nextPC;
					if (explicitBranchingInsturction.IsUnconditionalBranch)
						nextPC = GetInstructionIndex(controlFlowGraph.AllInstructions, ((BranchingInstruction) explicitBranchingInsturction).BranchTarget);
					else
						nextPC = thisPC + 1;

					yield return new NonDivergentExecutableInstruction
					{
						NextPC = nextPC,

						OpcodeType = MapOpcodeType(explicitBranchingInsturction.InstructionToken.Header.OpcodeType),
						Operands = explicitBranchingInsturction.InstructionToken.Operands,
						Saturate = explicitBranchingInsturction.InstructionToken.Saturate,
						TestBoolean = explicitBranchingInsturction.InstructionToken.TestBoolean
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
			return (ExecutableOpcodeType) Enum.ToObject(typeof (ExecutableOpcodeType), (int) opcodeType);
		}
	}
}