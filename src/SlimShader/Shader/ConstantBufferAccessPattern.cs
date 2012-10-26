using SlimShader.Util;

namespace SlimShader.Shader
{
	public enum ConstantBufferAccessPattern
	{
		[Description("immediateIndexed")]
		ImmediateIndexed = 0,

		[Description("dynamicIndexed")]
		DynamicIndexed = 1
	}
}