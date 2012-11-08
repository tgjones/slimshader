#pragma once

#include "PCH.h"
#include "BytecodeReader.h"
#include "NumberType.h"

namespace SlimShader
{
	/// <summary>
	/// Represents an int, float or uint.
	/// </summary>
	class Number
	{
	public :
		static Number Parse(BytecodeReader& reader, NumberType type);
		static Number FromRawBytes(uint8_t rawBytes[4], NumberType type);
		static Number FromFloat(float value);

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

	/// <summary>
	/// Represents four Numbers, or two doubles.
	/// </summary>
	union Number4
	{
	public :
		static Number4 FromRawBytes(uint8_t rawBytes[16]);

		uint8_t RawBytes[16];
		Number Numbers[4];
		double Doubles[2];
	};
};