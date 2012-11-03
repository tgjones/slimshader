#pragma once

#include "PCH.h"
#include "DxbcChunk.h"

namespace SlimShader
{
	class ShaderProgramChunk : public DxbcChunk
	{
	public :
		static std::shared_ptr<ShaderProgramChunk> Parse(BytecodeReader& reader);
	};
};