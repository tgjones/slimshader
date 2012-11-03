#pragma once

#include "PCH.h"

namespace SlimShader
{
	/// <summary>
	/// Very similar to Shader.ResourceDimension, but this one is used within the RDEF (Resource Definition) chunk.
	/// The multi-sampled values are different from ResourceDimension.
	/// </summary>
	enum class ShaderResourceViewDimension
	{
		/// <summary>
		/// The type is unknown.
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// The resource is a buffer.
		/// </summary>
		Buffer = 1,

		/// <summary>
		/// The resource is a 1D texture.
		/// </summary>
		Texture1D = 2,

		/// <summary>
		/// The resource is an array of 1D textures.
		/// </summary>
		Texture1DArray = 3,

		/// <summary>
		/// The resource is a 2D texture.
		/// </summary>
		Texture2D = 4,

		/// <summary>
		/// The resource is an array of 2D textures.
		/// </summary>
		Texture2DArray = 5,

		/// <summary>
		/// The resource is a multisampling 2D texture.
		/// </summary>
		Texture2DMultiSampled = 6,

		/// <summary>
		/// The resource is an array of multisampling 2D textures.
		/// </summary>
		Texture2DMultiSampledArray = 7,

		/// <summary>
		/// The resource is a 3D texture.
		/// </summary>
		Texture3D = 8,

		/// <summary>
		/// The resource is a cube texture.
		/// </summary>
		TextureCube = 9,

		/// <summary>
		/// The resource is an array of cube textures.
		/// </summary>
		TextureCubeArray = 10,

		/// <summary>
		/// The resource is a raw buffer.
		/// </summary>
		ExtendedBuffer = 11
	};

	std::string ToString(ShaderResourceViewDimension value);
};