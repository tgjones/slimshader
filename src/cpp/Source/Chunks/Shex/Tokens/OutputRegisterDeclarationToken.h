#pragma once

#include "PCH.h"
#include "DeclarationToken.h"
#include "SystemValueName.h"

namespace SlimShader
{
	/// <summary>
	/// Output Register Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_OUTPUT
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 1 operand:
	/// (1) Operand, starting with OperandToken0, defining which
	///     o# register (D3D10_SB_OPERAND_TYPE_OUTPUT) is being declared,
	///     including writemask.
	///     (in Pixel Shader, output can also be one of 
	///     D3D10_SB_OPERAND_TYPE_OUTPUT_DEPTH,
	///     D3D11_SB_OPERAND_TYPE_OUTPUT_DEPTH_GREATER_EQUAL, or
	///     D3D11_SB_OPERAND_TYPE_OUTPUT_DEPTH_LESS_EQUAL )
	///
	/// -------
	/// 
	/// Output Register Declaration w/System Interpreted Value
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_OUTPUT_SIV
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 2 operands:
	/// (1) an operand, starting with OperandToken0, defining which
	///     o# register (D3D10_SB_OPERAND_TYPE_OUTPUT) is being declared,
	///     including writemask.
	/// (2) a System Interpreted Name token (NameToken)
	///
	/// -------
	///
	/// Output Register Declaration w/System Generated Value
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_OUTPUT_SGV
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 2 operands:
	/// (1) an operand, starting with OperandToken0, defining which
	///     o# register (D3D10_SB_OPERAND_TYPE_OUTPUT) is being declared,
	///     including writemask.
	/// (2) a System Generated Name token (NameToken)
	/// </summary>
	class OutputRegisterDeclarationToken : public DeclarationToken
	{
	public :
		static std::shared_ptr<OutputRegisterDeclarationToken> Parse(BytecodeReader& reader);

		/// <summary>
		/// Only applicable for SGV and SIV declarations.
		/// </summary>
		SystemValueName GetSystemValueName() const;

	protected :
		virtual void Print(std::ostream& out) const;

	private :
		OutputRegisterDeclarationToken(Operand operand);

		SystemValueName _systemValueName;
	};
};