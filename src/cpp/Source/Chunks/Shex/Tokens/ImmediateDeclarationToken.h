#pragma once

#include "PCH.h"
#include "DeclarationToken.h"

namespace SlimShader
{
	class ImmediateDeclarationToken : public DeclarationToken
	{
	public :
		const uint32_t GetDeclarationLength() const;

	protected :
		uint32_t _declarationLength;
	};
};