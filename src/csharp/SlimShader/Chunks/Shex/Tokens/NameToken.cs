using SlimShader.Util;

namespace SlimShader.Chunks.Shex.Tokens
{
	/// <summary>
	/// Name Token (NameToken) (used in declaration statements)
	///
	/// [15:00] D3D10_SB_NAME enumeration
	/// [31:16] Reserved, 0
	/// </summary>
	public static class NameToken
	{
		public static SystemValueName Parse(BytecodeReader reader)
		{
			uint token = reader.ReadUInt32();
			return token.DecodeValue<SystemValueName>(0, 15);
		}
	}
}