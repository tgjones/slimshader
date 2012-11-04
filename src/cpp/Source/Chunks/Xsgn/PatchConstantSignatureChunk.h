#pragma once

#include "PCH.h"
#include "InputOutputSignatureChunk.h"

namespace SlimShader
{
	class PatchConstantSignatureChunk : public InputOutputSignatureChunk
	{
	protected :
		virtual std::string GetOutputDescription() const;
	};
};