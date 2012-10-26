using SlimShader.Util;

namespace SlimShader.Chunks.Shex
{
	public enum PrimitiveTopology
	{
		Undefined = 0,

		[Description("pointlist")]
		PointList = 1,

		[Description("linelist")]
		LineList = 2,

		[Description("linestrip")]
		LineStrip = 3,

		[Description("trianglelist")]
		TriangleList = 4,

		[Description("trianglestrip")]
		TriangleStrip = 5,

		// 6 is reserved for legacy triangle fans
		// Adjacency values should be equal to (0x8 & non-adjacency):

		LineListAdj = 10,
		LineStripAdj = 11,
		TriangleListAdj = 12,
		TriangleStripAdj = 13,
	}
}