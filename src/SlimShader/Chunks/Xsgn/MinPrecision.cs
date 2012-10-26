namespace SlimShader.Chunks.Xsgn
{
	public enum MinPrecision
	{
		Default = 0,
		Float16 = 1,
		Float2_8 = 2,
		Reserved = 3,
		SInt16 = 4,
		UInt16 = 5,
		Any16 = 0xf0,
		Any10 = 0xf1
	}
}