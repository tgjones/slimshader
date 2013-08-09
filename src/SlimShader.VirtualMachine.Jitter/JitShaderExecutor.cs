using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;
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
            var assemblyReferences = new[]
            {
                MetadataReference.CreateAssemblyReference("mscorlib"),
                new MetadataFileReference(typeof(VirtualMachine).Assembly.Location)
            };

            var outputName = "DynamicShader";
            var code = @"
                using System.Collections.Generic;
                using SlimShader.VirtualMachine;
                using SlimShader.VirtualMachine.Analysis.ExecutableInstructions;

                public static class DynamicShaderExecutor
                {
                    public static IEnumerable<ExecutionResponse> Execute(
                        VirtualMachine virtualMachine, ExecutionContext[] executionContexts,
                        ExecutableInstruction[] instructions)
                    {
                        // TODO
                        yield return ExecutionResponse.Finished;
                    }
                }
                ";
            
            var compilation = Compilation.Create(outputName)
                .WithOptions(new CompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(assemblyReferences)
                .AddSyntaxTrees(SyntaxTree.ParseText(code));

            var moduleBuilder = AppDomain.CurrentDomain
                .DefineDynamicAssembly(new AssemblyName(outputName),
                    AssemblyBuilderAccess.RunAndCollect)
                .DefineDynamicModule(outputName);

            var compilationResult = compilation.Emit(moduleBuilder);
            if (!compilationResult.Success)
            {
                var errorMessage = string.Empty;
                foreach (var diagnostic in compilationResult.Diagnostics)
                    errorMessage += diagnostic + Environment.NewLine;
                throw new Exception(errorMessage);
            }

            var dynamicClass = moduleBuilder.GetType("DynamicShaderExecutor", false, true);
            var dynamicMethod = dynamicClass.GetMethod("Execute");

            return (ExecuteShaderDelegate) dynamicMethod.CreateDelegate(typeof(ExecuteShaderDelegate));
        }
    }
}