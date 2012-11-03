#pragma once

#include "PCH.h"
#include "BytecodeReader.h"

namespace SlimShader
{
	class ClassType
	{
	public :
		static ClassType Parse(const BytecodeReader& reader, BytecodeReader& classTypeReader);

		std::string GetName() const;
		uint32_t GetID() const;
		uint32_t GetConstantBufferStride() const;
		uint32_t GetTexture() const;
		uint32_t GetSampler() const;

		friend std::ostream& operator<<(std::ostream& out, const ClassType& value);

	private :
		ClassType() { }

		std::string _name;
		uint32_t _id;
		uint32_t _constantBufferStride;
		uint32_t _texture;
		uint32_t _sampler;
	};
};