#pragma once

#include "PCH.h"
#include "CustomDataToken.h"

namespace SlimShader
{
	class ImmediateDeclarationToken : public CustomDataToken
	{
	public :
		uint32_t GetDeclarationLength() const;

	protected :
		uint32_t _declarationLength;
	};
};