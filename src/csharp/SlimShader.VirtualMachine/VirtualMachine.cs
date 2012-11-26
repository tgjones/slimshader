using System.Linq;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Shex.Tokens;
using SlimShader.VirtualMachine.Execution;
using SlimShader.VirtualMachine.Registers;

namespace SlimShader.VirtualMachine
{
	public class VirtualMachine
	{
		private readonly BytecodeContainer _bytecode;

		private readonly ExecutionContext[] _executionContexts;
		private readonly IShaderExecutor _shaderExecutor;

		private readonly RequiredRegisters _requiredRegisters;

		public VirtualMachine(BytecodeContainer bytecode, int numContexts)
		{
			_bytecode = bytecode;

			var instructions = bytecode.Shader.Tokens.OfType<InstructionToken>().ToArray();

			_requiredRegisters = RequiredRegisters.FromShader(bytecode.Shader);

			_executionContexts = new ExecutionContext[numContexts];
			for (int i = 0; i < _executionContexts.Length; i++)
				_executionContexts[i] = new ExecutionContext(_requiredRegisters);
			_shaderExecutor = new Interpreter(_executionContexts, instructions);
		}

		public void Execute()
		{
			_shaderExecutor.Execute();
		}

		public Number4 GetRegister(int contextIndex, OperandType registerType, RegisterIndex registerIndex)
		{
			Number4[] register;
			int index;
			_executionContexts[contextIndex].GetRegister(registerType, registerIndex, out register, out index);
			return register[index];
		}

		public void SetRegister(int contextIndex, OperandType registerType, RegisterIndex registerIndex, Number4 value)
		{
			Number4[] register;
			int index;
			_executionContexts[contextIndex].GetRegister(registerType, registerIndex, out register, out index);
			register[index] = value;
		}
	}
}