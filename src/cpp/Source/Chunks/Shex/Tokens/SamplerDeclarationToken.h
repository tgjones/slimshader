#pragma once

#include "PCH.h"
#include "DeclarationToken.h"
#include "SamplerMode.h"

namespace SlimShader
{
	/// <summary>
	/// Sampler Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_SAMPLER
	/// [14:11] D3D10_SB_SAMPLER_MODE
	/// [23:15] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 1 operand:
	/// (1) Operand starting with OperandToken0, defining which sampler
	///     (D3D10_SB_OPERAND_TYPE_SAMPLER) register # is being declared.
	/// </summary>
	class SamplerDeclarationToken : public DeclarationToken
	{
	public :
		static std::shared_ptr<SamplerDeclarationToken> Parse(BytecodeReader& reader);

		SamplerMode GetSamplerMode() const;

	protected :
		virtual void Print(std::ostream& out) const;

	private :
		SamplerDeclarationToken(Operand operand);

		SamplerMode _samplerMode;
	};
};