using SlimShader.Chunks.Common;
using SlimShader.Chunks.Shex;

namespace SlimShader.VirtualMachine
{
	public class GlobalMemory
	{
		private readonly int _inputStride;
		private readonly int _outputStride;
		private readonly Number4[][] _constantBuffers;

		private readonly Number4[] _inputs;
		private readonly Number4[] _outputs;

		public Number4[][] ConstantBuffers
		{
			get { return _constantBuffers; }
		}

		public Number4[] Inputs
		{
			get { return _inputs; }
		}

		public Number4[] Outputs
		{
			get { return _outputs; }
		}

		public GlobalMemory(RegisterCounts registerCounts, int numContexts, int inputStride, int outputStride)
		{
			_inputStride = inputStride;
			_outputStride = outputStride;

			var constantBufferCounts = registerCounts.ConstantBuffers;
			_constantBuffers = new Number4[constantBufferCounts.Length][];
			for (int i = 0; i < constantBufferCounts.Length; i++)
				_constantBuffers[i] = InitializeRegisters((int) constantBufferCounts[i]);

			_inputs = InitializeRegisters(registerCounts.Inputs * numContexts);
			_outputs = InitializeRegisters(registerCounts.Outputs * numContexts);
		}

		private static Number4[] InitializeRegisters(int count)
		{
			var result = new Number4[count];
			for (int i = 0; i < count; i++)
				result[i] = new Number4();
			return result;
		}

		public Number4 GetInput(int contextIndex, int inputIndex)
		{
			return _inputs[(_inputStride * contextIndex) + inputIndex];
		}

		public void SetOutput(int contextIndex, int outputIndex, Number4 value, ComponentMask mask)
		{
			_outputs[(_outputStride * contextIndex) + outputIndex].WriteMaskedValue(value, mask);
		}
	}
}