#pragma once

#include "PCH.h"
#include "DxbcChunk.h"

namespace SlimShader
{
	class StatisticsChunk : public DxbcChunk
	{
	public :
		static StatisticsChunk Parse(shared_ptr<BytecodeReader> reader, uint32_t chunkSize);
	};
};