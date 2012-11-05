#pragma once

#include "PCH.h"
#include "UnorderedAccessViewDeclarationTokenBase.h"

namespace SlimShader
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
	class StructuredUnorderedAccessViewDeclarationToken : public UnorderedAccessViewDeclarationTokenBase
	{
	public :
		static std::shared_ptr<StructuredUnorderedAccessViewDeclarationToken> Parse(BytecodeReader& reader);

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
		bool HasOrderPreservingCounter() const;

		uint32_t GetByteStride();

	protected :
		virtual void Print(std::ostream& out) const;

	private :
		bool _hasOrderPreservingCounter;
		uint32_t _byteStride;
	};
};