using SlimShader.Util;

namespace SlimShader.Chunks.Shex.Tokens
{
	/// <summary>
	/// Structured Unordered Access View Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D11_SB_OPCODE_DCL_UNORDERED_ACCESS_VIEW_STRUCTURED
	/// [15:11] Ignored, 0
	/// [16:16] D3D11_SB_GLOBALLY_COHERENT_ACCESS or 0 (LOCALLY_COHERENT)
	/// [22:17] Ignored, 0
	/// [23:23] D3D11_SB_UAV_HAS_ORDER_PRESERVING_COUNTER or 0
	///
	///            The presence of this flag means that if a UAV is bound to the
	///            corresponding slot, it must have been created with 
	///            D3D11_BUFFER_UAV_FLAG_COUNTER at the API.  Also, the shader
	///            can contain either imm_atomic_alloc or _consume instructions
	///            operating on the given UAV.
	/// 
	///            If this flag is not present, the shader can still contain
	///            either imm_atomic_alloc or imm_atomic_consume instructions for
	///            this UAV.  But if such instructions are present in this case,
	///            and a UAV is bound corresponding slot, it must have been created 
	///            with the D3D11_BUFFER_UAV_FLAG_APPEND flag at the API.
	///            Append buffers have a counter as well, but values returned 
	///            to the shader are only valid for the lifetime of the shader 
	///            invocation.
	///
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 2 operands:
	/// (1) an operand, starting with OperandToken0, defining which
	///     u# register (D3D11_SB_OPERAND_TYPE_UNORDERED_ACCESS_VIEW) is 
	///     being declared.
	/// (2) a DWORD indicating UINT32 byte stride
	/// </summary>
	public class StructuredUnorderedAccessViewDeclarationToken : UnorderedAccessViewDeclarationTokenBase
	{
		/// <summary>
		/// The presence of this flag means that if a UAV is bound to the
		/// corresponding slot, it must have been created with 
		/// D3D11_BUFFER_UAV_FLAG_COUNTER at the API.  Also, the shader
		/// can contain either imm_atomic_alloc or _consume instructions
		/// operating on the given UAV.
		/// 
		/// If this flag is not present, the shader can still contain
		/// either imm_atomic_alloc or imm_atomic_consume instructions for
		/// this UAV.  But if such instructions are present in this case,
		/// and a UAV is bound corresponding slot, it must have been created 
		/// with the D3D11_BUFFER_UAV_FLAG_APPEND flag at the API.
		/// Append buffers have a counter as well, but values returned 
		/// to the shader are only valid for the lifetime of the shader 
		/// invocation.
		/// </summary>
		public bool HasOrderPreservingCounter { get; private set; }

		public uint ByteStride { get; private set; }

		public static StructuredUnorderedAccessViewDeclarationToken Parse(BytecodeReader reader)
		{
			var token0 = reader.ReadUInt32();
			return new StructuredUnorderedAccessViewDeclarationToken
			{
				Coherency = token0.DecodeValue<UnorderedAccessViewCoherency>(16, 16),
				HasOrderPreservingCounter = (token0.DecodeValue(23, 23) == 0),
				Operand = Operand.Parse(reader, token0.DecodeValue<OpcodeType>(0, 10)),
				ByteStride = reader.ReadUInt32()
			};
		}

		public override string ToString()
		{
			return string.Format("{0} {1}, {2}", TypeDescription, Operand, ByteStride);
		}
	}
}