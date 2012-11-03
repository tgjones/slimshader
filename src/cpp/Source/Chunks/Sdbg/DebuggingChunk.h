#pragma once

#include "PCH.h"
#include "DxbcChunk.h"

namespace SlimShader
{
	/// <summary>
	/// TODO
	/// Contains the original source code of the shader, and links source code lines to ASM ops.
	/// </summary>
	class DebuggingChunk : public DxbcChunk
	{
	public :
		static std::shared_ptr<DebuggingChunk> Parse(BytecodeReader& reader);
	};
};