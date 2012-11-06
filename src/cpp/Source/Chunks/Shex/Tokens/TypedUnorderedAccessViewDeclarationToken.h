#pragma once

#include "PCH.h"
#include "UnorderedAccessViewDeclarationTokenBase.h"
#include "ResourceDimension.h"
#include "ResourceReturnTypeToken.h"

namespace SlimShader
{
	/// <summary>
	/// Typed Unordered Access View Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D11_SB_OPCODE_DCL_UNORDERED_ACCESS_VIEW_TYPED
	/// [15:11] D3D10_SB_RESOURCE_DIMENSION
	/// [16:16] D3D11_SB_GLOBALLY_COHERENT_ACCESS or 0 (LOCALLY_COHERENT)
	/// [23:17] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 2 operands:
	/// (1) an operand, starting with OperandToken0, defining which
	///     u# register (D3D11_SB_OPERAND_TYPE_UNORDERED_ACCESS_VIEW) is being declared.
	/// (2) a Resource Return Type token (ResourceReturnTypeToken)
	/// </summary>
	class TypedUnorderedAccessViewDeclarationToken : public UnorderedAccessViewDeclarationTokenBase
	{
	public :
		static std::shared_ptr<TypedUnorderedAccessViewDeclarationToken> Parse(BytecodeReader& reader);

		ResourceDimension GetResourceDimension() const;
		const ResourceReturnTypeToken& GetReturnType() const;

	protected :
		virtual void Print(std::ostream& out) const;

	private :
		TypedUnorderedAccessViewDeclarationToken(UnorderedAccessViewCoherency coherency, Operand operand);

		ResourceDimension _resourceDimension;
		ResourceReturnTypeToken _returnType;
	};
};