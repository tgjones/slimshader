using System;
using System.Linq;
using System.Text;
using SlimShader.VirtualMachine.Analysis.ExecutableInstructions;

namespace SlimShader.VirtualMachine.Jitter
{
    internal static class ShaderCodeGenerator
    {
        public static string Generate(ExecutableInstruction[] instructions)
        {
            var instructionsCode = new StringBuilder();
            foreach (var instruction in instructions)
            {
                instructionsCode.AppendLineIndent(2, "// " + instruction);
                GenerateInstructionCode(instructionsCode, instruction);
                instructionsCode.AppendLine();
            }

            return @"
using System.Collections.Generic;
using SlimShader.VirtualMachine;
using SlimShader.VirtualMachine.Analysis.ExecutableInstructions;

public static class DynamicShaderExecutor
{
    public static IEnumerable<ExecutionResponse> Execute(
        VirtualMachine virtualMachine, ExecutionContext[] executionContexts,
        ExecutableInstruction[] instructions)
    {
" + instructionsCode + @"
    }
}";
        }

        private static void GenerateInstructionCode(StringBuilder sb, ExecutableInstruction instruction)
        {
            switch (instruction.OpcodeType)
            {
                case Execution.ExecutableOpcodeType.Mov :
                    sb.AppendLineIndent(2, "yield return ExecutionResponse.Finished;");
                    break;
                case Execution.ExecutableOpcodeType.Ret :
                    sb.AppendLineIndent(2, "yield return ExecutionResponse.Finished;");
                    break;
                default :
                    throw new InvalidOperationException(instruction.OpcodeType + " is not yet supported.");
            }
        }

        private static void AppendLineIndent(this StringBuilder sb, int indent, string line)
        {
            sb.Append(new string(Enumerable.Repeat(' ', indent * 4).ToArray()));
            sb.AppendLine(line);
        }
    }
}