using SlimShader.Chunks;
using SlimShader.Chunks.Shex;

namespace SlimShader.VirtualMachine.Registers
{
	public struct RegisterKey
	{
		public OperandType RegisterType;
		public RegisterIndex Index;

		public RegisterKey(OperandType registerType, RegisterIndex index)
		{
			RegisterType = registerType;
			Index = index;
		}

		public override string ToString()
		{
			string result = RegisterType.GetDescription();
			switch (RegisterType)
			{
				case OperandType.Input:
					result += Index.Index1D;
					break;
				case OperandType.Temp:
				case OperandType.Output:
				case OperandType.Sampler:
				case OperandType.Resource:
				case OperandType.UnorderedAccessView:
					result += Index.Index1D;
					break;
				case OperandType.ImmediateConstantBuffer:
				case OperandType.InputPatchConstant:
				case OperandType.ThreadGroupSharedMemory:
					result += string.Format("[{0}]", Index.Index1D);
					break;
				case OperandType.IndexableTemp:
				case OperandType.ConstantBuffer:
					result += string.Format("{0}[{1}]", Index.Index2D_0, Index.Index2D_1);
					break;
				case OperandType.InputGSInstanceID:
				case OperandType.InputControlPoint:
				case OperandType.OutputControlPoint:
					result += string.Format("[{0}][{1}]", Index.Index2D_0, Index.Index2D_1);
					break;
			}
			return result;
		}
	}
}