#pragma once

#include "PCH.h"
#include "DeclarationToken.h"

namespace SlimShader
{
	/// <summary>
	/// Structured Thread Group Shared Memory Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D11_SB_OPCODE_DCL_THREAD_GROUP_SHARED_MEMORY_STRUCTURED
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 3 operands:
	/// (1) an operand, starting with OperandToken0, defining which
	///     g# register (D3D11_SB_OPERAND_TYPE_THREAD_GROUP_SHARED_MEMORY) is 
	///     being declared.
	/// (2) a DWORD indicating UINT32 struct byte stride
	/// (3) a DWORD indicating UINT32 struct count
	/// </summary>
	class StructuredThreadGroupSharedMemoryDeclarationToken : public DeclarationToken
	{
	public :
		static std::shared_ptr<StructuredThreadGroupSharedMemoryDeclarationToken> Parse(BytecodeReader& reader);

		uint32_t GetStructByteSize() const;
		uint32_t GetStructCount() const;

	protected :
		virtual void Print(std::ostream& out) const;

	private :
		StructuredThreadGroupSharedMemoryDeclarationToken(Operand operand);

		uint32_t _structByteSize;
		uint32_t _structCount;
	};
};