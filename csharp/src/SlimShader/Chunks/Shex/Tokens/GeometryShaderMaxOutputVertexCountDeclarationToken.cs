using SlimShader.Util;

namespace SlimShader.Chunks.Shex.Tokens
{
	/// <summary>
	/// Geometry Shader Maximum Output Vertex Count Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_MAX_OUTPUT_VERTEX_COUNT
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by a DWORD representing the
	/// maximum number of primitives that could be output
	/// by the Geometry Shader.
	/// </summary>
	public class GeometryShaderMaxOutputVertexCountDeclarationToken : DeclarationToken
	{
		public uint MaxPrimitives { get; private set; }

		public static GeometryShaderMaxOutputVertexCountDeclarationToken Parse(BytecodeReader reader)
		{
			var token0 = reader.ReadUInt32();
			return new GeometryShaderMaxOutputVertexCountDeclarationToken
			{
				MaxPrimitives = reader.ReadUInt32()
			};
		}

		public override string ToString()
		{
			return string.Format("{0} {1}", TypeDescription, MaxPrimitives);
		}
	}
}