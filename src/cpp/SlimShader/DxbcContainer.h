#pragma once

#include "stdafx.h"
#include <cstdint>
#include <memory>
#include <vector>
#include "BytecodeReader.h"
#include "DxbcContainerHeader.h"

using namespace std;

namespace SlimShader
{
	class DxbcChunk;

	class DxbcContainer
	{
	public :
		static DxbcContainer Parse(const uint8_t bytes[], const int length);
		static DxbcContainer Parse(const shared_ptr<BytecodeReader> reader);

		

	private :
		DxbcContainerHeader _header;
		vector<DxbcChunk> _chunks;
	};
};