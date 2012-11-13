using SlimShader.Util;

namespace SlimShader.Chunks.Shex.Tokens
{
	/// <summary>
	/// Hull Shader Declaration Phase: Hull Shader Max Tessfactor
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D11_SB_OPCODE_DCL_HS_MAX_TESSFACTOR
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by a float32 representing the
	/// maximum TessFactor.
	/// </summary>
	public class HullShaderMaxTessFactorDeclarationToken : DeclarationToken
	{
		public float MaxTessFactor { get; set; }

		public static HullShaderMaxTessFactorDeclarationToken Parse(BytecodeReader reader)
		{
			var token0 = reader.ReadUInt32();
			return new HullShaderMaxTessFactorDeclarationToken
			{
				MaxTessFactor = reader.ReadSingle()
			};
		}

		public override string ToString()
		{
			return string.Format("{0} l({1})", TypeDescription, MaxTessFactor.ToString("F6"));
		}
	}
}