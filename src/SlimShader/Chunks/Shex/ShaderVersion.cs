using SlimShader.Util;

namespace SlimShader.Chunks.Shex
{
	/// <summary>
	/// Version Token (VerTok)
	/// [07:00] minor version number (0-255)
	/// [15:08] major version number (0-255)
	/// [31:16] D3D10_SB_TOKENIZED_PROGRAM_TYPE
	/// </summary>
	public class ShaderVersion
	{
		public byte MajorVersion { get; set; }
		public byte MinorVersion { get; set; }
		public ProgramType ProgramType { get; set; }

		public static ShaderVersion Parse(BytecodeReader reader)
		{
			uint versionToken = reader.ReadUInt32();
			return new ShaderVersion
			{
				MinorVersion = versionToken.DecodeValue<byte>(0, 3),
				MajorVersion = versionToken.DecodeValue<byte>(4, 7),
				ProgramType = versionToken.DecodeValue<ProgramType>(16, 31)
			};
		}
	}
}