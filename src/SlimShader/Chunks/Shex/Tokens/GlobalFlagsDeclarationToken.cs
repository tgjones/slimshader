using SlimShader.Util;

namespace SlimShader.Chunks.Shex.Tokens
{
	/// <summary>
	/// Global Flags Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_GLOBAL_FLAGS
	/// [11:11] Refactoring allowed if bit set.
	/// [12:12] Enable double precision float ops.
	/// [13:13] Force early depth-stencil test.
	/// [14:14] Enable RAW and structured buffers in non-CS 4.x shaders.
	/// [15:15] Skip optimizations of shader IL when translating to native code
	/// [16:16] Enable minimum-precision data types
	/// [17:17] Enable 11.1 double-precision floating-point instruction extensions
	/// [18:18] Enable 11.1 non-double instruction extensions
	/// [23:19] Reserved for future flags.
	/// [30:24] Instruction length in DWORDs including the opcode token. == 1
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	// OpcodeToken0 is followed by no operands.
	/// </summary>
	public class GlobalFlagsDeclarationToken : DeclarationToken
	{
		public GlobalFlags Flags { get; private set; }

		public static GlobalFlagsDeclarationToken Parse(BytecodeReader reader)
		{
			var token0 = reader.ReadUInt32();
			return new GlobalFlagsDeclarationToken
			{
				Flags = token0.DecodeValue<GlobalFlags>(11, 18)
			};
		}

		public override string ToString()
		{
			return string.Format("{0} {1}", TypeDescription, Flags.GetDescription());
		}
	}
}