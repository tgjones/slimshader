#pragma once

#include "PCH.h"
#include "DxbcChunk.h"

namespace SlimShader
{
	class Sfi0Chunk : public DxbcChunk
	{
	public :
		static Sfi0Chunk Parse(shared_ptr<BytecodeReader> reader);
	};
};