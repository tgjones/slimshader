#pragma once

#include "PCH.h"
#include "BytecodeReader.h"
#include "NumberType.h"

namespace SlimShader
{
	/// <summary>
	/// Represents an int, float or uint.
	/// </summary>
	struct Number
	{
	public :
		static Number Parse(BytecodeReader& reader, NumberType type);

		Number(uint8_t rawBytes[4], NumberType type);
		Number() { }

		NumberType GetType() const;

		const uint8_t* AsBytes() const;
		int AsInt() const;
		uint32_t AsUInt() const;
		float AsFloat() const;

		friend std::ostream& operator<<(std::ostream& out, const Number& value);

	private :
		NumberType _type;
		union
		{
			uint8_t b[4];
			int i;
			uint32_t u;
			float f;
		} _value;
	};
};