using System.Collections.Generic;

namespace SlimShader.VirtualMachine.Execution
{
	public interface IShaderExecutor
	{
		IEnumerable<ExecutionResponse> Execute();
	}
}