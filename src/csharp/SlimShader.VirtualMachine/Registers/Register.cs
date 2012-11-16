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
			get { return Key.ToString(); }
		}

		protected Register(RegisterKey key)
		{
			_key = key;
		}
	}
}