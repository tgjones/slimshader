using SlimShader.Util;

namespace SlimShader.Chunks.Shex.Tokens
{
	/// <summary>
	/// Raw Unordered Access View Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D11_SB_OPCODE_DCL_UNORDERED_ACCESS_VIEW_RAW
	/// [15:11] Ignored, 0
	/// [16:16] D3D11_SB_GLOBALLY_COHERENT_ACCESS or 0 (LOCALLY_COHERENT)
	/// [23:17] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 1 operand:
	/// (1) an operand, starting with OperandToken0, defining which
	///     u# register (D3D11_SB_OPERAND_TYPE_UNORDERED_ACCESS_VIEW) is being declared.
	/// </summary>
	public class RawUnorderedAccessViewDeclarationToken : UnorderedAccessViewDeclarationTokenBase
	{
		public static RawUnorderedAccessViewDeclarationToken Parse(BytecodeReader reader)
		{
			var token0 = reader.ReadUInt32();
			return new RawUnorderedAccessViewDeclarationToken
			{
				Coherency = token0.DecodeValue<UnorderedAccessViewCoherency>(16, 16),
				Operand = Operand.Parse(reader, token0.DecodeValue<OpcodeType>(0, 10))
			};
		}

		public override string ToString()
		{
			return string.Format("{0} {1}", TypeDescription, Operand);
		}
	}
}