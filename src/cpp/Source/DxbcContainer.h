#pragma once

#include "PCH.h"
#include "BytecodeReader.h"
#include "DxbcContainerHeader.h"

using namespace std;

namespace SlimShader
{
	class DxbcChunk;
	class ResourceDefinitionChunk;

	class DxbcContainer
	{
	public :
		static DxbcContainer Parse(const uint8_t bytes[], const int length);
		static DxbcContainer Parse(const shared_ptr<BytecodeReader> reader);

		shared_ptr<ResourceDefinitionChunk> GetResourceDefinition();

	private :
		DxbcContainerHeader _header;
		vector<DxbcChunk> _chunks;
	};
};