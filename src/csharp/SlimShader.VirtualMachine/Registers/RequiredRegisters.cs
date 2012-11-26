using System.Collections.Generic;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Shex.Tokens;

namespace SlimShader.VirtualMachine.Registers
{
	public class RequiredRegisters
	{
		public int Inputs { get; private set; }
		public int Outputs { get; private set; }
		public int Resources { get; private set; }
		public int Samplers { get; private set; }
		public List<int> ConstantBuffers { get; private set; }
		public List<int> IndexableTemps { get; private set; }
		public int Temps { get; private set; }

		public static RequiredRegisters FromShader(ShaderProgramChunk shader)
		{
			var result = new RequiredRegisters();

			foreach (var declarationToken in shader.DeclarationTokens)
				switch (declarationToken.Header.OpcodeType)
				{
					case OpcodeType.DclInput:
					case OpcodeType.DclInputPs:
					case OpcodeType.DclInputPsSgv:
					case OpcodeType.DclInputPsSiv:
					case OpcodeType.DclInputSgv:
					case OpcodeType.DclInputSiv:
						result.Inputs = (int) declarationToken.Operand.Indices[0].Value + 1;
						break;
					case OpcodeType.DclOutput:
					case OpcodeType.DclOutputSgv:
					case OpcodeType.DclOutputSiv:
						result.Outputs = (int) declarationToken.Operand.Indices[0].Value + 1;
						break;
					case OpcodeType.DclResource:
						result.Resources = (int) declarationToken.Operand.Indices[0].Value + 1;
						break;
					case OpcodeType.DclSampler:
						result.Samplers = (int) declarationToken.Operand.Indices[0].Value + 1;
						break;
					case OpcodeType.DclConstantBuffer:
						while (result.ConstantBuffers.Count < (int) declarationToken.Operand.Indices[0].Value + 1)
							result.ConstantBuffers.Add(0);
						result.ConstantBuffers[(int) declarationToken.Operand.Indices[0].Value] =
							(int) declarationToken.Operand.Indices[1].Value + 1;
						break;
					case OpcodeType.DclIndexableTemp:
						while (result.IndexableTemps.Count < (int) declarationToken.Operand.Indices[0].Value + 1)
							result.IndexableTemps.Add(0);
						result.IndexableTemps[(int) declarationToken.Operand.Indices[0].Value] =
							(int) declarationToken.Operand.Indices[1].Value + 1;
						break;
					case OpcodeType.DclTemps:
						result.Temps = (int) ((TempRegisterDeclarationToken) declarationToken).TempCount;
						break;
				}

			return result;
		}

		private RequiredRegisters()
		{
			ConstantBuffers = new List<int>();
			IndexableTemps = new List<int>();
		}
	}
}