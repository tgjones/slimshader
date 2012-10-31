#pragma once

#include "PCH.h"
#include "DxbcChunk.h"
#include "ShaderVersion.h"

namespace SlimShader
{
	/// <summary>
	/// Most of this was adapted from 
	/// https://devel.nuclex.org/framework/browser/graphics/Nuclex.Graphics.Native/trunk/Source/Introspection/HlslShaderReflector.cpp?rev=1743
	/// Roughly corresponds to the D3D11_SHADER_DESC structure.
	/// </summary>
	class ResourceDefinitionChunk : public DxbcChunk
	{
	public :
		static ResourceDefinitionChunk Parse(shared_ptr<BytecodeReader> reader);

		shared_ptr<ShaderVersion> GetTarget();
	};
};