namespace SlimShader.Chunks.Rdef
{
	public enum ShaderInputType
	{
		[Description("cbuffer")]
		CBuffer = 0,

		TBuffer = 1,

		[Description("texture")]
		Texture = 2,

		[Description("sampler")]
		Sampler = 3,

		[Description("UAV")]
		UavRwTyped = 4,

		[Description("texture")]
		Structured = 5,

		[Description("UAV")]
		UavRwStructured = 6,

		[Description("texture")]
		ByteAddress = 7,

		[Description("UAV")]
		UavRwByteAddress = 8,

		[Description("UAV")]
		UavAppendStructured = 9,

		[Description("UAV")]
		UavConsumeStructured = 10,

		[Description("UAV")]
		UavRwStructuredWithCounter = 11
	}
}