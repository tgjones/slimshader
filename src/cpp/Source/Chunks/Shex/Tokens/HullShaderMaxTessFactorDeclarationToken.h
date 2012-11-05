#pragma once

#include "PCH.h"
#include "DeclarationToken.h"

namespace SlimShader
{
	/// <summary>
	/// Hull Shader Declaration Phase: Hull Shader Max Tessfactor
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D11_SB_OPCODE_DCL_HS_MAX_TESSFACTOR
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by a float32 representing the
	/// maximum TessFactor.
	/// </summary>
	class HullShaderMaxTessFactorDeclarationToken : public DeclarationToken
	{
	public :
		static std::shared_ptr<HullShaderMaxTessFactorDeclarationToken> Parse(BytecodeReader& reader);

		float GetMaxTessFactor() const;

	protected :
		virtual void Print(std::ostream& out) const;

	private :
		float _maxTessFactor;
	};
};