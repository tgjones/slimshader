using System.Collections;
using System.Collections.Generic;
using SlimShader.VirtualMachine.Execution;

namespace SlimShader.VirtualMachine.Analysis.ExecutableInstructions
{
    public class NonDivergentExecutableInstruction : ExecutableInstruction
    {
        public int NextPC { get; set; }

        public override bool UpdateDivergenceStack(DivergenceStack divergenceStack, IList<BitArray> activeMasks)
        {
            var topOfDivergenceStack = divergenceStack.Peek();
            if (NextPC == topOfDivergenceStack.ReconvergencePC)
            {
                // Reconvergence.
                divergenceStack.Pop();
                return true;
            }
            else
            {
                // No Divergence.
                topOfDivergenceStack.NextPC = NextPC;
                return false;
            }
        }
    }
}