using System.Collections.Generic;
using SlimShader.VirtualMachine.Analysis.ExecutableInstructions;

namespace SlimShader.VirtualMachine.Execution
{
	public interface IShaderExecutor
	{
        IEnumerable<ExecutionResponse> Execute(
            VirtualMachine virtualMachine,
            ExecutionContext[] executionContexts,
            ExecutableInstruction[] instructions);
	}
}