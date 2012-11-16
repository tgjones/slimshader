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
	}
}