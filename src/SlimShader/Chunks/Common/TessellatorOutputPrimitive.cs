using SlimShader.Util;

namespace SlimShader.Chunks.Common
{
	public enum TessellatorOutputPrimitive
	{
		Undefined = 0,

		[Description("output_point")]
		Point = 1,

		[Description("output_line")]
		Line = 2,

		TriangleCw = 3,
		TriangleCcw = 4
	}
}