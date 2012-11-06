#pragma once

#include "PCH.h"
#include "DeclarationToken.h"

namespace SlimShader
{
	/// <summary>
	/// Input or Output Register Indexing Range Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_INDEX_RANGE
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 2 operands:
	/// (1) an operand, starting with OperandToken0, defining which
	///     input (v#) or output (o#) register is having its array indexing range
	///     declared, including writemask.  For Geometry Shader inputs, 
	///     it is assumed that the vertex axis is always fully indexable,
	///     and 0 must be specified as the vertex# in this declaration, so that 
	///     only the a range of attributes are having their index range defined.
	///     
	/// (2) a DWORD representing the count of registers starting from the one
	///     indicated in (1).
	/// </summary>
	class IndexingRangeDeclarationToken : public DeclarationToken
	{
	public :
		static std::shared_ptr<IndexingRangeDeclarationToken> Parse(BytecodeReader& reader);

		/// <summary>
		/// Number of registers in this register bank
		/// </summary>
		uint32_t GetRegisterCount() const;

	protected :
		virtual void Print(std::ostream& out) const;

	private :
		IndexingRangeDeclarationToken(Operand operand);

		uint32_t _registerCount;
	};
};