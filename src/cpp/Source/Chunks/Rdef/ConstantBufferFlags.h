#pragma once

#include "PCH.h"

namespace SlimShader
{
	/// <summary>
	/// Values that identify the indended use of a constant-data buffer.
	/// Based on D3D_SHADER_CBUFFER_FLAGS.
	/// </summary>
	enum class ConstantBufferFlags
	{
		None = 0,

		/// <summary>
		/// Bind the constant buffer to an input slot defined in HLSL code (instead of letting the compiler choose the 
		/// input slot).
		/// </summary>
		UserPacked = 1
	};
};