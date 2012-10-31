using SlimShader.Chunks.Common;
using SlimShader.Util;

namespace SlimShader.Chunks.Shex.Tokens
{
	/// <summary>
	/// Hull Shader Declaration Phase: Tessellator Domain
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D11_SB_OPCODE_DCL_TESS_DOMAIN
	/// [12:11] Domain
	/// [23:13] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token. == 1
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	/// </summary>
	public class TessellatorDomainDeclarationToken : DeclarationToken
	{
		public TessellatorDomain Domain { get; private set; }

		public static TessellatorDomainDeclarationToken Parse(BytecodeReader reader)
		{
			var token0 = reader.ReadUInt32();
			return new TessellatorDomainDeclarationToken
			{
				Domain = token0.DecodeValue<TessellatorDomain>(11, 12)
			};
		}

		public override string ToString()
		{
			return string.Format("{0} {1}", TypeDescription, Domain.GetDescription(ChunkType.Shex));
		}
	}
}