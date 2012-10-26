using SlimShader.Chunks.Common;
using SlimShader.Chunks.Shex;

namespace SlimShader.Chunks.Stat
{
	public static class EnumExtensions
	{
		public static string GetDescription(this TessellatorDomain value)
		{
			return value.ToString();
		}

		public static string GetDescription(this TessellatorPartitioning value)
		{
			return value.ToString();
		}

		public static string GetDescription(this TessellatorOutputPrimitive value)
		{
			return value.ToString();
		}
	}
}