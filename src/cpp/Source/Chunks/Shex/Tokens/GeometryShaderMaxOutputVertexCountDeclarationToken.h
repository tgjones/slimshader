#pragma once

#include "PCH.h"
#include "DeclarationToken.h"

namespace SlimShader
{
	/// <summary>
	/// Geometry Shader Maximum Output Vertex Count Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_MAX_OUTPUT_VERTEX_COUNT
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by a DWORD representing the
	/// maximum number of primitives that could be output
	/// by the Geometry Shader.
	/// </summary>
	class GeometryShaderMaxOutputVertexCountDeclarationToken : public DeclarationToken
	{
	public :
		static std::shared_ptr<GeometryShaderMaxOutputVertexCountDeclarationToken> Parse(BytecodeReader& reader);

		uint32_t GetMaxPrimitives() const;

	protected :
		virtual void Print(std::ostream& out) const;

	private :
		uint32_t _maxPrimitives;
	};
};