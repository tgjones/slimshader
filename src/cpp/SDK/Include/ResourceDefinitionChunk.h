#pragma once

#include "PCH.h"
#include "ConstantBuffer.h"
#include "DxbcChunk.h"
#include "ResourceBinding.h"
#include "ShaderFlags.h"
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
		static std::shared_ptr<ResourceDefinitionChunk> Parse(BytecodeReader& reader);

		const std::vector<ConstantBuffer>& GetConstantBuffers();
		const std::vector<ResourceBinding>& GetResourceBindings();
		const ShaderVersion& GetTarget() const;
		ShaderFlags GetFlags() const;
		const std::string& GetCreator() const;

		friend std::ostream& operator<<(std::ostream& out, const ResourceDefinitionChunk& value);

	private:
		ResourceDefinitionChunk(ShaderVersion target);

		std::vector<ConstantBuffer> _constantBuffers;
		std::vector<ResourceBinding> _resourceBindings;
		const ShaderVersion _target;
		ShaderFlags _flags;
		std::string _creator;
	};
};