#pragma once

#include "PCH.h"

namespace SlimShader
{
	/// <summary>
	/// Indicates a constant buffer's type.
	/// Based on D3D11_CBUFFER_TYPE.
	/// </summary>
	enum class ConstantBufferType
	{
		/// <summary>
		/// A buffer containing scalar constants.
		/// </summary>
		ConstantBuffer,

		/// <summary>
		/// A buffer containing texture data.
		/// </summary>
		TextureBuffer,

		/// <summary>
		/// A buffer containing interface pointers.
		/// </summary>
		InterfacePointers,

		/// <summary>
		/// A buffer containing binding information.
		/// </summary>
		ResourceBindInformation
	};

	std::string ToString(ConstantBufferType value);
};