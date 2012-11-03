#pragma once

#include "PCH.h"

namespace SlimShader
{
	/// <summary>
	/// These flags identify the shader-variable class.
	/// Based on D3D10_SHADER_VARIABLE_CLASS.
	/// </summary>
	enum class ShaderVariableClass
	{
		/// <summary>
		/// The shader variable is a scalar.
		/// </summary>
		Scalar,

		/// <summary>
		/// The shader variable is a vector.
		/// </summary>
		Vector,

		/// <summary>
		/// The shader variable is a row-major matrix.
		/// </summary>
		MatrixRows,

		/// <summary>
		/// The shader variable is a column-major matrix.
		/// </summary>
		MatrixColumns,

		/// <summary>
		/// The shader variable is an object.
		/// </summary>
		Object,

		/// <summary>
		/// The shader variable is a structure.
		/// </summary>
		Struct,

		/// <summary>
		/// The shader variable is a class.
		/// </summary>
		InterfaceClass,

		/// <summary>
		/// The shader variable is an interface.
		/// </summary>
		InterfacePointer
	};

	std::string ToString(ShaderVariableClass value);
};