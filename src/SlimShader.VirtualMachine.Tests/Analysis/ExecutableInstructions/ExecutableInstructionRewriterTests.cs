using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SlimShader.Chunks.Shex.Tokens;
using SlimShader.VirtualMachine.Analysis.ControlFlow;
using SlimShader.VirtualMachine.Analysis.ExecutableInstructions;
using SlimShader.VirtualMachine.Analysis.ExplicitBranching;

namespace SlimShader.VirtualMachine.Tests.Analysis.ExecutableInstructions
{
	[TestFixture]
	public class ExecutableInstructionRewriterTests
	{
		[Test]
		public void SetsNextProgramCounterForEveryInstruction()
		{
			// Arrange.
			var instructions = new List<InstructionBase>
			{
				CreateInstruction<NormalInstruction>(),    // 0
				CreateInstruction<BranchingInstruction>(), // 1 Unconditional branch to [3]
				CreateInstruction<NormalInstruction>(),    // 2 Skipped
				CreateInstruction<BranchingInstruction>(), // 3 Conditional branch to [5]
				CreateInstruction<NormalInstruction>(),    // 4
				CreateInstruction<NormalInstruction>()     // 5
			};
			((BranchingInstruction) instructions[1]).BranchType = BranchType.Unconditional;
			((BranchingInstruction) instructions[1]).BranchTarget = instructions[3];
			((BranchingInstruction) instructions[3]).BranchType = BranchType.Conditional;
			((BranchingInstruction) instructions[3]).BranchTarget = instructions[5];
			var controlFlowGraph = ControlFlowGraph.FromInstructions(instructions);

			// Act.
			var executableInstructions = ExecutableInstructionRewriter.Rewrite(controlFlowGraph).ToList();

			// Assert.
			Assert.That(executableInstructions, Has.Count.EqualTo(6));
			Assert.That(((NonDivergentExecutableInstruction) executableInstructions[0]).NextPC, Is.EqualTo(1));
			Assert.That(((NonDivergentExecutableInstruction) executableInstructions[1]).NextPC, Is.EqualTo(3));
			Assert.That(((NonDivergentExecutableInstruction) executableInstructions[2]).NextPC, Is.EqualTo(3));
			Assert.That(((DivergentExecutableInstruction) executableInstructions[3]).NextPCs, Is.EquivalentTo(new[] { 4, 5 }));
			Assert.That(((DivergentExecutableInstruction) executableInstructions[3]).ReconvergencePC, Is.EqualTo(5));
			Assert.That(((NonDivergentExecutableInstruction) executableInstructions[4]).NextPC, Is.EqualTo(5));
			Assert.That(((NonDivergentExecutableInstruction) executableInstructions[5]).NextPC, Is.EqualTo(6));
		}

		private static T CreateInstruction<T>()
			where T : InstructionBase, new()
		{
			return new T { InstructionToken = new InstructionToken() };
		}
	}
}