#pragma once

#include "PCH.h"

namespace SlimShader
{
	enum class ShaderMessageFormat
	{
		/// <summary>
		/// No formatting, just a text string.  Operands are ignored.
		/// </summary>
		AnsiText,

		/// <summary>
		/// Format string follows C/C++ printf conventions.
		/// </summary>
		AnsiPrintf
	};
};