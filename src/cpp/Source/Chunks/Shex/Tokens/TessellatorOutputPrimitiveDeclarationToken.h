#pragma once

#include "PCH.h"
#include "DeclarationToken.h"
#include "TessellatorOutputPrimitive.h"

namespace SlimShader
{
	/// <summary>
	/// Hull Shader Declaration Phase: Tessellator Output Primitive
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D11_SB_OPCODE_DCL_TESS_OUTPUT_PRIMITIVE
	/// [13:11] Output Primitive
	/// [23:14] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token. == 1
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	/// </summary>
	class TessellatorOutputPrimitiveDeclarationToken : public DeclarationToken
	{
	public :
		static std::shared_ptr<TessellatorOutputPrimitiveDeclarationToken> Parse(BytecodeReader& reader);

		TessellatorOutputPrimitive GetOutputPrimitive() const;

	protected :
		virtual void Print(std::ostream& out) const;

	private :
		TessellatorOutputPrimitive _outputPrimitive;
	};
};