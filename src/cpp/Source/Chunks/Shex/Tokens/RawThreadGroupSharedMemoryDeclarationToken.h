#pragma once

#include "PCH.h"
#include "DeclarationToken.h"

namespace SlimShader
{
	/// <summary>
	/// Raw Thread Group Shared Memory Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D11_SB_OPCODE_DCL_THREAD_GROUP_SHARED_MEMORY_RAW
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 2 operands:
	/// (1) an operand, starting with OperandToken0, defining which
	///     g# register (D3D11_SB_OPERAND_TYPE_THREAD_GROUP_SHARED_MEMORY) is being declared.
	/// (2) a DWORD indicating the element count, # of 32-bit scalars..
	/// </summary>
	class RawThreadGroupSharedMemoryDeclarationToken : public DeclarationToken
	{
	public :
		static std::shared_ptr<RawThreadGroupSharedMemoryDeclarationToken> Parse(BytecodeReader& reader);

		uint32_t GetElementCount() const;

	protected :
		virtual void Print(std::ostream& out) const;

	private :
		RawThreadGroupSharedMemoryDeclarationToken(Operand operand);

		uint32_t _elementCount;
	};
};