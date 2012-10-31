#pragma once

#include "PCH.h"
#include "DxbcChunk.h"

namespace SlimShader
{
	class DebuggingChunk : public DxbcChunk
	{
	public :
		static DebuggingChunk Parse(shared_ptr<BytecodeReader> reader);
	};
};