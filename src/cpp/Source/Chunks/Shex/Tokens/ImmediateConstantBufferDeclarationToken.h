#pragma once

#include "PCH.h"
#include "ImmediateDeclarationToken.h"
#include "Number.h"

namespace SlimShader
{
	/// <summary>
	/// Immediate Constant Buffer Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_CUSTOMDATA
	/// [31:11] == D3D10_SB_CUSTOMDATA_DCL_IMMEDIATE_CONSTANT_BUFFER
	///
	/// OpcodeToken1 is followed by:
	/// (1) DWORD indicating length of declaration, including OpcodeToken0 and 1.
	///     This length must = 2(for OpcodeToken0 and 1) + a multiple of 4 
	///                                                    (# of immediate constants)
	/// (2) Sequence of 4-tuples of DWORDs defining the Immediate Constant Buffer.
	///     The number of 4-tuples is (length above - 1) / 4
	/// </summary>
	class ImmediateConstantBufferDeclarationToken : public ImmediateDeclarationToken
	{
	public :
		static std::shared_ptr<ImmediateConstantBufferDeclarationToken> Parse(BytecodeReader& reader);

		const std::vector<Number>& GetData() const;

	protected :
		virtual void Print(std::ostream& out) const;

	private :
		std::vector<Number> _data;
	};
};