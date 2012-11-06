#include "PCH.h"

namespace SlimShader
{
	enum class TessellatorPartitioning
	{
		Undefined = 0,

		Integer = 1,
		Pow2 = 2,
		FractionalOdd = 3,
		FractionalEven = 4
	};

	std::string ToStringShex(TessellatorPartitioning value);
	std::string ToStringStat(TessellatorPartitioning value);
};