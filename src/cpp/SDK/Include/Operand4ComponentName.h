#pragma once

#include "PCH.h"

namespace SlimShader
{
	enum class Operand4ComponentName
	{
		X = 0,
		Y = 1,
		Z = 2,
		W = 3,
		R = 0,
		G = 1,
		B = 2,
		A = 3
	};

	std::string ToString(Operand4ComponentName value);
};