#pragma once

#include "PCH.h"
#include "UnorderedAccessViewDeclarationTokenBase.h"

namespace SlimShader
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
	class RawUnorderedAccessViewDeclarationToken : public UnorderedAccessViewDeclarationTokenBase
	{
	public :
		static std::shared_ptr<RawUnorderedAccessViewDeclarationToken> Parse(BytecodeReader& reader);

	protected :
		virtual void Print(std::ostream& out) const;
	};
};