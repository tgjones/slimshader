using SlimShader.Chunks.Common;
using SlimShader.Util;

namespace SlimShader.Chunks.Shex.Tokens
{
	/// <summary>
	/// Geometry Shader Input Primitive Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_GS_INPUT_PRIMITIVE
	/// [16:11] D3D10_SB_PRIMITIVE [not D3D10_SB_PRIMITIVE_TOPOLOGY]
	/// [23:17] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token. == 1
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	/// </summary>
	public class GeometryShaderInputPrimitiveDeclarationToken : DeclarationToken
	{
		public Primitive Primitive { get; set; }

		public static GeometryShaderInputPrimitiveDeclarationToken Parse(BytecodeReader reader)
		{
			var token0 = reader.ReadUInt32();
			return new GeometryShaderInputPrimitiveDeclarationToken
			{
				Primitive = token0.DecodeValue<Primitive>(11, 16)
			};
		}

		public override string ToString()
		{
			return string.Format("{0} {1}", TypeDescription, Primitive.GetDescription(ChunkType.Shex));
		}
	}
}