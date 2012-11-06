#pragma once

#include "PCH.h"
#include "BytecodeReader.h"
#include "OpcodeToken.h"
#include "Operand.h"

namespace SlimShader
{
	class DeclarationToken : public OpcodeToken
	{
	public :
		static std::shared_ptr<DeclarationToken> Parse(BytecodeReader& reader, OpcodeType opcodeType);

		const Operand& GetOperand() const;

	protected :
		DeclarationToken(Operand operand);
		DeclarationToken() { }

	private :
		const Operand _operand;
	};
};