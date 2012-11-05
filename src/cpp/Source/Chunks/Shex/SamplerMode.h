#pragma once

#include "PCH.h"

namespace SlimShader
{
	enum class SamplerMode
	{
		Default = 0,
		Comparison = 1,
		Mono = 2
	};

	std::string ToString(SamplerMode value);
};