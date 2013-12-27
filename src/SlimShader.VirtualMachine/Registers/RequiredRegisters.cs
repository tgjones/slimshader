using System;
using System.Collections.Generic;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Shex.Tokens;

namespace SlimShader.VirtualMachine.Registers
{
	public class RequiredRegisters
	{
		public int NumPrimitives { get; private set; }
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
			{
				OperandIndex[] indices = (declarationToken.Operand != null) 
					? declarationToken.Operand.Indices 
					: new OperandIndex[0];
				var indexDimension = (declarationToken.Operand != null)
					? declarationToken.Operand.IndexDimension
					: OperandIndexDimension._0D;
				switch (declarationToken.Header.OpcodeType)
				{
					case OpcodeType.DclInput:
					case OpcodeType.DclInputPs:
					case OpcodeType.DclInputPsSgv:
					case OpcodeType.DclInputPsSiv:
					case OpcodeType.DclInputSgv:
					case OpcodeType.DclInputSiv:
						switch (indexDimension)
						{
							case OperandIndexDimension._2D:
								result.NumPrimitives = (int) indices[0].Value + 1;
								result.Inputs = Math.Max(result.Inputs, (int) indices[1].Value + 1);
								break;
							case OperandIndexDimension._1D:
								result.NumPrimitives = 1;
								result.Inputs = (int) indices[0].Value + 1;
								break;
							default:
								throw new ArgumentOutOfRangeException();
						}
						break;
					case OpcodeType.DclOutput:
					case OpcodeType.DclOutputSgv:
					case OpcodeType.DclOutputSiv:
						result.Outputs = (int)indices[0].Value + 1;
						break;
					case OpcodeType.DclResource:
						result.Resources = (int)indices[0].Value + 1;
						break;
					case OpcodeType.DclSampler:
						result.Samplers = (int)indices[0].Value + 1;
						break;
					case OpcodeType.DclConstantBuffer:
						while (result.ConstantBuffers.Count < (int) indices[0].Value + 1)
							result.ConstantBuffers.Add(0);
						result.ConstantBuffers[(int)indices[0].Value] = (int) indices[1].Value;
						break;
					case OpcodeType.DclIndexableTemp:
						while (result.IndexableTemps.Count < (int)indices[0].Value + 1)
							result.IndexableTemps.Add(0);
						result.IndexableTemps[(int)indices[0].Value] = (int)indices[1].Value + 1;
						break;
					case OpcodeType.DclTemps:
						result.Temps = (int) ((TempRegisterDeclarationToken) declarationToken).TempCount;
						break;
				}
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