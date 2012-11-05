#pragma once

#include "PCH.h"
#include "DeclarationToken.h"
#include "ResourceDimension.h"
#include "ResourceReturnTypeToken.h"

namespace SlimShader
{
	/// <summary>
	/// Resource Declaration (non multisampled)
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_RESOURCE
	/// [15:11] D3D10_SB_RESOURCE_DIMENSION
	/// [23:16] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 2 operands:
	/// (1) an operand, starting with OperandToken0, defining which
	///     t# register (D3D10_SB_OPERAND_TYPE_RESOURCE) is being declared.
	/// (2) a Resource Return Type token (ResourceReturnTypeToken)
	///
	/// -------
	/// 
	/// Resource Declaration (multisampled)
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_RESOURCE (same opcode as non-multisampled case)
	/// [15:11] D3D10_SB_RESOURCE_DIMENSION (must be TEXTURE2DMS or TEXTURE2DMSARRAY)
	/// [22:16] Sample count 1...127.  0 is currently disallowed, though
	///         in future versions 0 could mean "configurable" sample count
	/// [23:23] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 2 operands:
	/// (1) an operand, starting with OperandToken0, defining which
	///     t# register (D3D10_SB_OPERAND_TYPE_RESOURCE) is being declared.
	/// (2) a Resource Return Type token (ResourceReturnTypeToken)
	/// </summary>
	class ResourceDeclarationToken : public DeclarationToken
	{
	public :
		static std::shared_ptr<ResourceDeclarationToken> Parse(BytecodeReader& reader);

		ResourceDimension GetResourceDimension() const;
		uint8_t GetSampleCount() const;
		const ResourceReturnTypeToken& GetReturnType() const;
		bool IsMultiSampled() const;

	protected :
		virtual void Print(std::ostream& out) const;
	};
};