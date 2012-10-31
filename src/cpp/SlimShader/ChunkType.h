#pragma once

#include "stdafx.h"

namespace SlimShader
{
	enum class ChunkType
	{
		Unknown,

		/// <summary>
		/// Interfaces
		/// </summary>
		Ifce = 'IFCE',

		/// <summary>
		/// Input signature
		/// </summary>
		Isgn,

		/// <summary>
		/// Output signature (SM5)
		/// </summary>
		Osg5,

		/// <summary>
		/// Output signature
		/// </summary>
		Osgn,

		/// <summary>
		/// Patch constant signature
		/// </summary>
		Pcsg,

		/// <summary>
		/// Resource definition
		/// </summary>
		Rdef,

		/// <summary>
		/// Shader debugging info
		/// </summary>
		Sdbg,

		/// <summary>
		/// ?
		/// </summary>
		Sfi0,

		/// <summary>
		/// Shader (SM 4.0)
		/// </summary>
		Shdr,

		/// <summary>
		/// Shader (SM 5.0)
		/// </summary>
		Shex,

		/// <summary>
		/// Statistics
		/// </summary>
		Stat
	};
};