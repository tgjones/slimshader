using System;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Shex.Tokens;
using SlimShader.VirtualMachine.Registers;
using SlimShader.VirtualMachine.Util;

namespace SlimShader.VirtualMachine
{
	public class ExecutionContext
	{
		public Number4[][] ConstantBuffers { get; private set; }

		public Number4[] Inputs { get; private set; }
		public Number4[] Outputs { get; private set; }

		public Number4[] Temps { get; private set; }
		public Number4[][] IndexableTemps { get; private set; }

		public ExecutionContext(RequiredRegisters requiredRegisters)
		{
			ConstantBuffers = new Number4[requiredRegisters.ConstantBuffers.Count][];
			for (int i = 0; i < requiredRegisters.ConstantBuffers.Count; i++)
				ConstantBuffers[i] = new Number4[requiredRegisters.ConstantBuffers[i]];

			Inputs = new Number4[requiredRegisters.Inputs];
			Outputs = new Number4[requiredRegisters.Outputs];

			Temps = new Number4[requiredRegisters.Temps];

			IndexableTemps = new Number4[requiredRegisters.IndexableTemps.Count][];
			for (int i = 0; i < requiredRegisters.IndexableTemps.Count; i++)
				IndexableTemps[i] = new Number4[requiredRegisters.IndexableTemps[i]];
		}

		public void GetRegister(OperandType operandType, RegisterIndex registerIndex, out Number4[] register, out int index)
		{
			switch (operandType)
			{
				case OperandType.ConstantBuffer:
					register = ConstantBuffers[registerIndex.Index2D_0];
					index = registerIndex.Index2D_1;
					return;
				case OperandType.Input:
					register = Inputs;
					index = registerIndex.Index1D;
					return;
				case OperandType.Output:
					register = Outputs;
					index = registerIndex.Index1D;
					return;
				case OperandType.Temp:
					register = Temps;
					index = registerIndex.Index1D;
					return;
				case OperandType.IndexableTemp:
					register = IndexableTemps[registerIndex.Index2D_0];
					index = registerIndex.Index2D_1;
					return;
				default:
					throw new ArgumentException("Unsupported operand type: " + operandType);
			}
		}

		private void GetRegister(Operand operand, out Number4[] register, out int index)
		{
			var registerIndex = GetRegisterIndex(operand);
			GetRegister(operand.OperandType, registerIndex, out register, out index);
		}

		/// <summary>
		/// Gets potentially-swizzled value for use on RHS of an operation.
		/// </summary>
		private Number4 GetOperandValue(Operand operand)
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
					GetRegister(operand, out register, out index);
					var swizzledNumber = OperandUtility.ApplyOperandSelectionMode(register[index], operand);
					return OperandUtility.ApplyOperandModifier(swizzledNumber, operand.Modifier);
				default:
					throw new ArgumentException("Unsupported operand type: " + operand.OperandType);
			}
		}

		private RegisterIndex GetRegisterIndex(Operand operand)
		{
			var result = new RegisterIndex();
			switch (operand.IndexDimension)
			{
				case OperandIndexDimension._1D:
					result.Index2D_0 = EvaluateOperandIndex(operand.Indices[0]);
					break;
				case OperandIndexDimension._2D:
					result.Index2D_0 = EvaluateOperandIndex(operand.Indices[0]);
					result.Index2D_1 = EvaluateOperandIndex(operand.Indices[1]);
					break;
			}
			return result;
		}

		private ushort EvaluateOperandIndex(OperandIndex index)
		{
			var result = (ushort) index.Value;
			switch (index.Representation)
			{
				case OperandIndexRepresentation.Immediate32PlusRelative :
				case OperandIndexRepresentation.Immediate64PlusRelative :
				case OperandIndexRepresentation.Relative :
					var operandValue = GetOperandValue(index.Register);
					result += (ushort) operandValue.GetMaskedNumber(index.Register.ComponentMask).UInt;
					break;
			}
			return result;
		}

		private void SetRegisterValue(Operand operand, Number4 value)
		{
			Number4[] register;
			int index;
			GetRegister(operand, out register, out index);
			register[index].WriteMaskedValue(value, operand.ComponentMask);
		}

		public void ExecuteAdd(InstructionToken token)
		{
			Execute(token, (src0, src1) => Number.FromFloat(src0.Float + src1.Float, token.Saturate));
		}

		public void ExecuteDiv(InstructionToken token)
		{
			Execute(token, (src0, src1) => Number.FromFloat(src0.Float / src1.Float, token.Saturate));
		}

		public void ExecuteDp2(InstructionToken token)
		{
			Execute(token, (src0, src1) => Number.FromFloat(
				src0.Number0.Float * src1.Number0.Float
				+ src0.Number1.Float * src1.Number1.Float,
				token.Saturate));
		}

		public void ExecuteDp3(InstructionToken token)
		{
			Execute(token, (src0, src1) => Number.FromFloat(
				src0.Number0.Float * src1.Number0.Float
				+ src0.Number1.Float * src1.Number1.Float
				+ src0.Number2.Float * src1.Number2.Float,
				token.Saturate));
		}

		public void ExecuteDp4(InstructionToken token)
		{
			Execute(token, (src0, src1) => Number.FromFloat(
				src0.Number0.Float * src1.Number0.Float
				+ src0.Number1.Float * src1.Number1.Float
				+ src0.Number2.Float * src1.Number2.Float
				+ src0.Number3.Float * src1.Number3.Float,
				token.Saturate));
		}

		public void ExecuteILt(InstructionToken token)
		{
			Execute(token, (src0, src1) => Number.FromUInt((src0.Int < src1.Int) ? 0xFFFFFFFF : 0x0000000));
		}

		public void ExecuteItoF(InstructionToken token)
		{
			Execute(token, src => Number.FromFloat(Convert.ToSingle(src.Int), token.Saturate));
		}

		public void ExecuteFtoI(InstructionToken token)
		{
			Execute(token, src => Number.FromInt(Convert.ToInt32(src.Float)));
		}

		public void ExecuteFtoU(InstructionToken token)
		{
			Execute(token, src => Number.FromUInt(Convert.ToUInt32(src.Float)));
		}

		public void ExecuteIAdd(InstructionToken token)
		{
			Execute(token, (src0, src1) => Number.FromInt(src0.Int + src1.Int));
		}

		public void ExecuteIGe(InstructionToken token)
		{
			Execute(token, (src0, src1) => Number.FromUInt((src0.Int >= src1.Int) ? 0xFFFFFFFF : 0x0000000));
		}

		public void ExecuteLt(InstructionToken token)
		{
			Execute(token, (src0, src1) => Number.FromUInt((src0.Float < src1.Float) ? 0xFFFFFFFF : 0x0000000));
		}

		public void ExecuteMad(InstructionToken token)
		{
			Execute(token, (src0, src1, src2) => Number.FromFloat((src0.Float * src1.Float) + src2.Float, token.Saturate));
		}

		public void ExecuteMax(InstructionToken token)
		{
			Execute(token, (src0, src1) => Number.FromFloat(Math.Max(src0.Float, src1.Float), token.Saturate));
		}

		public void ExecuteMov(InstructionToken token)
		{
			Execute(token, src => src);
		}

		public void ExecuteMul(InstructionToken token)
		{
			Execute(token, (src0, src1) => Number.FromFloat(src0.Float * src1.Float, token.Saturate));
		}

		public void ExecuteRsq(InstructionToken token)
		{
			Execute(token, src => Number.FromFloat((float) (1.0f / Math.Sqrt(src.Float)), token.Saturate));
		}

		public void ExecuteSqrt(InstructionToken token)
		{
			Execute(token, src => Number.FromFloat((float) Math.Sqrt(src.Float), token.Saturate));
		}

		public void ExecuteUtoF(InstructionToken token)
		{
			Execute(token, src => Number.FromFloat(Convert.ToSingle(src.UInt), token.Saturate));
		}

		public void ExecuteXor(InstructionToken token)
		{
			Execute(token, (src0, src1) => Number.FromUInt(src0.UInt | src1.UInt));
		}

		private void Execute(InstructionToken token, Func<Number, Number> callback)
		{
			var src = GetOperandValue(token.Operands[1]);

			SetRegisterValue(token.Operands[0], new Number4
			{
				Number0 = callback(src.Number0),
				Number1 = callback(src.Number1),
				Number2 = callback(src.Number2),
				Number3 = callback(src.Number3)
			});
		}

		private void Execute(InstructionToken token, Func<Number, Number, Number> callback)
		{
			var src0 = GetOperandValue(token.Operands[1]);
			var src1 = GetOperandValue(token.Operands[2]);

			SetRegisterValue(token.Operands[0], new Number4
			{
				Number0 = callback(src0.Number0, src1.Number0),
				Number1 = callback(src0.Number1, src1.Number1),
				Number2 = callback(src0.Number2, src1.Number2),
				Number3 = callback(src0.Number3, src1.Number3)
			});
		}

		private void Execute(InstructionToken token, Func<Number, Number, Number, Number> callback)
		{
			var src0 = GetOperandValue(token.Operands[1]);
			var src1 = GetOperandValue(token.Operands[2]);
			var src2 = GetOperandValue(token.Operands[3]);

			SetRegisterValue(token.Operands[0], new Number4
			{
				Number0 = callback(src0.Number0, src1.Number0, src2.Number0),
				Number1 = callback(src0.Number1, src1.Number1, src2.Number1),
				Number2 = callback(src0.Number2, src1.Number2, src2.Number2),
				Number3 = callback(src0.Number3, src1.Number3, src2.Number3)
			});
		}

		private void Execute(InstructionToken token, Func<Number4, Number4, Number> callback)
		{
			var src0 = GetOperandValue(token.Operands[1]);
			var src1 = GetOperandValue(token.Operands[2]);
			var result = callback(src0, src1);

			SetRegisterValue(token.Operands[0], new Number4
			{
				Number0 = result,
				Number1 = result,
				Number2 = result,
				Number3 = result
			});
		}
	}
}