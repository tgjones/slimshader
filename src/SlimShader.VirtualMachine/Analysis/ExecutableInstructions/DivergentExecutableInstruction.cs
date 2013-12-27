using System.Collections;
using System.Collections.Generic;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Shex.Tokens;
using SlimShader.VirtualMachine.Execution;
using SlimShader.VirtualMachine.Util;

namespace SlimShader.VirtualMachine.Analysis.ExecutableInstructions
{
    public class DivergentExecutableInstruction : ExecutableInstruction
    {
		private readonly bool _branchIfPositive;

	    public List<int> NextPCs { get; set; }
        public int ReconvergencePC { get; set; }

	    public override InstructionTestBoolean TestBoolean
	    {
		    get
		    {
				if (_branchIfPositive)
					return base.TestBoolean;

				if (base.TestBoolean == InstructionTestBoolean.NonZero)
					return InstructionTestBoolean.Zero;
			    return InstructionTestBoolean.NonZero;
		    }
	    }

		public DivergentExecutableInstruction(InstructionToken instructionToken, bool branchIfPositive)
            : base(instructionToken)
        {
			_branchIfPositive = branchIfPositive;
        }

	    public override bool UpdateDivergenceStack(DivergenceStack divergenceStack, IList<BitArray> activeMasks)
        {
            divergenceStack.Peek().NextPC = ReconvergencePC;
            for (var i = 0; i < NextPCs.Count; i++)
                if (activeMasks[i].Any())
                    divergenceStack.Push(NextPCs[i], activeMasks[i], ReconvergencePC);
            return true;
        }
    }
}