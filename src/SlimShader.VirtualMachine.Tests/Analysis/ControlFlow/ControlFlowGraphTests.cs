using NUnit.Framework;
using SlimShader.VirtualMachine.Analysis.ControlFlow;
using SlimShader.VirtualMachine.Analysis.ExplicitBranching;

namespace SlimShader.VirtualMachine.Tests.Analysis.ControlFlow
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

		[Test]
		public void CanComputePostDominators()
		{
			// Arrange.
			// A
			// |\
			// | \
			// B  C
			// |   \
			// |    \
			// D__>__E
			// |    /
			// |   /
			// |  /
			// | /
			// |/
			// F
			var blockA = new BasicBlock(0, null);
			var blockB = new BasicBlock(1, null);
			var blockC = new BasicBlock(2, null);
			var blockD = new BasicBlock(3, null);
			var blockE = new BasicBlock(4, null);
			var blockF = new BasicBlock(5, null);
			blockA.Successors.AddRange(new[] { blockB, blockC });
			blockB.Successors.AddRange(new[] { blockD });
			blockC.Successors.AddRange(new[] { blockE });
			blockD.Successors.AddRange(new[] { blockE, blockF });
			blockE.Successors.AddRange(new[] { blockF });
			var controlFlowGraph = new ControlFlowGraph(null)
			{
				BasicBlocks = { blockA, blockB, blockC, blockD, blockE, blockF }
			};

			// Act.
			controlFlowGraph.ComputePostDominators();

			// Assert.
			Assert.That(blockA.ImmediatePostDominator, Is.EqualTo(blockF));
			Assert.That(blockB.ImmediatePostDominator, Is.EqualTo(blockD));
			Assert.That(blockC.ImmediatePostDominator, Is.EqualTo(blockE));
			Assert.That(blockD.ImmediatePostDominator, Is.EqualTo(blockF));
			Assert.That(blockE.ImmediatePostDominator, Is.EqualTo(blockF));
			Assert.That(blockF.ImmediatePostDominator, Is.Null);
		}
	}
}