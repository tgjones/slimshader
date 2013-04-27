using SlimShader.Util;

namespace SlimShader.Chunks.Shex.Tokens
{
	/// <summary>
	/// Hull Shader Declaration Phase: HS/DS Input Control Point Count
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D11_SB_OPCODE_DCL_INPUT_CONTROL_POINT_COUNT
	/// [16:11] Control point count 
	/// [23:17] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token. == 1
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	/// 
	/// -------
	/// 
	/// Hull Shader Declaration Phase: HS Output Control Point Count
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D11_SB_OPCODE_DCL_OUTPUT_CONTROL_POINT_COUNT
	/// [16:11] Control point count 
	/// [23:17] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token. == 1
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	/// </summary>
	public class ControlPointCountDeclarationToken : DeclarationToken
	{
		public uint ControlPointCount { get; private set; }

		public static ControlPointCountDeclarationToken Parse(BytecodeReader reader)
		{
			var token0 = reader.ReadUInt32();
			return new ControlPointCountDeclarationToken
			{
				ControlPointCount = token0.DecodeValue(11, 16)
			};
		}

		public override string ToString()
		{
			return string.Format("{0} {1}", TypeDescription, ControlPointCount);
		}
	}
}