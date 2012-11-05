#pragma once

#include "PCH.h"
#include "DeclarationToken.h"
#include "PrimitiveTopology.h"

namespace SlimShader
{
	/// <summary>
	/// Geometry Shader Output Topology Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_GS_OUTPUT_PRIMITIVE_TOPOLOGY
	/// [17:11] D3D10_SB_PRIMITIVE_TOPOLOGY
	/// [23:18] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token. == 1
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	/// </summary>
	class GeometryShaderOutputPrimitiveTopologyDeclarationToken : public DeclarationToken
	{
	public :
		static std::shared_ptr<GeometryShaderOutputPrimitiveTopologyDeclarationToken> Parse(BytecodeReader& reader);

		PrimitiveTopology GetPrimitiveTopology() const;

	protected :
		virtual void Print(std::ostream& out) const;

	private :
		PrimitiveTopology _primitiveTopology;
	};
};