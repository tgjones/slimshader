#include "stdafx.h"
#include "BytecodeReader.h"

using namespace SlimShader;

BytecodeReader::BytecodeReader(const uint8_t *bytecode, size_t length)
	: _bytecode(bytecode), _end(bytecode + length)
{

}

const bool BytecodeReader::IsEndOfBuffer()
{
	return _bytecode >= _end;
}

uint8_t BytecodeReader::ReadUInt8()
{
	return Read<uint8_t>();
}

uint16_t BytecodeReader::ReadUInt16()
{
	return Read<uint16_t>();
}

uint32_t BytecodeReader::ReadUInt32()
{
	return Read<uint32_t>();
}

int32_t BytecodeReader::ReadInt32()
{
	return Read<int32_t>();
}

float BytecodeReader::ReadFloat()
{
	return Read<float>();
}

double BytecodeReader::ReadDouble()
{
	return Read<double>();
}

string BytecodeReader::ReadString()
{
	string result;
	while (!IsEndOfBuffer())
	{
		char nextCharacter = *_bytecode;
		if (nextCharacter == 0)
			break;

		result.push_back(nextCharacter);
		++_bytecode;
	}
	return result;
}

template <class T>
T BytecodeReader::Read()
{
	if ((_bytecode + sizeof(T)) > _end)
		throw std::runtime_error("Unexpected end of file");
	auto result = *reinterpret_cast<const T*>(_bytecode);
	_bytecode += sizeof(T);
	return result;
}