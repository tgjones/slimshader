#pragma once

#include "PCH.h"

namespace SlimShader
{
	enum class RegisterComponentType
	{
		Unknown = 0,
		UInt32 = 1,
		SInt32 = 2,
		Float32 = 3
	};

	std::string ToString(RegisterComponentType value);
};