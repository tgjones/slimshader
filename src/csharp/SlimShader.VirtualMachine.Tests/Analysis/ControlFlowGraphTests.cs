using NUnit.Framework;
using SlimShader.VirtualMachine.Analysis;
using SlimShader.VirtualMachine.Analysis.ExplicitBranching;

namespace SlimShader.VirtualMachine.Tests.Analysis
{
	[TestFixture]
	public class ControlFlowGraphTests
	{
		[Test]
		public void CreatesSingleBasicBlockWhenNoBranchingInstructions()
		{
			// Arrange.
			var instructions = new[]
			{
				new NormalInstruction(),
				new NormalInstruction(),
				new NormalInstruction()
			};

			// Act.
			var controlFlowGraph = ControlFlowGraph.FromInstructions(instructions);

			// Assert.
			Assert.That(controlFlowGraph.BasicBlocks, Is.Not.Null);
			Assert.That(controlFlowGraph.BasicBlocks, Has.Count.EqualTo(1));
			Assert.That(controlFlowGraph.BasicBlocks[0].Position, Is.EqualTo(0));
			Assert.That(controlFlowGraph.BasicBlocks[0].Instructions, Has.Count.EqualTo(3));
			Assert.That(controlFlowGraph.BasicBlocks[0].Successors, Is.Empty);
		}

		[Test]
		public void CreatesMultipleBasicBlocksWhenBranchingInstructionsArePresent()
		{
			// Arrange.
			var instructions = new InstructionBase[]
			{
				new NormalInstruction(),
				new NormalInstruction(),
				new BranchingInstruction { BranchType = BranchType.Conditional },
				new NormalInstruction()
			};

			// Act.
			var controlFlowGraph = ControlFlowGraph.FromInstructions(instructions);

			// Assert.
			Assert.That(controlFlowGraph.BasicBlocks, Is.Not.Null);
			Assert.That(controlFlowGraph.BasicBlocks, Has.Count.EqualTo(2));
			Assert.That(controlFlowGraph.BasicBlocks[0].Position, Is.EqualTo(0));
			Assert.That(controlFlowGraph.BasicBlocks[0].Instructions, Has.Count.EqualTo(3));
			Assert.That(controlFlowGraph.BasicBlocks[0].Successors, Has.Count.EqualTo(1));
			Assert.That(controlFlowGraph.BasicBlocks[0].Successors[0], Is.EqualTo(controlFlowGraph.BasicBlocks[1]));
			Assert.That(controlFlowGraph.BasicBlocks[1].Position, Is.EqualTo(1));
			Assert.That(controlFlowGraph.BasicBlocks[1].Instructions, Has.Count.EqualTo(1));
			Assert.That(controlFlowGraph.BasicBlocks[1].Successors, Is.Empty);
		}
	}
}