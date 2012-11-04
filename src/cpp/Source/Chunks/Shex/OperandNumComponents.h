#pragma once

#include "PCH.h"

namespace SlimShader
{
	/// <summary>
	/// Number of components in data vector referred to by operand
	/// </summary>
	enum class OperandNumComponents
	{
		Zero = 0,
		One = 1,
		Four = 2,
		N = 3 // unused for now
	};
};