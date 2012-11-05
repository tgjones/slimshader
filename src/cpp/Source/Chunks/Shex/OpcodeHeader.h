#pragma once

#include "PCH.h"
#include "OpcodeType.h"

namespace SlimShader
{
	struct OpcodeHeader
	{
	public :
		OpcodeType OpcodeType;
		uint32_t Length;
		bool IsExtended;
	};
};