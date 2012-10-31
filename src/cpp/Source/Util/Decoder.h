#pragma once

#include "PCH.h"

using namespace std;

namespace SlimShader
{
	uint32_t decode(uint32_t token, uint8_t start, uint8_t end);

	template <class T>
	T decode(uint32_t token, uint8_t start, uint8_t end);
};