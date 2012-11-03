#pragma once

#include "PCH.h"

namespace SlimShader
{
	enum class ChunkType
	{
		Unknown,

		/// <summary>
		/// Interfaces
		/// </summary>
		Ifce = 'ECFI',

		/// <summary>
		/// Input signature
		/// </summary>
		Isgn = 'NGSI',

		/// <summary>
		/// Output signature (SM5)
		/// </summary>
		Osg5 = '5GSO',

		/// <summary>
		/// Output signature
		/// </summary>
		Osgn = 'NGSO',

		/// <summary>
		/// Patch constant signature
		/// </summary>
		Pcsg = 'GSCP',

		/// <summary>
		/// Resource definition
		/// </summary>
		Rdef = 'FEDR',

		/// <summary>
		/// Shader debugging info
		/// </summary>
		Sdbg = 'GBDS',

		/// <summary>
		/// ?
		/// </summary>
		Sfi0 = '0IFS',

		/// <summary>
		/// Shader (SM 4.0)
		/// </summary>
		Shdr = 'RDHS',

		/// <summary>
		/// Shader (SM 5.0)
		/// </summary>
		Shex = 'XEHS',

		/// <summary>
		/// Statistics
		/// </summary>
		Stat = 'TATS'
	};
};