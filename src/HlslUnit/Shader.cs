using System;
using SlimShader;
using SlimShader.Chunks.Shex;
using SlimShader.Compiler;
using SlimShader.VirtualMachine;
using SlimShader.VirtualMachine.Registers;
using SlimShader.VirtualMachine.Util;

namespace HlslUnit
{
	public class Shader
	{
		private readonly VirtualMachine _virtualMachine;

		public Shader(string fileName, string entryPoint, string profile)
		{
			var bytecodeContainer = ShaderCompiler.CompileFromFile(fileName, entryPoint, profile);
			_virtualMachine = new VirtualMachine(bytecodeContainer, 1);
		}

		public void SetConstantBuffer<T>(string name, T value)
			where T : struct
		{
			var constantBufferIndex = _virtualMachine.Bytecode.ResourceDefinition.ConstantBuffers.FindIndex(x => x.Name == name);
			if (constantBufferIndex == -1)
				throw new ArgumentException("Could not find constant buffer named '" + name + "'", "name");

			var bytes = StructUtility.ToBytes(value);
			for (var i = 0; i < bytes.Length; i += 16)
			{
				_virtualMachine.SetRegister(0, OperandType.ConstantBuffer,
					new RegisterIndex((ushort) constantBufferIndex, (ushort) (i / 16)),
					Number4.FromByteArray(bytes, i));
			}
		}

		public TOutput Execute<TOutput>(params object[] parameters)
			where TOutput : struct
		{
			for (var i = 0; i < parameters.Length; i++)
			{
				var bytes = StructUtility.ToBytes(parameters[i]);
				_virtualMachine.SetRegister(0, OperandType.Input,
					new RegisterIndex(0, (ushort) i),
					Number4.FromByteArray(bytes, i));
			}

			_virtualMachine.Execute();

			var outputBytes = new byte[_virtualMachine.NumOutputs * 16];
			for (var i = 0; i < _virtualMachine.NumOutputs; i++)
			{
				var thisOutputBytes = _virtualMachine.GetOutputRegisterValue(0, i).RawBytes;
				Array.Copy(thisOutputBytes, 0, outputBytes, i * 16, thisOutputBytes.Length);
			}

			return StructUtility.FromBytes<TOutput>(outputBytes);
		}
	}
}