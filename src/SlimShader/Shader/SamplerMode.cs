using SlimShader.Util;

namespace SlimShader.Shader
{
	public enum SamplerMode
	{
		[Description("mode_default")]
		Default = 0,

		[Description("comparison")]
		Comparison = 1,

		[Description("mono")]
		Mono = 2
	}
}