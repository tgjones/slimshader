#pragma once

#include "PCH.h"
#include "BytecodeReader.h"

namespace SlimShader
{
	class ClassInstance
	{
	public :
		static ClassInstance Parse(const BytecodeReader& reader, BytecodeReader& classInstanceReader);

		std::string GetName() const;
		uint16_t GetType() const;
		uint16_t GetConstantBuffer() const;
		uint16_t GetConstantBufferOffset() const;
		uint16_t GetTexture() const;
		uint16_t GetSampler() const;

		friend std::ostream& operator<<(std::ostream& out, const ClassInstance& value);

	private :
		ClassInstance() { }

		std::string _name;
		uint16_t _type;
		uint16_t _constantBuffer;
		uint16_t _constantBufferOffset;
		uint16_t _texture;
		uint16_t _sampler;
	};
};