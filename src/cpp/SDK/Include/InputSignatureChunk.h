#pragma once

#include "PCH.h"
#include "InputOutputSignatureChunk.h"

namespace SlimShader
{
	class InputSignatureChunk : public InputOutputSignatureChunk
	{
	protected :
		virtual std::string GetOutputDescription() const;
	};
};