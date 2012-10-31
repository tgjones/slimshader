#pragma once

#include "PCH.h"
#include "DxbcChunk.h"

namespace SlimShader
{
	class InterfacesChunk : public DxbcChunk
	{
	public :
		static InterfacesChunk Parse(shared_ptr<BytecodeReader> reader, uint32_t sizeInBytes);
	};
};