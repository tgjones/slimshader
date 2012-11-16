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
					case OpcodeType.DclConstantBuffer :
					case OpcodeType.DclIndexableTemp:
					case OpcodeType.DclInput:
					case OpcodeType.DclInputPs:
					case OpcodeType.DclInputPsSgv:
					case OpcodeType.DclInputPsSiv:
					case OpcodeType.DclInputSgv:
					case OpcodeType.DclInputSiv:
					case OpcodeType.DclOutput:
					case OpcodeType.DclResource:
					case OpcodeType.DclSampler:
					case OpcodeType.DclTemps:
						result.Registers.Add(new RegisterKey(declarationToken.Operand.OperandType,
							GetRegisterIndex(declarationToken)));
						break;
				}
			}
			

			return result;
		}

		private static RegisterIndex GetRegisterIndex(DeclarationToken token)
		{
			var result = new RegisterIndex();

			switch (token.Operand.IndexDimension)
			{
				case OperandIndexDimension._1D :
					result.Index1D = (ushort) token.Operand.Indices[0].Value;
					break;
				case OperandIndexDimension._2D:
					result.Index2D_0 = (ushort) token.Operand.Indices[0].Value;
					result.Index2D_1 = (ushort) token.Operand.Indices[1].Value;
					break;
			}
			
			return result;
		}

		public RequiredRegisters()
		{
			Registers = new List<RegisterKey>();
		}
	}
}