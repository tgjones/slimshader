using SlimShader.Util;

namespace SlimShader.InputOutputSignature
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