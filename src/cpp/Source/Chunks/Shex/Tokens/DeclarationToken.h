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
		void SetOperand(Operand operand);

	private :
		Operand _operand;
	};
};