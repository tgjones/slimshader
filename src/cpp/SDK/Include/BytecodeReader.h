#pragma once

#include "PCH.h"

namespace SlimShader
{
	class BytecodeReader
	{
	public :
		BytecodeReader(const uint8_t *bytecode, size_t length);
		BytecodeReader(const BytecodeReader& reader, uint32_t length);

		BytecodeReader CopyAtOffset(const uint32_t offset) const;

		bool IsEndOfBuffer() const;
		const uint8_t* GetCurrentPosition() const;

		uint8_t ReadUInt8();
		uint16_t ReadUInt16();
		uint32_t ReadUInt32();
		uint64_t ReadUInt64();
		int32_t ReadInt32();
		float ReadFloat();
		double ReadDouble();
		std::string ReadString();

	private :
		template <class T>
		T Read();

		const uint8_t* _bytecode;
		const uint8_t* _end;
	};
};