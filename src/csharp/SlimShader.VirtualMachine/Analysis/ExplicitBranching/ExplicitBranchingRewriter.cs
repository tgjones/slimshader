using System.Collections.Generic;
using System.Linq;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Shex.Tokens;

namespace SlimShader.VirtualMachine.Analysis.ExplicitBranching
{
	public static class ExplicitBranchingRewriter
	{
		public static IEnumerable<InstructionBase> Rewrite(IEnumerable<InstructionToken> instructionTokens)
		{
			var instructions = instructionTokens.Select<InstructionToken, InstructionBase>(x =>
			{
				switch (x.Header.OpcodeType)
				{
					case OpcodeType.If:
					case OpcodeType.Loop :
					case OpcodeType.Switch:
					case OpcodeType.BreakC :
						return new BranchingInstruction
						{
							BranchType = BranchType.Conditional,
							InstructionToken = x
						};
					case OpcodeType.Else :
					case OpcodeType.EndLoop:
					case OpcodeType.Break:
						return new BranchingInstruction
						{
							BranchType = BranchType.Unconditional,
							InstructionToken = x
						};
					default:
						return new NormalInstruction
						{
							InstructionToken = x
						};
				}
			}).ToList();

			var controlFlowStack = new Stack<BranchingInstruction>();
			var breakStack = new Stack<List<BranchingInstruction>>();
			for (int i = 0; i < instructions.Count; i++)
			{
				var instruction = instructions[i];
				switch (instruction.InstructionToken.Header.OpcodeType)
				{
					case OpcodeType.Loop:
					case OpcodeType.Switch:
					{
						breakStack.Push(new List<BranchingInstruction>());
						goto case OpcodeType.If;
					}
					case OpcodeType.If:
					{
						controlFlowStack.Push((BranchingInstruction) instruction);
						break;
					}
					case OpcodeType.Else :
					{
						var branchingInstruction = controlFlowStack.Pop();
						branchingInstruction.BranchTarget = instructions[i + 1];
						controlFlowStack.Push((BranchingInstruction) instruction);
						break;
					}
					case OpcodeType.Break:
					case OpcodeType.BreakC:
					{
						breakStack.Peek().Add((BranchingInstruction) instruction);
						break;
					}
					case OpcodeType.EndLoop:
					{
						var breakInstructions = breakStack.Pop();
						foreach (var breakInstruction in breakInstructions)
							breakInstruction.BranchTarget = instructions[i + 1];
						var branchingInstruction = controlFlowStack.Pop();
						((BranchingInstruction) instruction).BranchTarget = branchingInstruction;
						break;
					}
					case OpcodeType.EndSwitch:
					{
						var breakInstructions = breakStack.Pop();
						foreach (var breakInstruction in breakInstructions)
							breakInstruction.BranchTarget = instructions[i + 1];
						goto case OpcodeType.EndIf;
					}
					case OpcodeType.EndIf:
					{
						var branchingInstruction = controlFlowStack.Pop();
						branchingInstruction.BranchTarget = instructions[i + 1];
						break;
					}
				}
			}

			// Remove redundant instructions, making sure to update any branching instructions that have their
			// BranchTarget set to these instructions, to point to the following instruction.
			bool changed;
			do
			{
				changed = false;
				for (int i = 0; i < instructions.Count; i++)
				{
					var instruction = instructions[i];
					var opcodeType = instruction.InstructionToken.Header.OpcodeType;
					if (opcodeType == OpcodeType.EndIf || opcodeType == OpcodeType.EndSwitch || opcodeType == OpcodeType.Loop)
					{
						foreach (var otherInstruction in instructions.OfType<BranchingInstruction>().Where(y => y.BranchTarget == instruction).ToList())
							otherInstruction.BranchTarget = instructions[i + 1];
						instructions.RemoveAt(i);
						changed = true;
						break;
					}
				}
			} while (changed);

			return instructions;
		}
	}
}