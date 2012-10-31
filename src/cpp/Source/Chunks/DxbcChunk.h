#pragma once

#include "PCH.h"
#include "BytecodeReader.h"
#include "ChunkType.h"
#include "DxbcContainer.h"

using namespace std;

namespace SlimShader
{
	class DxbcChunk
	{
	public :
		static DxbcChunk Parse(const shared_ptr<BytecodeReader> chunkReader, shared_ptr<DxbcContainer> container);

	private :
		uint32_t _fourCc;
		ChunkType _chunkType;
		uint32_t _chunkSize;
	};
};