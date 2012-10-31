using SlimShader.Util;

namespace SlimShader.Chunks.Shex.Tokens
{
	/// <summary>
	/// Interface function body Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_FUNCTION_BODY
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  If it is extended, then
	///         it contains the actual instruction length in DWORDs, since
	///         it may not fit into 7 bits if enough operands are defined.
	///
	/// OpcodeToken0 is followed by a DWORD that represents the function body
	/// identifier.
	/// </summary>
	public class FunctionBodyDeclarationToken : DeclarationToken
	{
		/// <summary>
		/// Function body identifier
		/// </summary>
		public uint Identifier { get; private set; }

		public static FunctionBodyDeclarationToken Parse(BytecodeReader reader)
		{
			uint token0 = reader.ReadUInt32();
			return new FunctionBodyDeclarationToken
			{
				Identifier = reader.ReadUInt32()
			};
		}

		public override string ToString()
		{
			return string.Format("{0} fb{1}", TypeDescription, Identifier);
		}
	}
}