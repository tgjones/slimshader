namespace SlimShader.Chunks.Common
{
	public enum TessellatorPartitioning
	{
		Undefined = 0,

		[Description("partitioning_integer", ChunkType.Shex)]
		Integer = 1,

		[Description("partitioning_pow2", ChunkType.Shex)]
		Pow2 = 2,

		[Description("partitioning_fractional_odd", ChunkType.Shex)]
		FractionalOdd = 3,

		[Description("partitioning_fractional_even", ChunkType.Shex)]
		FractionalEven = 4
	}
}