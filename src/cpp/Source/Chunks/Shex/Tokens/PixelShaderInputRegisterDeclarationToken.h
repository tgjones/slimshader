#pragma once

#include "PCH.h"
#include "InputRegisterDeclarationToken.h"
#include "InterpolationMode.h"

namespace SlimShader
{
	/// <summary>
	/// Pixel Shader Input Register Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_INPUT_PS
	/// [14:11] D3D10_SB_INTERPOLATION_MODE
	/// [23:15] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 1 operand:
	/// (1) Operand, starting with OperandToken0, defining which input
	///     v# register (D3D10_SB_OPERAND_TYPE_INPUT) is being declared,
	///     including writemask.
	/// 
	/// -------
	/// 
	/// Pixel Shader Input Register Declaration w/System Interpreted Value
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_INPUT_PS_SIV
	/// [14:11] D3D10_SB_INTERPOLATION_MODE
	/// [23:15] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 2 operands:
	/// (1) Operand, starting with OperandToken0, defining which input
	///     v# register (D3D10_SB_OPERAND_TYPE_INPUT) is being declared.
	/// (2) a System Interpreted Value Name (NameToken)
	/// 
	/// -------
	/// 
	/// Pixel Shader Input Register Declaration w/System Generated Value
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_INPUT_PS_SGV
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 2 operands:
	/// (1) Operand, starting with OperandToken0, defining which input
	///     v# register (D3D10_SB_OPERAND_TYPE_INPUT) is being declared.
	/// (2) a System Generated Value Name (NameToken)
	/// </summary>
	class PixelShaderInputRegisterDeclarationToken : public InputRegisterDeclarationToken
	{
	public :
		static std::shared_ptr<PixelShaderInputRegisterDeclarationToken> Parse(BytecodeReader& reader);

		/// <summary>
		/// Not applicable for D3D10_SB_OPCODE_DCL_INPUT_PS_SGV
		/// </summary>
		InterpolationMode GetInterpolationMode() const;

	protected :
		virtual void Print(std::ostream& out) const;

	private :
		InterpolationMode _interpolationMode;
	};
};