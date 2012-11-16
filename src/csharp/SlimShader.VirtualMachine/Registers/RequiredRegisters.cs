using System.Collections.Generic;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Shex.Tokens;

namespace SlimShader.VirtualMachine.Registers
{
	public class RequiredRegisters
	{
		public List<RegisterKey> Registers { get; private set; }

		public static RequiredRegisters FromShader(ShaderProgramChunk shader)
		{
			var result = new RequiredRegisters();
			foreach (var declarationToken in shader.DeclarationTokens)
			{
				switch (declarationToken.Header.OpcodeType)
				{
					case OpcodeType.DclInput:
					case OpcodeType.DclInputPs:
					case OpcodeType.DclInputPsSgv:
					case OpcodeType.DclInputPsSiv:
					case OpcodeType.DclInputSgv:
					case OpcodeType.DclInputSiv:
					case OpcodeType.DclOutput:
					case OpcodeType.DclOutputSgv :
					case OpcodeType.DclOutputSiv :
					case OpcodeType.DclResource:
					case OpcodeType.DclSampler:
						result.Registers.Add(new RegisterKey(declarationToken.Operand.OperandType,
							new RegisterIndex((ushort) declarationToken.Operand.Indices[0].Value)));
						break;
					case OpcodeType.DclConstantBuffer:
					case OpcodeType.DclIndexableTemp:
					{
						var count = (ushort) declarationToken.Operand.Indices[1].Value;
						for (ushort i = 0; i < count; i++)
							result.Registers.Add(new RegisterKey(declarationToken.Operand.OperandType,
								new RegisterIndex((ushort) declarationToken.Operand.Indices[0].Value, i)));
						break;
					}
					case OpcodeType.DclTemps:
					{
						var tempDeclarationToken = (TempRegisterDeclarationToken) declarationToken;
						for (ushort i = 0; i < tempDeclarationToken.TempCount; i++)
							result.Registers.Add(new RegisterKey(OperandType.Temp, new RegisterIndex(i)));
						break;
					}
				}
			}
			

			return result;
		}

		public RequiredRegisters()
		{
			Registers = new List<RegisterKey>();
		}
	}
}