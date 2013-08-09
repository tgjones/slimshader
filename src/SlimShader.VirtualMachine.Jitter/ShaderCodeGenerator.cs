using System;
using System.Linq;
using System.Text;
using SlimShader.Chunks.Common;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Shex.Tokens;
using SlimShader.VirtualMachine.Analysis.ExecutableInstructions;
using SlimShader.VirtualMachine.Util;

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
using SlimShader;
using SlimShader.VirtualMachine;
using SlimShader.VirtualMachine.Analysis.ExecutableInstructions;
using SlimShader.VirtualMachine.Execution;

public static class DynamicShaderExecutor
{
    public static IEnumerable<ExecutionResponse> Execute(
        VirtualMachine virtualMachine, ExecutionContext[] executionContexts,
        ExecutableInstruction[] instructions)
    {
        var warp = new Warp(executionContexts.Length);
        var activeExecutionContexts = Warp.GetActiveExecutionContexts(executionContexts, warp.DivergenceStack.Peek());

" + instructionsCode + @"
    }
}";
        }

        private static void GenerateInstructionCode(StringBuilder sb, ExecutableInstruction instruction)
        {
            switch (instruction.OpcodeType)
            {
                case Execution.ExecutableOpcodeType.Mov :
                    sb.AppendLineIndent(2, "foreach (var context in activeExecutionContexts)");
                    sb.AppendLineIndent(2, "{");
                    sb.AppendLineIndent(2, "    var src0 = {0};", GenerateGetOperandValue(instruction.Operands[1], NumberType.Float));
                    sb.AppendLineIndent(2, "    var result = InstructionImplementations.Mov({0}, ref src0);", instruction.Saturate.ToString().ToLower());
                    GenerateSetRegisterValue(sb, instruction.Operands[0]);
                    sb.AppendLineIndent(2, "}");
                    break;
                case Execution.ExecutableOpcodeType.Ret :
                    sb.AppendLineIndent(2, "yield return ExecutionResponse.Finished;");
                    break;
                default :
                    throw new InvalidOperationException(instruction.OpcodeType + " is not yet supported.");
            }
        }

        private static void GenerateSetRegisterValue(StringBuilder sb, Operand operand)
        {
            var registerName = GetRegisterName(operand.OperandType);
            var registerIndex = GetRegisterIndex(operand);

            if (operand.ComponentMask.HasFlag(ComponentMask.X))
                sb.AppendLineIndent(2, "    context.{0}{1}.Number0 = result.Number0;", registerName, registerIndex);
            if (operand.ComponentMask.HasFlag(ComponentMask.Y))
                sb.AppendLineIndent(2, "    context.{0}{1}.Number1 = result.Number1;", registerName, registerIndex);
            if (operand.ComponentMask.HasFlag(ComponentMask.Z))
                sb.AppendLineIndent(2, "    context.{0}{1}.Number2 = result.Number2;", registerName, registerIndex);
            if (operand.ComponentMask.HasFlag(ComponentMask.W))
                sb.AppendLineIndent(2, "    context.{0}{1}.Number3 = result.Number3;", registerName, registerIndex);
        }

        private static string GetRegisterIndex(Operand operand)
        {
            switch (operand.IndexDimension)
            {
                case OperandIndexDimension._1D:
                    return string.Format("[{0}]", 
                        GetRegisterIndex(operand.Indices[0]));
                case OperandIndexDimension._2D:
                    return string.Format("[{0}][{1}]", 
                        GetRegisterIndex(operand.Indices[0]),
                        GetRegisterIndex(operand.Indices[1]));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static string GetRegisterIndex(OperandIndex index)
        {
            string result = index.Value.ToString();
            switch (index.Representation)
            {
                case OperandIndexRepresentation.Immediate32PlusRelative:
                case OperandIndexRepresentation.Immediate64PlusRelative:
                case OperandIndexRepresentation.Relative:
                    throw new NotImplementedException();
            }
            return result;
        }

        private static string GetRegisterName(OperandType operandType)
        {
            switch (operandType)
            {
                case OperandType.Temp:
                case OperandType.Input:
                case OperandType.Output:
                case OperandType.IndexableTemp:
                case OperandType.ConstantBuffer:
                    return operandType.ToString() + "s";
                default:
                    throw new ArgumentOutOfRangeException("operandType");
            }
        }

        private static string GenerateGetOperandValue(Operand operand, NumberType numberType)
        {
            switch (operand.OperandType)
            {
                case OperandType.Immediate32 :
                    var value = OperandUtility.ApplyOperandModifier(operand.ImmediateValues, numberType, operand.Modifier);
                    return string.Format("new Number4({0}f, {1}f, {2}f, {3}f)",
                        value.Float0, value.Float1, value.Float2, value.Float3);
                default:
                    throw new ArgumentException("Unsupported operand type: " + operand.OperandType);
            }
        }

        private static void AppendLineIndent(this StringBuilder sb, int indent, string line)
        {
            sb.Append(new string(Enumerable.Repeat(' ', indent * 4).ToArray()));
            sb.AppendLine(line);
        }

        private static void AppendLineIndent(this StringBuilder sb, int indent, string format, params object[] args)
        {
            sb.Append(new string(Enumerable.Repeat(' ', indent * 4).ToArray()));
            sb.AppendLine(string.Format(format, args));
        }
    }
}