using System;
using SlimShader.Chunks.Shex;
using SlimShader.VirtualMachine.Registers;
using SlimShader.VirtualMachine.Resources;

namespace SlimShader.VirtualMachine
{
	public class ExecutionContext
	{
		public Number4[][] ConstantBuffers { get; private set; }

		public Number4[][] Inputs { get; private set; }
		public Number4[] Outputs { get; private set; }

		public Number4[] Temps { get; private set; }
		public Number4[][] IndexableTemps { get; private set; }

		public ITexture[] Textures { get; private set; }
		public ISampler[] Samplers { get; private set; }

		public ExecutionContext(RequiredRegisters requiredRegisters)
		{
			ConstantBuffers = new Number4[requiredRegisters.ConstantBuffers.Count][];
			for (int i = 0; i < requiredRegisters.ConstantBuffers.Count; i++)
				ConstantBuffers[i] = new Number4[requiredRegisters.ConstantBuffers[i]];

			Inputs = new Number4[requiredRegisters.NumPrimitives][];
			for (int i = 0; i < requiredRegisters.NumPrimitives; i++)
				Inputs[i] = new Number4[requiredRegisters.Inputs];

			Outputs = new Number4[requiredRegisters.Outputs];

			Temps = new Number4[requiredRegisters.Temps];

			IndexableTemps = new Number4[requiredRegisters.IndexableTemps.Count][];
			for (int i = 0; i < requiredRegisters.IndexableTemps.Count; i++)
				IndexableTemps[i] = new Number4[requiredRegisters.IndexableTemps[i]];

			Textures = new ITexture[requiredRegisters.Resources];
			Samplers = new ISampler[requiredRegisters.Samplers];
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
					// Only GS requires 2-dimensional inputs, but for simplicity we always use a 2-dimensional input array.
					register = Inputs[registerIndex.Index2D_0];
					index = registerIndex.Index2D_1;
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
	}
}