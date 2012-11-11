using System;
using System.Collections.Generic;
using SlimShader.Chunks.Common;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Shex.Tokens;

namespace SlimShader.VirtualMachine
{
	public class VirtualMachineThread
	{
		private readonly Number4[][] _constantBufferRegisters;
		private readonly Number4[][] _indexableTempRegisters;
		private readonly Number4[] _inputRegisters;
		private readonly Number4[] _outputRegisters;
		private readonly Number4[] _resourceRegisters;
		private readonly Number4[] _samplerRegisters;
		private readonly Number4[] _tempRegisters;

		public Number4[][] ConstantBuffers
		{
			get { return _constantBufferRegisters; }
		}

		public Number4[] Input
		{
			get { return _inputRegisters; }
		}

		public Number4[] Output
		{
			get { return _outputRegisters; }
		}

		public int PerThreadProgramCounter { get; private set; }

		public VirtualMachineThread(int[] constantBufferCounts, int[] indexableTempCounts,
			int inputCount, int outputCount, int resourceCount, int samplerCount, int tempCount)
		{
			_constantBufferRegisters = new Number4[constantBufferCounts.Length][];
			for (int i = 0; i < constantBufferCounts.Length; i++)
				_constantBufferRegisters[i] = InitializeRegisters(constantBufferCounts[i]);

			_indexableTempRegisters = new Number4[indexableTempCounts.Length][];
			for (int i = 0; i < indexableTempCounts.Length; i++)
				_indexableTempRegisters[i] = InitializeRegisters(indexableTempCounts[i]);

			_inputRegisters = InitializeRegisters(inputCount);
			_outputRegisters = InitializeRegisters(outputCount);
			_resourceRegisters = InitializeRegisters(resourceCount);
			_samplerRegisters = InitializeRegisters(samplerCount);
			_tempRegisters = InitializeRegisters(tempCount);
		}

		private static Number4[] InitializeRegisters(int count)
		{
			var result = new Number4[count];
			for (int i = 0; i < count; i++)
				result[i] = new Number4();
			return result;
		}

		/// <summary>
		/// Gets value for use on LHS of an operation.
		/// </summary>
		public Number4 GetRegisterLhs(Operand operand)
		{
			// TODO: Indices might be relative
			var indices = operand.Indices;
			switch (operand.OperandType)
			{
				case OperandType.ConstantBuffer :
					return _constantBufferRegisters[indices[0].Value][indices[1].Value];
				case OperandType.IndexableTemp:
					return _indexableTempRegisters[indices[0].Value][indices[1].Value];
				case OperandType.Input:
					return _inputRegisters[indices[0].Value];
				case OperandType.Output :
					return _outputRegisters[indices[0].Value];
				case OperandType.Temp:
					return _tempRegisters[indices[0].Value];
				default:
					throw new ArgumentException("Unsupported operand type");
			}
		}

		/// <summary>
		/// Gets potentially-swizzled value for use on RHS of an operation.
		/// </summary>
		public Number4 GetRegisterRhs(Operand operand)
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

			Number4 result;
			switch (operand.OperandType)
			{
				case OperandType.Immediate32:
				case OperandType.Immediate64:
					return absNegFunc(operand.ImmediateValues);
				case OperandType.ConstantBuffer :
				case OperandType.IndexableTemp:
				case OperandType.Input:
				case OperandType.Output :
				case OperandType.Temp :
					return absNegFunc(swizzleFunc(GetRegisterLhs(operand)));
				default:
					throw new ArgumentException("Unsupported operand type: " + operand.OperandType);
			}
		}

		public void ExecuteAdd(InstructionToken token)
		{
			Execute(token, (src0, src1) => Number.FromFloat(src0.Float + src1.Float));
		}

		public bool ExecuteBreakC(InstructionToken token)
		{
			var src = GetRegisterRhs(token.Operands[0]);
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
			Execute(token, (src0, src1) => Number.FromFloat(src0.Float / src1.Float));
		}

		public void ExecuteDp2(InstructionToken token)
		{
			Execute(token, (src0, src1) => Number.FromFloat(
				src0.Number0.Float * src1.Number0.Float
				+ src0.Number1.Float * src1.Number1.Float));
		}

		public void ExecuteDp3(InstructionToken token)
		{
			Execute(token, (src0, src1) => Number.FromFloat(
				src0.Number0.Float * src1.Number0.Float
				+ src0.Number1.Float * src1.Number1.Float
				+ src0.Number2.Float * src1.Number2.Float));
		}

		public void ExecuteDp4(InstructionToken token)
		{
			Execute(token, (src0, src1) => Number.FromFloat(
				src0.Number0.Float * src1.Number0.Float
				+ src0.Number1.Float * src1.Number1.Float
				+ src0.Number2.Float * src1.Number2.Float
				+ src0.Number3.Float * src1.Number3.Float));
		}

		public void ExecuteILt(InstructionToken token)
		{
			Execute(token, (src0, src1) => Number.FromUInt((src0.Int < src1.Int) ? 0xFFFFFFFF : 0x0000000));
		}

		public void ExecuteItoF(InstructionToken token)
		{
			Execute(token, src => Number.FromFloat(Convert.ToSingle(src.Int)));
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
			Execute(token, (src0, src1) => Number.FromFloat((src0.Float < src1.Float) ? 0xFFFFFFFF : 0x0000000));
		}

		public void ExecuteMov(InstructionToken token)
		{
			Execute(token, src => src);
		}

		public void ExecuteMul(InstructionToken token)
		{
			Execute(token, (src0, src1) => Number.FromFloat(src0.Float * src1.Float));
		}

		public void ExecuteSqrt(InstructionToken token)
		{
			Execute(token, src => Number.FromFloat((float) Math.Sqrt(src.Float)));
		}

		public void ExecuteUtoF(InstructionToken token)
		{
			Execute(token, src => Number.FromFloat(Convert.ToSingle(src.UInt)));
		}

		public void ExecuteXor(InstructionToken token)
		{
			Execute(token, (src0, src1) => Number.FromUInt(src0.UInt | src1.UInt));
		}

		private void Execute(InstructionToken token, Func<Number, Number> callback)
		{
			var destOperand = token.Operands[0];
			var dest = GetRegisterLhs(destOperand);
			var src0 = GetRegisterRhs(token.Operands[1]);
			for (var i = 0; i < 4; i++)
				if (destOperand.ComponentMask.HasFlag((ComponentMask) i))
					dest.SetNumber(i, callback(src0.GetNumber(i)));

			if (token.Saturate)
				dest.Saturate();

			PerThreadProgramCounter++;
		}

		private void Execute(InstructionToken token, Func<Number, Number, Number> callback)
		{
			var destOperand = token.Operands[0];
			var dest = GetRegisterLhs(destOperand);
			var src0 = GetRegisterRhs(token.Operands[1]);
			var src1 = GetRegisterRhs(token.Operands[2]);
			for (var i = 0; i < 4; i++)
				if (destOperand.ComponentMask.HasFlag((ComponentMask) i))
					dest.SetNumber(i, callback(src0.GetNumber(i), src1.GetNumber(i)));

			if (token.Saturate)
				dest.Saturate();

			PerThreadProgramCounter++;
		}

		private void Execute(InstructionToken token, Func<Number4, Number4, Number> callback)
		{
			var destOperand = token.Operands[0];
			var dest = GetRegisterLhs(destOperand);
			var src0 = GetRegisterRhs(token.Operands[1]);
			var src1 = GetRegisterRhs(token.Operands[2]);
			var result = callback(src0, src1);
			for (var i = 0; i < 4; i++)
				if (destOperand.ComponentMask.HasFlag((ComponentMask) i))
					dest.SetNumber(i, result);

			if (token.Saturate)
				dest.Saturate();

			PerThreadProgramCounter++;
		}
	}
}