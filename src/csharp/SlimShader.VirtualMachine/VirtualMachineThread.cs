using System;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Shex.Tokens;

namespace SlimShader.VirtualMachine
{
	public class VirtualMachineThread
	{
		private readonly int _contextIndex;
		private readonly GlobalMemory _globalMemory;
		private readonly Number4[][] _indexableTempRegisters;
		private readonly Number4[] _tempRegisters;

		public int PerThreadProgramCounter { get; private set; }

		public VirtualMachineThread(int contextIndex, GlobalMemory globalMemory, RegisterCounts registerCounts)
		{
			_contextIndex = contextIndex;
			_globalMemory = globalMemory;

			var indexableTempsCounts = registerCounts.IndexableTemps;
			_indexableTempRegisters = new Number4[indexableTempsCounts.Length][];
			for (int i = 0; i < indexableTempsCounts.Length; i++)
				_indexableTempRegisters[i] = InitializeRegisters(indexableTempsCounts[i]);
			_tempRegisters = InitializeRegisters((uint) registerCounts.Temps);
		}

		private static Number4[] InitializeRegisters(uint count)
		{
			var result = new Number4[count];
			for (int i = 0; i < count; i++)
				result[i] = new Number4();
			return result;
		}

		/// <summary>
		/// Gets potentially-swizzled value for use on RHS of an operation.
		/// </summary>
		private Number4 GetRegister(Operand operand)
		{
			Func<Number4, Number4> absNegFunc = n =>
			{
				switch (operand.Modifier)
				{
					case OperandModifier.None:
						return n;
					case OperandModifier.Neg:
						return Number4.Negate(n);
					case OperandModifier.Abs:
						return Number4.Abs(n);
					case OperandModifier.AbsNeg:
						return Number4.Negate(Number4.Abs(n));
					default:
						throw new ArgumentOutOfRangeException();
				}
			};

			Func<Number4, Number4> swizzleFunc = n =>
			{
				if (operand.SelectionMode != Operand4ComponentSelectionMode.Swizzle)
					return n;
				return Number4.Swizzle(n, operand.Swizzles);
			};

			// TODO: Indices might be relative
			var indices = operand.Indices;
			switch (operand.OperandType)
			{
				case OperandType.Immediate32:
				case OperandType.Immediate64:
					return absNegFunc(operand.ImmediateValues);
				case OperandType.ConstantBuffer :
					return absNegFunc(swizzleFunc(_globalMemory.ConstantBuffers[indices[0].Value][indices[1].Value]));
				case OperandType.IndexableTemp:
					return absNegFunc(swizzleFunc(_indexableTempRegisters[indices[0].Value][indices[1].Value]));
				case OperandType.Input:
					return absNegFunc(swizzleFunc(_globalMemory.GetInput(_contextIndex, (int) indices[0].Value)));
				case OperandType.Temp:
					return absNegFunc(swizzleFunc(_tempRegisters[indices[0].Value]));
				default:
					throw new ArgumentException("Unsupported operand type: " + operand.OperandType);
			}
		}

		private void SetRegister(Operand operand, Number4 value)
		{
			// TODO: Indices might be relative
			var indices = operand.Indices;
			switch (operand.OperandType)
			{
				case OperandType.IndexableTemp :
					_indexableTempRegisters[indices[0].Value][indices[1].Value].WriteMaskedValue(value, operand.ComponentMask);
					break;
				case OperandType.Output :
					_globalMemory.SetOutput(_contextIndex, (int) indices[0].Value, value, operand.ComponentMask);
					break;
				case OperandType.Temp :
					_tempRegisters[indices[0].Value].WriteMaskedValue(value, operand.ComponentMask);
					break;
				default:
					throw new ArgumentException("Unsupported operand type: " + operand.OperandType);	
			}
		}

		public void ExecuteAdd(InstructionToken token)
		{
			Execute(token, (src0, src1) => Number.FromFloat(src0.Float + src1.Float, token.Saturate));
		}

		public bool ExecuteBreakC(InstructionToken token)
		{
			var src = GetRegister(token.Operands[0]);
			bool shouldBreak; 
			switch (token.TestBoolean)
			{
				case InstructionTestBoolean.Zero:
					shouldBreak = src.AllZero;
					break;
				case InstructionTestBoolean.NonZero:
					shouldBreak = src.AnyNonZero;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			if (shouldBreak)
			{
				PerThreadProgramCounter += token.LinkedInstructionOffset;
				return true;
			}
			PerThreadProgramCounter++;
			return false;
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

		public void ExecuteLoop(InstructionToken token)
		{
			PerThreadProgramCounter++;
		}

		public void ExecuteLt(InstructionToken token)
		{
			Execute(token, (src0, src1) => Number.FromFloat((src0.Float < src1.Float) ? 0xFFFFFFFF : 0x0000000, token.Saturate));
		}

		public void ExecuteMov(InstructionToken token)
		{
			Execute(token, src => src);
		}

		public void ExecuteMul(InstructionToken token)
		{
			Execute(token, (src0, src1) => Number.FromFloat(src0.Float * src1.Float, token.Saturate));
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
			var src = GetRegister(token.Operands[1]);

			SetRegister(token.Operands[0], new Number4
			{
				Number0 = callback(src.Number0),
				Number1 = callback(src.Number1),
				Number2 = callback(src.Number2),
				Number3 = callback(src.Number3)
			});

			PerThreadProgramCounter++;
		}

		private void Execute(InstructionToken token, Func<Number, Number, Number> callback)
		{
			var src0 = GetRegister(token.Operands[1]);
			var src1 = GetRegister(token.Operands[2]);

			SetRegister(token.Operands[0], new Number4
			{
				Number0 = callback(src0.Number0, src1.Number0),
				Number1 = callback(src0.Number1, src1.Number1),
				Number2 = callback(src0.Number2, src1.Number2),
				Number3 = callback(src0.Number3, src1.Number3)
			});

			PerThreadProgramCounter++;
		}

		private void Execute(InstructionToken token, Func<Number4, Number4, Number> callback)
		{
			var src0 = GetRegister(token.Operands[1]);
			var src1 = GetRegister(token.Operands[2]);
			var result = callback(src0, src1);

			SetRegister(token.Operands[0], new Number4
			{
				Number0 = result,
				Number1 = result,
				Number2 = result,
				Number3 = result
			});

			PerThreadProgramCounter++;
		}
	}
}