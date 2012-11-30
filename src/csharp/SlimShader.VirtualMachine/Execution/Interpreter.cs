using System;
using System.Collections.Generic;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Shex.Tokens;
using SlimShader.VirtualMachine.Registers;
using SlimShader.VirtualMachine.Util;

namespace SlimShader.VirtualMachine.Execution
{
	public class Interpreter : IShaderExecutor
	{
		private readonly ExecutionContext[] _executionContexts;
		private readonly InstructionToken[] _instructions;

		public Interpreter(ExecutionContext[] executionContexts, InstructionToken[] instructions)
		{
			_executionContexts = executionContexts;
			_instructions = instructions;
		}

		/// <summary>
		/// http://http.developer.nvidia.com/GPUGems2/gpugems2_chapter34.html
		/// http://people.maths.ox.ac.uk/gilesm/pp10/lec2_2x2.pdf
		/// http://stackoverflow.com/questions/10119796/how-does-cuda-compiler-know-the-divergence-behaviour-of-warps
		/// http://www.istc-cc.cmu.edu/publications/papers/2011/SIMD.pdf
		/// http://hal.archives-ouvertes.fr/docs/00/62/26/54/PDF/collange_sympa2011_en.pdf
		/// </summary>
		public IEnumerable<ExecutionResponse> Execute()
		{
			for (int programCounter = 0; programCounter < _instructions.Length; programCounter++)
			{
				InstructionToken token = _instructions[programCounter];
				switch (token.Header.OpcodeType)
				{
					case OpcodeType.Add:
						Execute(t => Execute(t, token, (src0, src1) => Number.FromFloat(src0.Float + src1.Float, token.Saturate)));
						break;
					case OpcodeType.Cut :
					case OpcodeType.CutStream:
						yield return ExecutionResponse.Cut;
						break;
					case OpcodeType.Div:
						Execute(t => Execute(t, token, (src0, src1) => Number.FromFloat(src0.Float / src1.Float, token.Saturate)));
						break;
					case OpcodeType.Dp2:
						Execute(t => Execute(t, token, (src0, src1) => Number.FromFloat(
							src0.Number0.Float * src1.Number0.Float + src0.Number1.Float * src1.Number1.Float,
							token.Saturate)));
						break;
					case OpcodeType.Dp3:
						Execute(t => Execute(t, token, (src0, src1) => Number.FromFloat(
							src0.Number0.Float * src1.Number0.Float
								+ src0.Number1.Float * src1.Number1.Float
								+ src0.Number2.Float * src1.Number2.Float,
							token.Saturate)));
						break;
					case OpcodeType.Dp4:
						Execute(t => Execute(t, token, (src0, src1) => Number.FromFloat(
							src0.Number0.Float * src1.Number0.Float
								+ src0.Number1.Float * src1.Number1.Float
								+ src0.Number2.Float * src1.Number2.Float
								+ src0.Number3.Float * src1.Number3.Float,
							token.Saturate)));
						break;
					case OpcodeType.Emit:
					case OpcodeType.EmitStream :
						yield return ExecutionResponse.Emit;
						break;
					case OpcodeType.EndSwitch:
						break;
					case OpcodeType.ILt:
						Execute(t => Execute(t, token, (src0, src1) => Number.FromUInt((src0.Int < src1.Int) ? 0xFFFFFFFF : 0x0000000)));
						break;
					case OpcodeType.EndLoop:
						programCounter += token.LinkedInstructionOffset;
						break;
					case OpcodeType.ItoF:
						Execute(t => Execute(t, token, src => Number.FromFloat(Convert.ToSingle(src.Int), token.Saturate)));
						break;
					case OpcodeType.FtoI:
						Execute(t => Execute(t, token, src => Number.FromInt(Convert.ToInt32(src.Float))));
						break;
					case OpcodeType.FtoU:
						Execute(t => Execute(t, token, src => Number.FromUInt(Convert.ToUInt32(src.Float))));
						break;
					case OpcodeType.IAdd:
						Execute(t => Execute(t, token, (src0, src1) => Number.FromInt(src0.Int + src1.Int)));
						break;
					case OpcodeType.IGe:
						Execute(t => Execute(t, token, (src0, src1) => Number.FromUInt((src0.Int >= src1.Int) ? 0xFFFFFFFF : 0x0000000)));
						break;
					case OpcodeType.Loop:
						break;
					case OpcodeType.Lt:
						Execute(t => Execute(t, token, (src0, src1) => Number.FromUInt((src0.Float < src1.Float) ? 0xFFFFFFFF : 0x0000000)));
						break;
					case OpcodeType.Mad:
						Execute(t => Execute(t, token, (src0, src1, src2) => Number.FromFloat((src0.Float * src1.Float) + src2.Float, token.Saturate)));
						break;
					case OpcodeType.Max:
						Execute(t => Execute(t, token, (src0, src1) => Number.FromFloat(Math.Max(src0.Float, src1.Float), token.Saturate)));
						break;
					case OpcodeType.Mov:
						Execute(t => Execute(t, token, src => src));
						break;
					case OpcodeType.Mul:
						Execute(t => Execute(t, token, (src0, src1) => Number.FromFloat(src0.Float * src1.Float, token.Saturate)));
						break;
					case OpcodeType.Ret:
						break;
					case OpcodeType.Rsq:
						Execute(t => Execute(t, token, src => Number.FromFloat((float)(1.0f / Math.Sqrt(src.Float)), token.Saturate)));
						break;
					case OpcodeType.Sqrt:
						Execute(t => Execute(t, token, src => Number.FromFloat((float)Math.Sqrt(src.Float), token.Saturate)));
						break;
					case OpcodeType.Utof:
						Execute(t => Execute(t, token, src => Number.FromFloat(Convert.ToSingle(src.UInt), token.Saturate)));
						break;
					case OpcodeType.Xor:
						Execute(t => Execute(t, token, (src0, src1) => Number.FromUInt(src0.UInt | src1.UInt)));
						break;
					default:
						throw new InvalidOperationException(token.Header.OpcodeType + " is not yet supported.");
				}
			}
			yield return ExecutionResponse.Finished;
		}

		private void Execute(Action<ExecutionContext> callback)
		{
			foreach (var thread in _executionContexts)
				callback(thread);
		}

		private void GetRegister(ExecutionContext context, Operand operand, out Number4[] register, out int index)
		{
			var registerIndex = GetRegisterIndex(context, operand);
			context.GetRegister(operand.OperandType, registerIndex, out register, out index);
		}

		/// <summary>
		/// Gets potentially-swizzled value for use on RHS of an operation.
		/// </summary>
		private Number4 GetOperandValue(ExecutionContext context, Operand operand)
		{
			switch (operand.OperandType)
			{
				case OperandType.Immediate32:
				case OperandType.Immediate64:
					return OperandUtility.ApplyOperandModifier(operand.ImmediateValues, operand.Modifier);
				case OperandType.ConstantBuffer:
				case OperandType.IndexableTemp:
				case OperandType.Input:
				case OperandType.Temp:
					Number4[] register;
					int index;
					GetRegister(context, operand, out register, out index);
					var swizzledNumber = OperandUtility.ApplyOperandSelectionMode(register[index], operand);
					return OperandUtility.ApplyOperandModifier(swizzledNumber, operand.Modifier);
				default:
					throw new ArgumentException("Unsupported operand type: " + operand.OperandType);
			}
		}

		private RegisterIndex GetRegisterIndex(ExecutionContext context, Operand operand)
		{
			var result = new RegisterIndex();
			switch (operand.IndexDimension)
			{
				case OperandIndexDimension._1D:
					result.Index1D = EvaluateOperandIndex(context, operand.Indices[0]);
					break;
				case OperandIndexDimension._2D:
					result.Index2D_0 = EvaluateOperandIndex(context, operand.Indices[0]);
					result.Index2D_1 = EvaluateOperandIndex(context, operand.Indices[1]);
					break;
			}
			return result;
		}

		private ushort EvaluateOperandIndex(ExecutionContext context, OperandIndex index)
		{
			var result = (ushort)index.Value;
			switch (index.Representation)
			{
				case OperandIndexRepresentation.Immediate32PlusRelative:
				case OperandIndexRepresentation.Immediate64PlusRelative:
				case OperandIndexRepresentation.Relative:
					var operandValue = GetOperandValue(context, index.Register);
					result += (ushort)operandValue.GetMaskedNumber(index.Register.ComponentMask).UInt;
					break;
			}
			return result;
		}

		private void SetRegisterValue(ExecutionContext context, Operand operand, Number4 value)
		{
			Number4[] register;
			int index;
			GetRegister(context, operand, out register, out index);
			register[index].WriteMaskedValue(value, operand.ComponentMask);
		}

		private void Execute(ExecutionContext context, InstructionToken token, Func<Number, Number> callback)
		{
			var src = GetOperandValue(context, token.Operands[1]);

			SetRegisterValue(context, token.Operands[0], new Number4
			{
				Number0 = callback(src.Number0),
				Number1 = callback(src.Number1),
				Number2 = callback(src.Number2),
				Number3 = callback(src.Number3)
			});
		}

		private void Execute(ExecutionContext context, InstructionToken token, Func<Number, Number, Number> callback)
		{
			var src0 = GetOperandValue(context, token.Operands[1]);
			var src1 = GetOperandValue(context, token.Operands[2]);

			SetRegisterValue(context, token.Operands[0], new Number4
			{
				Number0 = callback(src0.Number0, src1.Number0),
				Number1 = callback(src0.Number1, src1.Number1),
				Number2 = callback(src0.Number2, src1.Number2),
				Number3 = callback(src0.Number3, src1.Number3)
			});
		}

		private void Execute(ExecutionContext context, InstructionToken token, Func<Number, Number, Number, Number> callback)
		{
			var src0 = GetOperandValue(context, token.Operands[1]);
			var src1 = GetOperandValue(context, token.Operands[2]);
			var src2 = GetOperandValue(context, token.Operands[3]);

			SetRegisterValue(context, token.Operands[0], new Number4
			{
				Number0 = callback(src0.Number0, src1.Number0, src2.Number0),
				Number1 = callback(src0.Number1, src1.Number1, src2.Number1),
				Number2 = callback(src0.Number2, src1.Number2, src2.Number2),
				Number3 = callback(src0.Number3, src1.Number3, src2.Number3)
			});
		}

		private void Execute(ExecutionContext context, InstructionToken token, Func<Number4, Number4, Number> callback)
		{
			var src0 = GetOperandValue(context, token.Operands[1]);
			var src1 = GetOperandValue(context, token.Operands[2]);
			var result = callback(src0, src1);

			SetRegisterValue(context, token.Operands[0], new Number4
			{
				Number0 = result,
				Number1 = result,
				Number2 = result,
				Number3 = result
			});
		}
	}
}