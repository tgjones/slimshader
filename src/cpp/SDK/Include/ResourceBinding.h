#pragma once

#include "PCH.h"
#include "BytecodeReader.h"
#include "ResourceReturnType.h"
#include "ShaderInputFlags.h"
#include "ShaderInputType.h"
#include "ShaderResourceViewDimension.h"

namespace SlimShader
{
	/// <summary>
	/// Roughly corresponds to the D3D11_SHADER_INPUT_BIND_DESC structure.
	/// </summary>
	class ResourceBinding
	{
	public :
		static ResourceBinding Parse(const BytecodeReader& reader, BytecodeReader& resourceBindingReader);

		/// <summary>
		/// Name of the shader resource.
		/// </summary>
		const std::string& GetName() const;

		/// <summary>
		/// Identifies the type of data in the resource.
		/// </summary>
		ShaderInputType GetType() const;

		/// <summary>
		/// Starting bind point.
		/// </summary>
		uint32_t GetBindPoint() const;

		/// <summary>
		/// Number of contiguous bind points for arrays.
		/// </summary>
		uint32_t GetBindCount() const;

		/// <summary>
		/// Shader input-parameter options.
		/// </summary>
		ShaderInputFlags GetFlags() const;

		/// <summary>
		/// Identifies the dimensions of the bound resource.
		/// </summary>
		ShaderResourceViewDimension GetDimension() const;

		/// <summary>
		/// If the input is a texture, the return type.
		/// </summary>
		ResourceReturnType GetReturnType() const;

		/// <summary>
		/// The number of samples for a multisampled texture; otherwise 0.
		/// </summary>
		uint32_t GetNumSamples();

		friend std::ostream& operator<<(std::ostream& out, const ResourceBinding& value);

	private :
		ResourceBinding() { }

		std::string _name;
		ShaderInputType _type;
		uint32_t _bindPoint;
		uint32_t _bindCount;
		ShaderInputFlags _flags;
		ShaderResourceViewDimension _dimension;
		ResourceReturnType _returnType;
		uint32_t _numSamples;
	};
};