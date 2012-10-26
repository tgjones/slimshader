using SlimShader.Util;

namespace SlimShader.Chunks.Shex
{
	public enum TessellatorPartitioning
	{
		Undefined = 0,

		[Description("partitioning_integer")]
		Integer = 1,

		[Description("partitioning_pow2")]
		Pow2 = 2,

		[Description("partitioning_fractional_odd")]
		FractionalOdd = 3,

		[Description("partitioning_fractional_even")]
		FractionalEven = 4
	}
}