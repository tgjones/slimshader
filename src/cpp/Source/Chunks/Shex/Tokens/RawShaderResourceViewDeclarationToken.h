#pragma once

#include "PCH.h"
#include "DeclarationToken.h"

namespace SlimShader
{
	/// <summary>
	/// Raw Shader Resource View Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D11_SB_OPCODE_DCL_RESOURCE_RAW
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 2 operands:
	/// (1) an operand, starting with OperandToken0, defining which
	///     t# register (D3D10_SB_OPERAND_TYPE_RESOURCE) is being declared.
	/// </summary>
	class RawShaderResourceViewDeclarationToken : public DeclarationToken
	{
	public :
		static std::shared_ptr<RawShaderResourceViewDeclarationToken> Parse(BytecodeReader& reader);

	protected :
		virtual void Print(std::ostream& out) const;
	};
};