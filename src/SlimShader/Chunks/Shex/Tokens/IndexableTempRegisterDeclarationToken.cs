using SlimShader.Util;

namespace SlimShader.Chunks.Shex.Tokens
{
	/// <summary>
	/// Indexable Temp Register (x#[size]) Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_INDEXABLE_TEMP
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 3 DWORDs:
	/// (1) Register index (defines which x# register is declared)
	/// (2) Number of registers in this register bank
	/// (3) Number of components in the array (1-4). 1 means .x, 2 means .xy etc.
	/// </summary>
	public class IndexableTempRegisterDeclarationToken : DeclarationToken
	{
		/// <summary>
		/// Register index (defines which x# register is declared)
		/// </summary>
		public uint RegisterIndex { get; internal set; }

		/// <summary>
		/// Number of registers in this register bank
		/// </summary>
		public uint RegisterCount { get; internal set; }

		/// <summary>
		/// Number of components in the array (1-4). 1 means .x, 2 means .xy, etc.
		/// </summary>
		public uint NumComponents { get; internal set; }

		public static IndexableTempRegisterDeclarationToken Parse(BytecodeReader reader)
		{
			var token0 = reader.ReadUInt32();
			return new IndexableTempRegisterDeclarationToken
			{
				RegisterIndex = reader.ReadUInt32(),
				RegisterCount = reader.ReadUInt32(),
				NumComponents = reader.ReadUInt32()
			};
		}

		public override string ToString()
		{
			return string.Format("{0} x{1}[{2}], {3}", TypeDescription,
				RegisterIndex, RegisterCount, NumComponents);
		}
	}
}