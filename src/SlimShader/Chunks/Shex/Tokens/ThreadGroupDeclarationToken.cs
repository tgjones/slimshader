using SlimShader.Util;

namespace SlimShader.Chunks.Shex.Tokens
{
	/// <summary>
	/// Thread Group Declaration (Compute Shader)
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D11_SB_OPCODE_DCL_THREAD_GROUP
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  If it is extended, then
	///         it contains the actual instruction length in DWORDs, since
	///         it may not fit into 7 bits if enough types are used.
	///
	/// OpcodeToken0 is followed by 3 DWORDs, the Thread Group dimensions as UINT32:
	/// x, y, z
	/// </summary>
	public class ThreadGroupDeclarationToken : DeclarationToken
	{
		public uint[] Dimensions { get; private set; }

		public ThreadGroupDeclarationToken()
		{
			Dimensions = new uint[3];
		}

		public static ThreadGroupDeclarationToken Parse(BytecodeReader reader)
		{
			var token0 = reader.ReadUInt32();

			var result = new ThreadGroupDeclarationToken();
			result.Dimensions[0] = reader.ReadUInt32();
			result.Dimensions[1] = reader.ReadUInt32();
			result.Dimensions[2] = reader.ReadUInt32();

			return result;
		}

		public override string ToString()
		{
			return string.Format("{0} {1}, {2}, {3}", TypeDescription, Dimensions[0], Dimensions[1], Dimensions[2]);
		}
	}
}