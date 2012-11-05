#pragma once

#include "PCH.h"
#include "DeclarationToken.h"
#include "TessellatorDomain.h"

namespace SlimShader
{
	/// <summary>
	/// Hull Shader Declaration Phase: Tessellator Domain
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D11_SB_OPCODE_DCL_TESS_DOMAIN
	/// [12:11] Domain
	/// [23:13] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token. == 1
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	/// </summary>
	class TessellatorDomainDeclarationToken : public DeclarationToken
	{
	public :
		static std::shared_ptr<TessellatorDomainDeclarationToken> Parse(BytecodeReader& reader);

		TessellatorDomain GetDomain() const;

	protected :
		virtual void Print(std::ostream& out) const;

	private :
		TessellatorDomain _domain;
	};
};