#pragma once

#include "PCH.h"
#include "BytecodeReader.h"
#include "ChunkType.h"
#include "DxbcContainer.h"

namespace SlimShader
{
	class DxbcChunk
	{
	public :
		static std::shared_ptr<DxbcChunk> Parse(BytecodeReader& chunkReader, const DxbcContainer& container);
		
		virtual ~DxbcChunk() = 0; // Force DxbcChunk to be abstract.

		uint32_t GetFourCc() const;
		ChunkType GetChunkType() const;
		uint32_t GetChunkSize() const;

	private :
		uint32_t _fourCc;
		ChunkType _chunkType;
		uint32_t _chunkSize;
	};

	inline DxbcChunk::~DxbcChunk() { }
};