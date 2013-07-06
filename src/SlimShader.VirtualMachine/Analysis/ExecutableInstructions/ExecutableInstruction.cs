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

		public abstract bool UpdateDivergenceStack(DivergenceStack divergenceStack, IList<BitArray> activeMasks);
	}
}