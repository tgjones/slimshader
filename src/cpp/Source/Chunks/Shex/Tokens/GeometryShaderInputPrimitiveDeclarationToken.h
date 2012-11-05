#pragma once

#include "PCH.h"
#include "DeclarationToken.h"
#include "Primitive.h"

namespace SlimShader
{
	/// <summary>
	/// Geometry Shader Input Primitive Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_GS_INPUT_PRIMITIVE
	/// [16:11] D3D10_SB_PRIMITIVE [not D3D10_SB_PRIMITIVE_TOPOLOGY]
	/// [23:17] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token. == 1
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	/// </summary>
	class GeometryShaderInputPrimitiveDeclarationToken : public DeclarationToken
	{
	public :
		static std::shared_ptr<GeometryShaderInputPrimitiveDeclarationToken> Parse(BytecodeReader& reader);

		Primitive GetPrimitive() const;

	protected :
		virtual void Print(std::ostream& out) const;

	private :
		Primitive _primitive;
	};
};