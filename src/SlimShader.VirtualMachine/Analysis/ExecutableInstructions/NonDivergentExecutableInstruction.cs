using System.Collections;
using System.Collections.Generic;
using SlimShader.VirtualMachine.Execution;

namespace SlimShader.VirtualMachine.Analysis.ExecutableInstructions
{
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
}