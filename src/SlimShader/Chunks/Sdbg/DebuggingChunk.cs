using SlimShader.Util;

namespace SlimShader.Chunks.Sdbg
{
	/// <summary>
	/// TODO
	/// Contains the original source code of the shader, and links source code lines to ASM ops.
	/// </summary>
	public class DebuggingChunk : DxbcChunk
	{
		public static DebuggingChunk Parse(BytecodeReader reader)
		{
			return new DebuggingChunk();
		}
	}
}