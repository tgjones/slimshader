using SlimShader.Chunks.Common;
using SlimShader.Util;

namespace SlimShader.Chunks.Shex.Tokens
{
	/// <summary>
	/// Hull Shader Declaration Phase: Tessellator Output Primitive
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D11_SB_OPCODE_DCL_TESS_OUTPUT_PRIMITIVE
	/// [13:11] Output Primitive
	/// [23:14] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token. == 1
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	/// </summary>
	public class TessellatorOutputPrimitiveDeclarationToken : DeclarationToken
	{
		public TessellatorOutputPrimitive OutputPrimitive { get; set; }

		public static TessellatorOutputPrimitiveDeclarationToken Parse(BytecodeReader reader)
		{
			var token0 = reader.ReadUInt32();
			return new TessellatorOutputPrimitiveDeclarationToken
			{
				OutputPrimitive = token0.DecodeValue<TessellatorOutputPrimitive>(11, 13)
			};
		}

		public override string ToString()
		{
			return string.Format("{0} {1}", TypeDescription, OutputPrimitive.GetDescription(ChunkType.Shex));
		}
	}
}