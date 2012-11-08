using System;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Shex.Tokens;
using SlimShader.VirtualMachine.Resources;

namespace SlimShader.VirtualMachine
{
	public class VirtualMachine
	{
		private readonly DxbcContainer _bytecode;
		private readonly UnorderedAccessView[] _unorderedAccessViews;

		private readonly Register<Number>[] _outputRegisters;

		public Register<Number>[] OutputRegisters
		{
			get { return _outputRegisters; }
		}

		public VirtualMachine(DxbcContainer bytecode)
		{
			_bytecode = bytecode;
			_unorderedAccessViews = new UnorderedAccessView[64];
			_outputRegisters = new Register<Number>[8];
			for (int i = 0; i < 8; i++)
				_outputRegisters[i] = new Register<Number>(4);
		}

		public void SetUnorderedAccessViews(int startSlot, UnorderedAccessView[] unorderedAccessViews)
		{
			for (int i = 0; i < unorderedAccessViews.Length; i++)
				_unorderedAccessViews[startSlot + i] = unorderedAccessViews[i];
		}

		public void Execute()
		{
			foreach (var token in _bytecode.Shader.Tokens)
			{
				switch (token.Header.OpcodeType)
				{
					case OpcodeType.DclGlobalFlags :
						break;
					case OpcodeType.DclOutput :
						break;
					case OpcodeType.Mov :
						ExecuteMov((InstructionToken) token);
						break;
					case OpcodeType.Ret :
						return;
					default :
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		private void ExecuteMov(InstructionToken token)
		{
			var destination = token.Operands[0];
			var source = token.Operands[1];

			switch (destination.OperandType)
			{
				case OperandType.Output:
					_outputRegisters[destination.Indices[0].Value].Values[0] = source.ImmediateValues.GetNumber(0);
					_outputRegisters[destination.Indices[0].Value].Values[1] = source.ImmediateValues.GetNumber(1);
					_outputRegisters[destination.Indices[0].Value].Values[2] = source.ImmediateValues.GetNumber(2);
					_outputRegisters[destination.Indices[0].Value].Values[3] = source.ImmediateValues.GetNumber(3);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}

	public class Register<T>
	{
		private readonly T[] _values;

		public T[] Values
		{
			get { return _values; }
		}

		public Register(int dimension)
		{
			_values = new T[dimension];
		}
	}
}