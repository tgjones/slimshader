using System;
using SlimShader.IO;
using SlimShader.Shader;
using SlimShader.Util;

namespace SlimShader.ResourceDefinition
{
	public static class ShaderTarget
	{
		public static ShaderVersion Parse(BytecodeReader reader)
		{
			uint target = reader.ReadUInt32();

			var programTypeValue = target.DecodeValue<ushort>(16, 31);
			ProgramType programType;
			switch (programTypeValue)
			{
				case 0xFFFF:
					programType = ProgramType.PixelShader;
					break;
				case 0xFFFE:
					programType = ProgramType.VertexShader;
					break;
				case 0x4853:
					programType = ProgramType.HullShader;
					break;
				case 0x4753 :
					programType = ProgramType.GeometryShader;
					break;
				case 0x4453 :
					programType = ProgramType.DomainShader;
					break;
				case 0x4353 :
					programType = ProgramType.ComputeShader;
					break;
				default:
					throw new ArgumentOutOfRangeException(string.Format("Unknown program type: 0x{0:X}", programTypeValue));
			}

			return new ShaderVersion
			{
				MajorVersion = target.DecodeValue<byte>(8, 15),
				MinorVersion = target.DecodeValue<byte>(0, 7),
				ProgramType = programType
			};
		}
	}
}