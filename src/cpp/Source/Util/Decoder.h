#pragma once

#include "PCH.h"

namespace SlimShader
{
	uint32_t GenerateMask(uint8_t start, uint8_t end);

	uint32_t DecodeValue(uint32_t token, uint8_t start, uint8_t end);

	template <class T>
	T DecodeValue(uint32_t token, uint8_t start, uint8_t end)
	{
		return (T) DecodeValue(token, start, end);
	}

	std::string ToFourCcString(uint32_t fourCc);

	template <class T>
	bool HasFlag(T value, T flag)
	{
		auto valueInt = static_cast<int>(value);
		auto flagInt = static_cast<int>(flag);
		return (valueInt & flagInt) == flagInt;
	}
};