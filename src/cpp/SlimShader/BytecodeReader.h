#pragma once

#include "stdafx.h"
#include <cstdint>
#include <string>

using namespace std;

namespace SlimShader
{
	class BytecodeReader
	{
	public :
		BytecodeReader(const uint8_t *bytecode, size_t length);
		BytecodeReader(const shared_ptr<BytecodeReader> reader, uint32_t length);

		const bool IsEndOfBuffer();
		const long GetCurrentPosition();

		uint8_t ReadUInt8();
		uint16_t ReadUInt16();
		uint32_t ReadUInt32();
		int32_t ReadInt32();
		float ReadFloat();
		double ReadDouble();
		string ReadString();

	private :
		const uint8_t* _bytecode;
		const uint8_t* _end;

		template <class T>
		T Read();
	};
};