using System.Linq;
using NUnit.Framework;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Shex.Tokens;
using SlimShader.VirtualMachine.Analysis.ExplicitBranching;

namespace SlimShader.VirtualMachine.Tests.Analysis.ExplicitBranching
{
	[TestFixture]
	public class ExplicitBranchingRewriterTests
	{
		[Test]
		public void RewritesIfsToBranches()
		{
			// Arrange.
			var instructions = new[]
			{
				CreateInstruction(OpcodeType.Add),
				CreateInstruction(OpcodeType.If),
				CreateInstruction(OpcodeType.Add),
				CreateInstruction(OpcodeType.EndIf),
				CreateInstruction(OpcodeType.Add)
			};

			// Act.
			var rewrittenInstructions = ExplicitBranchingRewriter.Rewrite(instructions).ToList();

			// Assert.
			Assert.That(rewrittenInstructions, Has.Count.EqualTo(4));
			Assert.That(rewrittenInstructions[0], Is.InstanceOf<NormalInstruction>());
			AssertBranchingInstruction(rewrittenInstructions[1], rewrittenInstructions[3], BranchType.Conditional);
			Assert.That(rewrittenInstructions[2], Is.InstanceOf<NormalInstruction>());
			Assert.That(rewrittenInstructions[3], Is.InstanceOf<NormalInstruction>());
		}

		[Test]
		public void RewritesIfElsesToBranches()
		{
			// Arrange.
			var instructions = new[]
			{
				CreateInstruction(OpcodeType.Add),
				CreateInstruction(OpcodeType.If),
				CreateInstruction(OpcodeType.Add),
				CreateInstruction(OpcodeType.Else),
				CreateInstruction(OpcodeType.Mul),
				CreateInstruction(OpcodeType.EndIf),
				CreateInstruction(OpcodeType.Add)
			};

			// Act.
			var rewrittenInstructions = ExplicitBranchingRewriter.Rewrite(instructions).ToList();

			// Assert.
			Assert.That(rewrittenInstructions, Has.Count.EqualTo(6));
			Assert.That(rewrittenInstructions[0], Is.InstanceOf<NormalInstruction>());
			AssertBranchingInstruction(rewrittenInstructions[1], rewrittenInstructions[4], BranchType.Conditional);
			Assert.That(rewrittenInstructions[2], Is.InstanceOf<NormalInstruction>());
			AssertBranchingInstruction(rewrittenInstructions[3], rewrittenInstructions[5], BranchType.Unconditional);
			Assert.That(rewrittenInstructions[4], Is.InstanceOf<NormalInstruction>());
			Assert.That(rewrittenInstructions[5], Is.InstanceOf<NormalInstruction>());
		}

		[Test]
		public void HandlesNestedControlFlowStructures()
		{
			// Arrange.
			var instructions = new[]
			{
				CreateInstruction(OpcodeType.Add),               // 0  => 0
				CreateInstruction(OpcodeType.Loop),              // 1
					CreateInstruction(OpcodeType.Loop),          // 2
						CreateInstruction(OpcodeType.BreakC),    // 3  => 1
					CreateInstruction(OpcodeType.EndLoop),       // 4  => 2
					CreateInstruction(OpcodeType.If),            // 5  => 3
						CreateInstruction(OpcodeType.BreakC),    // 6  => 4
					CreateInstruction(OpcodeType.EndIf),         // 7
					CreateInstruction(OpcodeType.Loop),          // 8
						CreateInstruction(OpcodeType.If),        // 9  => 5
							CreateInstruction(OpcodeType.Break), // 10 => 6
						CreateInstruction(OpcodeType.Else),      // 11 => 7
							CreateInstruction(OpcodeType.Mul),   // 12 => 8
						CreateInstruction(OpcodeType.EndIf),     // 13
					CreateInstruction(OpcodeType.EndLoop),       // 14 => 9
				CreateInstruction(OpcodeType.EndLoop),           // 15 => 10
				CreateInstruction(OpcodeType.Add)                // 16 => 11
			};

			// Act.
			var rewrittenInstructions = ExplicitBranchingRewriter.Rewrite(instructions).ToList();

			// Assert.
			Assert.That(rewrittenInstructions, Has.Count.EqualTo(12));
			Assert.That(rewrittenInstructions[0], Is.InstanceOf<NormalInstruction>());
			AssertBranchingInstruction(rewrittenInstructions[1], rewrittenInstructions[3], BranchType.Conditional);
			AssertBranchingInstruction(rewrittenInstructions[2], rewrittenInstructions[1], BranchType.Unconditional);
			AssertBranchingInstruction(rewrittenInstructions[3], rewrittenInstructions[5], BranchType.Conditional);
			AssertBranchingInstruction(rewrittenInstructions[4], rewrittenInstructions[11], BranchType.Conditional);
			AssertBranchingInstruction(rewrittenInstructions[5], rewrittenInstructions[8], BranchType.Conditional);
			AssertBranchingInstruction(rewrittenInstructions[6], rewrittenInstructions[10], BranchType.Unconditional);
			AssertBranchingInstruction(rewrittenInstructions[7], rewrittenInstructions[9], BranchType.Unconditional);
			Assert.That(rewrittenInstructions[8], Is.InstanceOf<NormalInstruction>());
			AssertBranchingInstruction(rewrittenInstructions[9], rewrittenInstructions[5], BranchType.Unconditional);
			AssertBranchingInstruction(rewrittenInstructions[10], rewrittenInstructions[1], BranchType.Unconditional);
			Assert.That(rewrittenInstructions[11], Is.InstanceOf<NormalInstruction>());
		}

		private static InstructionToken CreateInstruction(OpcodeType opcodeType)
		{
			return new InstructionToken { Header = { OpcodeType = opcodeType } };
		}

		private static void AssertBranchingInstruction(
			InstructionBase instruction, 
			InstructionBase branchTarget,
			BranchType branchType)
		{
			Assert.That(instruction, Is.InstanceOf<BranchingInstruction>());
			Assert.That(((BranchingInstruction) instruction).BranchTarget, Is.EqualTo(branchTarget));
			Assert.That(((BranchingInstruction) instruction).BranchType, Is.EqualTo(branchType));
		}
	}
}