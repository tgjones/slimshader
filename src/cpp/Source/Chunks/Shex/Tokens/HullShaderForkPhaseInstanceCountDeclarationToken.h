#pragma once

#include "PCH.h"
#include "DeclarationToken.h"

namespace SlimShader
{
	/// <summary>
	/// Hull Shader Declaration Phase: Hull Shader Fork Phase Instance Count
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D11_SB_OPCODE_DCL_HS_FORK_PHASE_INSTANCE_COUNT
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by a UINT32 representing the
	/// number of instances of the current fork phase program to execute.
	/// </summary>
	class HullShaderForkPhaseInstanceCountDeclarationToken : public DeclarationToken
	{
	public :
		static std::shared_ptr<HullShaderForkPhaseInstanceCountDeclarationToken> Parse(BytecodeReader& reader);

		uint32_t GetInstanceCount() const;

	protected :
		virtual void Print(std::ostream& out) const;

	private :
		uint32_t _instanceCount;
	};
};