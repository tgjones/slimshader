#include "PCH.h"

namespace SlimShader
{
	enum class PrimitiveTopology
	{
		Undefined = 0,

		PointList = 1,
		LineList = 2,
		LineStrip = 3,
		TriangleList = 4,
		TriangleStrip = 5,

		// 6 is reserved for legacy triangle fans
		// Adjacency values should be equal to (0x8 & non-adjacency):

		LineListAdj = 10,
		LineStripAdj = 11,
		TriangleListAdj = 12,
		TriangleStripAdj = 13,
	};
};