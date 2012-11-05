#pragma once

#include "PCH.h"
#include "DeclarationToken.h"

namespace SlimShader
{
	/// <summary>
	/// Temp Register Declaration r0...r(n-1) 
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_TEMPS
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 1 operand:
	/// (1) DWORD (unsigned int) indicating how many temps are being declared.  
	///     i.e. 5 means r0...r4 are declared.
	/// </summary>
	class TempRegisterDeclarationToken : public DeclarationToken
	{
	public :
		static std::shared_ptr<TempRegisterDeclarationToken> Parse(BytecodeReader& reader);

		/// <summary>
		/// Indicates how many temps are being declared. i.e. 5 means r0...r4 are declared.
		/// </summary>
		uint32_t GetTempCount() const;

	protected :
		virtual void Print(std::ostream& out) const;

	private :
		uint32_t _tempCount;
	};
};