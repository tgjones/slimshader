#pragma once

#include "PCH.h"
#include "DxbcChunk.h"

namespace SlimShader
{
	/// <summary>
	/// TODO: No idea, but this is present in ParticleDrawVS.asm, which is unique for including
	/// the enableRawAndStructuredBuffers global flag. I think this is because it includes both a normal
	/// Texture2D, and a StructuredBuffer.
	/// </summary>
	class Sfi0Chunk : public DxbcChunk
	{
	public :
		static std::shared_ptr<Sfi0Chunk> Parse(BytecodeReader& reader);
	};
};