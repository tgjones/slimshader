#pragma once

#include "PCH.h"
#include "DeclarationToken.h"

namespace SlimShader
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
	class FunctionBodyDeclarationToken : public DeclarationToken
	{
	public :
		static std::shared_ptr<FunctionBodyDeclarationToken> Parse(BytecodeReader& reader);

		/// <summary>
		/// Function body identifier
		/// </summary>
		uint32_t GetIdentifier() const;

	protected :
		virtual void Print(std::ostream& out) const;

	private :
		uint32_t _identifier;
	};
};