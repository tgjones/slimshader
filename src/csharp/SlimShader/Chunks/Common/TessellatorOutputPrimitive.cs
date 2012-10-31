namespace SlimShader.Chunks.Common
{
	public enum TessellatorOutputPrimitive
	{
		Undefined = 0,

		[Description("output_point", ChunkType.Shex)]
		Point = 1,

		[Description("output_line", ChunkType.Shex)]
		Line = 2,

		[Description("output_triangle_cw", ChunkType.Shex)]
		[Description("Clockwise Triangles", ChunkType.Stat)]
		TriangleCw = 3,

		TriangleCcw = 4
	}
}