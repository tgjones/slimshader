namespace SlimShader.VirtualMachine.Registers
{
	public class NumberRegister : Register
	{
		public Number4 Value;

		public NumberRegister(RegisterKey key) 
			: base(key)
		{
			
		}
	}
}