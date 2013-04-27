using System.Collections;
using System.Collections.Generic;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Shex.Tokens;
using SlimShader.VirtualMachine.Execution;

namespace SlimShader.VirtualMachine.Analysis.ExecutableInstructions
{
	public abstract class ExecutableInstruction
	{
		public ExecutableOpcodeType OpcodeType { get; set; }
		public bool Saturate { get; set; }
		public InstructionTestBoolean TestBoolean { get; set; }
		public List<Operand> Operands { get; set; }

		public abstract void UpdateDivergenceStack(DivergenceStack divergenceStack, IList<BitArray> activeMasks);
	}

	public class NonDivergentExecutableInstruction : ExecutableInstruction
	{
		public int NextPC { get; set; }

		public override void UpdateDivergenceStack(DivergenceStack divergenceStack, IList<BitArray> activeMasks)
		{
			var topOfDivergenceStack = divergenceStack.Peek();
			if (NextPC == topOfDivergenceStack.ReconvergencePC)
			{
				// Reconvergence.
				divergenceStack.Pop();
			}
			else
			{
				// No Divergence.
				topOfDivergenceStack.NextPC = NextPC;
			}
		}
	}

	public class DivergentExecutableInstruction : ExecutableInstruction
	{
		public List<int> NextPCs { get; set; }
		public int ReconvergencePC { get; set; }

		public override void UpdateDivergenceStack(DivergenceStack divergenceStack, IList<BitArray> activeMasks)
		{
			// TODO: Check whether all threads in warp have taken the same branch.
			divergenceStack.Peek().NextPC = ReconvergencePC;
			for (int i = 0; i < NextPCs.Count; i++)
				divergenceStack.Push(NextPCs[i], activeMasks[i], ReconvergencePC);
		}
	}
}