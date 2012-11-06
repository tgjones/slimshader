#include "PCH.h"
#include "Number.h"

using namespace std;
using namespace SlimShader;

Number Number::Parse(BytecodeReader& reader, NumberType type)
{
	const int byteCount = 4;
	uint8_t bytes[byteCount];
	for (int i = 0; i < byteCount; i++)
		bytes[i] = reader.ReadUInt8();
	return Number(bytes, type);
}

Number::Number(uint8_t rawBytes[4], NumberType type)
	: _type(type)
{
	_value.b[0] = rawBytes[0];
	_value.b[1] = rawBytes[1];
	_value.b[2] = rawBytes[2];
	_value.b[3] = rawBytes[3];
}

NumberType Number::GetType() const { return _type; }
const uint8_t* Number::AsBytes() const { return &_value.b[0]; }
int Number::AsInt() const { return _value.i; }
uint32_t Number::AsUInt() const { return _value.u; }
float Number::AsFloat() const { return _value.f; }

std::ostream& SlimShader::operator<<(std::ostream& out, const Number& value)
{
	const int hexThreshold = 10000; // This is the correct value, derived through fxc.exe and a bisect-search.
	const uint32_t negThreshold = 0xFFFFFFF0; // TODO: Work out the actual negative threshold.
	const int floatThresholdPos = 0x00700000; // TODO: Work out the actual float threshold.
	const int floatThresholdNeg = -0x00700000; // TODO: Work out the actual float threshold.
	switch (value._type)
	{
	case NumberType::Int:
	NumberTypeInt :
		if (value.AsInt() > hexThreshold)
			out << "0x" << boost::format("%08x") % value.AsInt();
		else
			out << value.AsInt();
		break;
	case NumberType::UInt:
		if (value.AsUInt() > negThreshold)
			out << value.AsInt();
		else if (value.AsUInt() > hexThreshold)
			out << "0x" << boost::format("%08x") % value.AsUInt();
		else
			out << value.AsUInt();
		break;
	case NumberType::Float:
	NumberTypeFloat :
		if (value.AsBytes()[0] == 0 && value.AsBytes()[1] == 0 && value.AsBytes()[2] == 0 && value.AsBytes()[3] == 128)
			out << "-0.000000"; // "Negative" zero
		else
			out << boost::format("%6f") % ((double) value.AsFloat());
		break;
	case NumberType::Unknown:
		// fxc.exe has some strange rules for formatting output of numbers of 
		// unknown type - for example, as operands to the mov op. It only matters for string output -
		// when actually executing these instructions that can have operands of unknown type, they simply
		// move bytes around without interpreting them - this is from the mov doc page:
		// "The modifiers, other than swizzle, assume the data is floating point. The absence of modifiers 
		// just moves data without altering bits."
		if (value.AsInt() < floatThresholdNeg || value.AsInt() > floatThresholdPos)
			goto NumberTypeFloat;
		goto NumberTypeInt;
	default:
		throw runtime_error("Type '" + to_string((int) value._type) + "' is not supported.");
	}

	return out;
}