#pragma once

#include "stdafx.h"
#include "DxbcChunk.h"

namespace SlimShader
{
	class DebuggingChunk : public DxbcChunk
	{
	public :
		static DebuggingChunk Parse(shared_ptr<BytecodeReader> reader);
	};
};