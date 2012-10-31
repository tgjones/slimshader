namespace SlimShader.Chunks.Common
{
	public enum PrimitiveTopology
	{
		Undefined = 0,

		[Description("pointlist", ChunkType.Shex)]
		PointList = 1,

		[Description("linelist", ChunkType.Shex)]
		LineList = 2,

		[Description("linestrip", ChunkType.Shex)]
		LineStrip = 3,

		[Description("trianglelist", ChunkType.Shex)]
		TriangleList = 4,

		[Description("trianglestrip", ChunkType.Shex)]
		TriangleStrip = 5,

		// 6 is reserved for legacy triangle fans
		// Adjacency values should be equal to (0x8 & non-adjacency):

		LineListAdj = 10,
		LineStripAdj = 11,
		TriangleListAdj = 12,
		TriangleStripAdj = 13,
	}
}