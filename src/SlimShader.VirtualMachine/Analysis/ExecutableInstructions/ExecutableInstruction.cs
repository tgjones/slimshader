using System.Collections;
using System.Collections.Generic;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Shex.Tokens;
using SlimShader.VirtualMachine.Execution;

namespace SlimShader.VirtualMachine.Analysis.ExecutableInstructions
{
	public abstract class ExecutableInstruction
	{
	    private readonly InstructionToken _instructionToken;

	    public ExecutableOpcodeType OpcodeType { get; set; }

	    public bool Saturate
	    {
	        get { return _instructionToken.Saturate; }
	    }

	    public InstructionTestBoolean TestBoolean
	    {
	        get { return _instructionToken.TestBoolean; }
	    }

	    public List<Operand> Operands
	    {
	        get { return _instructionToken.Operands; }
	    }

	    protected ExecutableInstruction(InstructionToken instructionToken)
        {
            _instructionToken = instructionToken;
        }

	    public abstract bool UpdateDivergenceStack(DivergenceStack divergenceStack, IList<BitArray> activeMasks);

        public override string ToString()
        {
            return _instructionToken.ToString();
        }
	}
}