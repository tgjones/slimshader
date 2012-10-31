#pragma once

#include "PCH.h"
#include "DxbcChunk.h"

namespace SlimShader
{
	class ShaderProgramChunk : public DxbcChunk
	{
	public :
		static ShaderProgramChunk Parse(shared_ptr<BytecodeReader> reader);
	};
};