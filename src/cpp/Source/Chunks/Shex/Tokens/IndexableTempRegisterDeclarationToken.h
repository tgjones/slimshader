#pragma once

#include "PCH.h"
#include "DeclarationToken.h"

namespace SlimShader
{
	/// <summary>
	/// Indexable Temp Register (x#[size]) Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_INDEXABLE_TEMP
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 3 DWORDs:
	/// (1) Register index (defines which x# register is declared)
	/// (2) Number of registers in this register bank
	/// (3) Number of components in the array (1-4). 1 means .x, 2 means .xy etc.
	/// </summary>
	class IndexableTempRegisterDeclarationToken : public DeclarationToken
	{
	public :
		static std::shared_ptr<IndexableTempRegisterDeclarationToken> Parse(BytecodeReader& reader);

		/// <summary>
		/// Register index (defines which x# register is declared)
		/// </summary>
		uint32_t GetRegisterIndex() const;

		/// <summary>
		/// Number of registers in this register bank
		/// </summary>
		uint32_t GetRegisterCount() const;

		/// <summary>
		/// Number of components in the array (1-4). 1 means .x, 2 means .xy, etc.
		/// </summary>
		uint32_t GetNumComponents() const;

	protected :
		virtual void Print(std::ostream& out) const;

	private :
		uint32_t _registerIndex;
		uint32_t _registerCount;
		uint32_t _numComponents;
	};
};