using SlimShader.Util;

namespace SlimShader.Chunks.Shex.Tokens
{
	/// <summary>
	/// Typed Unordered Access View Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D11_SB_OPCODE_DCL_UNORDERED_ACCESS_VIEW_TYPED
	/// [15:11] D3D10_SB_RESOURCE_DIMENSION
	/// [16:16] D3D11_SB_GLOBALLY_COHERENT_ACCESS or 0 (LOCALLY_COHERENT)
	/// [23:17] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 2 operands:
	/// (1) an operand, starting with OperandToken0, defining which
	///     u# register (D3D11_SB_OPERAND_TYPE_UNORDERED_ACCESS_VIEW) is being declared.
	/// (2) a Resource Return Type token (ResourceReturnTypeToken)
	/// </summary>
	public class TypedUnorderedAccessViewDeclarationToken : UnorderedAccessViewDeclarationTokenBase
	{
		public ResourceDimension ResourceDimension { get; private set; }
		public ResourceReturnTypeToken ReturnType { get; private set; }

		public static TypedUnorderedAccessViewDeclarationToken Parse(BytecodeReader reader)
		{
			var token0 = reader.ReadUInt32();

			return new TypedUnorderedAccessViewDeclarationToken
			{
				ResourceDimension = token0.DecodeValue<ResourceDimension>(11, 15),
				Coherency = token0.DecodeValue<UnorderedAccessViewCoherency>(16, 16),
				Operand = Operand.Parse(reader, token0.DecodeValue<OpcodeType>(0, 10)),
				ReturnType = ResourceReturnTypeToken.Parse(reader)
			};
		}
	}
}