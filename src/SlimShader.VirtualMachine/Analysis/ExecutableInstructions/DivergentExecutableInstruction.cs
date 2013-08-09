using System.Collections;
using System.Collections.Generic;
using SlimShader.Chunks.Shex.Tokens;
using SlimShader.VirtualMachine.Execution;
using SlimShader.VirtualMachine.Util;

namespace SlimShader.VirtualMachine.Analysis.ExecutableInstructions
{
    public class DivergentExecutableInstruction : ExecutableInstruction
    {
        public List<int> NextPCs { get; set; }
        public int ReconvergencePC { get; set; }

        public DivergentExecutableInstruction(InstructionToken instructionToken)
            : base(instructionToken)
        {
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