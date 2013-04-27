using SlimShader.Util;

namespace SlimShader.Chunks.Shex.Tokens
{
	/// <summary>
	/// Temp Register Declaration r0...r(n-1) 
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_TEMPS
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 1 operand:
	/// (1) DWORD (unsigned int) indicating how many temps are being declared.  
	///     i.e. 5 means r0...r4 are declared.
	/// </summary>
	public class TempRegisterDeclarationToken : DeclarationToken
	{
		/// <summary>
		/// Indicates how many temps are being declared. i.e. 5 means r0...r4 are declared.
		/// </summary>
		public uint TempCount { get; internal set; }

		public static TempRegisterDeclarationToken Parse(BytecodeReader reader)
		{
			var token0 = reader.ReadUInt32();
			return new TempRegisterDeclarationToken
			{
				TempCount = reader.ReadUInt32()
			};
		}

		public override string ToString()
		{
			return string.Format("{0} {1}", TypeDescription, TempCount);
		}
	}
}