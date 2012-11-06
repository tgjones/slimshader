#pragma once

#include "PCH.h"
#include "ConstantBufferAccessPattern.h"
#include "DeclarationToken.h"

namespace SlimShader
{
	/// <summary>
	/// Constant Buffer Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_CONSTANT_BUFFER
	/// [11]    D3D10_SB_CONSTANT_BUFFER_ACCESS_PATTERN
	/// [23:12] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 1 operand:
	/// (1) Operand, starting with OperandToken0, defining which CB slot (cb#[size])
	///     is being declared. (operand type: D3D10_SB_OPERAND_TYPE_CONSTANT_BUFFER)
	///     The indexing dimension for the register must be 
	///     D3D10_SB_OPERAND_INDEX_DIMENSION_2D, where the first index specifies
	///     which cb#[] is being declared, and the second (array) index specifies the size 
	///     of the buffer, as a count of 32-bit*4 elements.  (As opposed to when the 
	///     cb#[] is used in shader instructions, and the array index represents which 
	///     location in the constant buffer is being referenced.)
	///     If the size is specified as 0, the CB size is not known (any size CB
	///     can be bound to the slot).
	///
	/// The order of constant buffer declarations in a shader indicates their
	/// relative priority from highest to lowest (hint to driver).
	/// </summary>
	class ConstantBufferDeclarationToken : public DeclarationToken
	{
	public :
		static std::shared_ptr<ConstantBufferDeclarationToken> Parse(BytecodeReader& reader);
		ConstantBufferAccessPattern GetAccessPattern() const;

	protected :
		virtual void Print(std::ostream& out) const;

	private :
		ConstantBufferDeclarationToken(Operand operand);

		ConstantBufferAccessPattern _accessPattern;
	};
};