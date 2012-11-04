#pragma once

#include "PCH.h"

namespace SlimShader
{
	enum class InterpolationMode
	{
		Undefined = 0,
		Constant = 1,
		Linear = 2,
		LinearCentroid = 3,
		LinearNoPerspective = 4,
		LinearNoPerspectiveCentroid = 5,

		// Following are new in DX10.1

		LinearSample = 6,
		LinearNoPerspectiveSample = 7
	};

	std::string ToString(InterpolationMode value);
};