using System.Collections.Generic;
using SlimShader.VirtualMachine.Analysis.ExecutableInstructions;
using SlimShader.VirtualMachine.Execution;

namespace SlimShader.VirtualMachine.Jitter
{
    public class JitShaderExecutor : IShaderExecutor
    {
        public IEnumerable<ExecutionResponse> Execute(
            VirtualMachine virtualMachine, ExecutionContext[] executionContexts,
            ExecutableInstruction[] instructions)
        {
            // TODO
            yield return ExecutionResponse.Finished;
        }
    }
}
