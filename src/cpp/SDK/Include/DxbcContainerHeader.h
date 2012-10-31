#pragma once

#include "PCH.h"
#include "BytecodeReader.h"

using namespace std;

namespace SlimShader
{
	class DxbcContainerHeader
	{
	public :
		static DxbcContainerHeader Parse(shared_ptr<BytecodeReader> reader);

		uint32_t GetFourCc();
		uint32_t* GetUniqueKey();
		uint32_t GetOne();
		uint32_t GetTotalSize();
		uint32_t GetChunkCount();

	private :
		uint32_t _fourCc;
		uint32_t _uniqueKey[4];
		uint32_t _one;
		uint32_t _totalSize;
		uint32_t _chunkCount;
	};
};