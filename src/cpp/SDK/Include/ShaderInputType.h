#pragma once

#include "PCH.h"

namespace SlimShader
{
	enum class ShaderInputType
	{
		CBuffer = 0,
		TBuffer = 1,
		Texture = 2,
		Sampler = 3,
		UavRwTyped = 4,
		Structured = 5,
		UavRwStructured = 6,
		ByteAddress = 7,
		UavRwByteAddress = 8,
		UavAppendStructured = 9,
		UavConsumeStructured = 10,
		UavRwStructuredWithCounter = 11
	};

	std::string ToString(ShaderInputType value);
};