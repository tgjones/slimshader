using System;
using System.Collections.Generic;

namespace SlimShader.VirtualMachine.Registers
{
	public class RegisterSet
	{
		private readonly Dictionary<RegisterKey, Register> _registers;

		public Register this[RegisterKey key]
		{
			get
			{
				Register value;
				if (!_registers.TryGetValue(key, out value))
					throw new ArgumentOutOfRangeException("key", "Key not found: " + key);
				return value;
			}
		}

		public RegisterSet(RequiredRegisters requiredRegisters)
		{
			_registers = new Dictionary<RegisterKey, Register>();

			foreach (var registerKey in requiredRegisters.Registers)
				_registers.Add(registerKey, new NumberRegister(registerKey));
		}
	}
}