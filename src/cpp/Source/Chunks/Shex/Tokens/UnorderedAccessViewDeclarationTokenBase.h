#pragma once

#include "PCH.h"
#include "DeclarationToken.h"
#include "UnorderedAccessViewCoherency.h"

namespace SlimShader
{
	class UnorderedAccessViewDeclarationTokenBase : public DeclarationToken
	{
	public :
		UnorderedAccessViewCoherency GetCoherency() const;

	protected :
		UnorderedAccessViewDeclarationTokenBase(UnorderedAccessViewCoherency coherency, Operand operand);

	private:
		const UnorderedAccessViewCoherency _coherency;
	};
};