using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection.Emit;
using SlimShader.VirtualMachine.Analysis.ExecutableInstructions;
using SlimShader.VirtualMachine.Execution;

namespace SlimShader.VirtualMachine.Jitter
{
    public class JitShaderExecutor : IShaderExecutor
    {
        private delegate IEnumerable<ExecutionResponse> ExecuteShaderDelegate(
            VirtualMachine virtualMachine, ExecutionContext[] executionContexts,
            ExecutableInstruction[] instructions);

        private readonly Dictionary<BytecodeContainer, ExecuteShaderDelegate> _jittedShaderCache;

        public JitShaderExecutor()
        {
            _jittedShaderCache = new Dictionary<BytecodeContainer, ExecuteShaderDelegate>();
        }

        public IEnumerable<ExecutionResponse> Execute(
            VirtualMachine virtualMachine, ExecutionContext[] executionContexts,
            ExecutableInstruction[] instructions)
        {
            // Find existing JITted shader.
            ExecuteShaderDelegate jittedShader;
            if (!_jittedShaderCache.TryGetValue(virtualMachine.Bytecode, out jittedShader))
            {
                // If shader hasn't already been JITted, JIT it now.
                jittedShader = JitCompileShader(virtualMachine.Bytecode);
                _jittedShaderCache.Add(virtualMachine.Bytecode, jittedShader);
            }

            // Execute shader.
            return jittedShader(virtualMachine, executionContexts, instructions);
        }

        private static ExecuteShaderDelegate JitCompileShader(BytecodeContainer bytecode)
        {
            var dynamicMethod = new DynamicMethod("ExecuteShader",
                typeof(IEnumerable<ExecutionResponse>),
                new[]
                {
                    typeof(VirtualMachine),
                    typeof(ExecutionContext[]),
                    typeof(ExecutableInstruction)
                });

            var ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ret);

            return (ExecuteShaderDelegate) dynamicMethod.CreateDelegate(typeof(ExecuteShaderDelegate));
        }
    }
}