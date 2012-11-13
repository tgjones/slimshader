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
		[Description("Odd Fractional", ChunkType.Stat)]
		FractionalOdd = 3,

		[Description("partitioning_fractional_even", ChunkType.Shex)]
		[Description("Even Fractional", ChunkType.Stat)]
		FractionalEven = 4
	}
}