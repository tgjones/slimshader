#pragma once

#include "PCH.h"
#include "DxbcChunk.h"
#include "ProgramType.h"

namespace SlimShader
{
	class InputOutputSignatureChunk : public DxbcChunk
	{
	public :
		static InputOutputSignatureChunk Parse(shared_ptr<BytecodeReader> reader, ChunkType chunkType,
			ProgramType programType);
	};
};