#pragma once

#include "PCH.h"
#include "DeclarationToken.h"
#include "SystemValueName.h"

namespace SlimShader
{
	/// <summary>
	/// Input Register Declaration (see separate declarations for Pixel Shaders)
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_INPUT
	/// [23:11] Ignored, 0
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
	/// Input Register Declaration w/System Interpreted Value
	/// (see separate declarations for Pixel Shaders)
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_INPUT_SIV
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 2 operands:
	/// (1) Operand, starting with OperandToken0, defining which input
	///     v# register (D3D10_SB_OPERAND_TYPE_INPUT) is being declared,
	///     including writemask.  For Geometry Shaders, the input is 
	///     v[vertex][attribute], and this declaration is only for which register 
	///     on the attribute axis is being declared.  The vertex axis value must 
	///     be equal to the # of vertices in the current input primitive for the GS
	///     (i.e. 6 for triangle + adjacency).
	/// (2) a System Interpreted Value Name (NameToken)
	/// 
	/// -------
	/// 
	/// Input Register Declaration w/System Generated Value
	/// (available for all shaders incl. Pixel Shader, no interpolation mode needed)
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_INPUT_SGV
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 2 operands:
	/// (1) Operand, starting with OperandToken0, defining which input
	///     v# register (D3D10_SB_OPERAND_TYPE_INPUT) is being declared,
	///     including writemask.
	/// (2) a System Generated Value Name (NameToken)
	/// </summary>	
	class InputRegisterDeclarationToken : public DeclarationToken
	{
	public :
		static std::shared_ptr<InputRegisterDeclarationToken> Parse(BytecodeReader& reader);

		/// <summary>
		/// Only applicable for SGV and SIV declarations.
		/// </summary>
		SystemValueName GetSystemValueName() const;

	protected :
		InputRegisterDeclarationToken(Operand operand);

		virtual void Print(std::ostream& out) const;

	private :
		SystemValueName _systemValueName;
	};
};