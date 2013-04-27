namespace SlimShader.Chunks.Xsgn
{
	public enum RegisterComponentType
	{
		Unknown = 0,

		[Description("uint")]
		UInt32 = 1,

		[Description("int")]
		SInt32 = 2,

		[Description("float")]
		Float32 = 3
	}
}