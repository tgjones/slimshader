#include "PCH.h"
#include "Decoder.h"
#include <cmath>

using namespace SlimShader;

uint32_t generate_mask(uint8_t start, uint8_t end)
{
	uint32_t mask = 0;
	for (int i = start; i <= end; i++)
		mask |= (uint32_t) pow(2, i);
	return mask;
}

uint32_t decode(uint32_t token, uint8_t start, uint8_t end)
{
	const uint32_t mask = generate_mask(start, end);
	const uint8_t shift = start;

	return (token & mask) >> shift;
}

template <class T>
T decode(uint32_t token, uint8_t start, uint8_t end)
{
	return (T) decode(token, start, end);
}