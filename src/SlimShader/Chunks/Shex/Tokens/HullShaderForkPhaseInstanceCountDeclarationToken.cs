using SlimShader.Util;

namespace SlimShader.Chunks.Shex.Tokens
{
	/// <summary>
	/// Hull Shader Declaration Phase: Hull Shader Fork Phase Instance Count
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D11_SB_OPCODE_DCL_HS_FORK_PHASE_INSTANCE_COUNT
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by a UINT32 representing the
	/// number of instances of the current fork phase program to execute.
	/// </summary>
	public class HullShaderForkPhaseInstanceCountDeclarationToken : DeclarationToken
	{
		public uint InstanceCount { get; private set; }

		public static HullShaderForkPhaseInstanceCountDeclarationToken Parse(BytecodeReader reader)
		{
			var token0 = reader.ReadUInt32();
			return new HullShaderForkPhaseInstanceCountDeclarationToken
			{
				InstanceCount = reader.ReadUInt32()
			};
		}

		public override string ToString()
		{
			return string.Format("{0} {1}", TypeDescription, InstanceCount);
		}
	}
}