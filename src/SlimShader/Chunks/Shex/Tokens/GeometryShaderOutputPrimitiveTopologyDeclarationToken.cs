using SlimShader.Chunks.Common;
using SlimShader.Util;

namespace SlimShader.Chunks.Shex.Tokens
{
	/// <summary>
	/// Geometry Shader Output Topology Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_GS_OUTPUT_PRIMITIVE_TOPOLOGY
	/// [17:11] D3D10_SB_PRIMITIVE_TOPOLOGY
	/// [23:18] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token. == 1
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	/// </summary>
	public class GeometryShaderOutputPrimitiveTopologyDeclarationToken : DeclarationToken
	{
		public PrimitiveTopology PrimitiveTopology { get; set; }

		public static GeometryShaderOutputPrimitiveTopologyDeclarationToken Parse(BytecodeReader reader)
		{
			var token0 = reader.ReadUInt32();
			return new GeometryShaderOutputPrimitiveTopologyDeclarationToken
			{
				PrimitiveTopology = token0.DecodeValue<PrimitiveTopology>(11, 17)
			};
		}

		public override string ToString()
		{
			return string.Format("{0} {1}", TypeDescription, PrimitiveTopology.GetDescription(ChunkType.Shex));
		}
	}
}