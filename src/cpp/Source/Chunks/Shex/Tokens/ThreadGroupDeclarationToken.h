#pragma once

#include "PCH.h"
#include "DeclarationToken.h"

namespace SlimShader
{
	/// <summary>
	/// Thread Group Declaration (Compute Shader)
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D11_SB_OPCODE_DCL_THREAD_GROUP
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  If it is extended, then
	///         it contains the actual instruction length in DWORDs, since
	///         it may not fit into 7 bits if enough types are used.
	///
	/// OpcodeToken0 is followed by 3 DWORDs, the Thread Group dimensions as UINT32:
	/// x, y, z
	/// </summary>
	class ThreadGroupDeclarationToken : public DeclarationToken
	{
	public :
		static std::shared_ptr<ThreadGroupDeclarationToken> Parse(BytecodeReader& reader);

		const uint32_t* GetDimensions() const;

	protected :
		virtual void Print(std::ostream& out) const;

	private :
		uint32_t _dimensions[3];
	};
};