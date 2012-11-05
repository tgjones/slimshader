#pragma once

#include "PCH.h"
#include "DeclarationToken.h"

namespace SlimShader
{
	/// <summary>
	/// Interface function table Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_FUNCTION_TABLE
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  If it is extended, then
	///         it contains the actual instruction length in DWORDs, since
	///         it may not fit into 7 bits if enough functions are defined.
	///
	/// OpcodeToken0 is followed by a DWORD that represents the function table
	/// identifier and another DWORD (TableLength) that gives the number of
	/// functions in the table.
	///
	/// This is followed by TableLength DWORDs which are function body indices.
	/// </summary>
	class FunctionTableDeclarationToken : public DeclarationToken
	{
	public :
		static std::shared_ptr<FunctionTableDeclarationToken> Parse(BytecodeReader& reader);

		/// <summary>
		/// Function table identifier
		/// </summary>
		uint32_t GetIdentifier() const;

		/// <summary>
		/// Function body indices
		/// </summary>
		const std::vector<uint32_t>& GetFunctionBodyIndices() const;

	protected :
		virtual void Print(std::ostream& out) const;

	private :
		uint32_t _identifier;
		std::vector<uint32_t> _functionBodyIndices;
	};
};