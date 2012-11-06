#include "PCH.h"
#include "Decoder.h"

using namespace std;
using namespace SlimShader;

uint32_t SlimShader::GenerateMask(uint8_t start, uint8_t end)
{
	uint32_t mask = 0;
	for (int i = start; i <= end; i++)
		mask |= (uint32_t) pow(2, i);
	return mask;
}

uint32_t SlimShader::DecodeValue(uint32_t token, uint8_t start, uint8_t end)
{
	const uint32_t mask = GenerateMask(start, end);
	const uint8_t shift = start;

	return (token & mask) >> shift;
}

int8_t SlimShader::DecodeSigned4BitValue(uint32_t token, uint8_t start, uint8_t end)
{
	if (end - start != 3)
		throw runtime_error("Not the right length");
	auto value = DecodeValue<int8_t>(token, start, end);
	if (value > 7)
		return (int8_t) (value - 16);
	return value;
}

string SlimShader::ToFourCcString(uint32_t fourCc)
{
	char chars[4];
	chars[0] = DecodeValue<char>(fourCc, 0, 7);
	chars[1] = DecodeValue<char>(fourCc, 8, 15);
	chars[2] = DecodeValue<char>(fourCc, 16, 23);
	chars[3] = DecodeValue<char>(fourCc, 24, 31);
	return string(chars, 4);
}