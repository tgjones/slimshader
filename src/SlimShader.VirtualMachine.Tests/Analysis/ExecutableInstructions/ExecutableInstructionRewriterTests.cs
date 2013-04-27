using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
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
				new NormalInstruction { }
			};
			var controlFlowGraph = new ControlFlowGraph(instructions);

			// Act.
			var executableInstructions = ExecutableInstructionRewriter.Rewrite(controlFlowGraph).ToList();

			// Assert.
			Assert.That(executableInstructions, Has.Count.EqualTo(5));
		}
	}
}