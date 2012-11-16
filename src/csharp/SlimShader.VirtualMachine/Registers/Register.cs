using SlimShader.Chunks;
using SlimShader.Chunks.Shex;

namespace SlimShader.VirtualMachine.Registers
{
	public abstract class Register
	{
		private readonly RegisterKey _key;

		public RegisterKey Key
		{
			get { return _key; }
		}

		public string Name
		{
			get
			{
				string result = Key.RegisterType.GetDescription();
				switch (Key.RegisterType)
				{
					case OperandType.Input:
						result += Key.Index.Index1D;
						break;
					case OperandType.Temp :
					case OperandType.Output:
					case OperandType.Sampler:
					case OperandType.Resource:
					case OperandType.UnorderedAccessView:
						result += Key.Index.Index1D;
						break;
					case OperandType.ImmediateConstantBuffer :
					case OperandType.InputPatchConstant:
					case OperandType.ThreadGroupSharedMemory:
						result += string.Format("[{0}]", Key.Index.Index1D);
						break;
					case OperandType.IndexableTemp :
					case OperandType.ConstantBuffer:
						result += string.Format("{0}[{1}]", Key.Index.Index2D_0, Key.Index.Index2D_1);
						break;
					case OperandType.InputGSInstanceID:
					case OperandType.InputControlPoint :
					case OperandType.OutputControlPoint :
						result += string.Format("[{0}][{1}]", Key.Index.Index2D_0, Key.Index.Index2D_1);
						break;
				}
				return result;
			}
		}

		public Register(RegisterKey key)
		{
			_key = key;
		}
	}
}