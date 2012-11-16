using System.Collections.Generic;

namespace SlimShader.VirtualMachine.Registers
{
	public class RegisterSet
	{
		private readonly Dictionary<RegisterKey, Register> _registers;

		public Register this[RegisterKey key]
		{
			get { return _registers[key]; }
		}

		public RegisterSet(RequiredRegisters requiredRegisters)
		{
			_registers = new Dictionary<RegisterKey, Register>();

			foreach (var registerKey in requiredRegisters.Registers)
				_registers.Add(registerKey, new NumberRegister(registerKey));
		}
	}
}