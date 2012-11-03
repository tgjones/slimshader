using System.Diagnostics;
using SlimShader.Util;

namespace SlimShader.Chunks.Sfi0
{
	/// <summary>
	/// TODO: No idea, but this is present in ParticleDrawVS.asm, which is unique for including
	/// the enableRawAndStructuredBuffers global flag. I think this is because it includes both a normal
	/// Texture2D, and a StructuredBuffer.
	/// </summary>
	public class Sfi0Chunk : DxbcChunk
	{
		public static Sfi0Chunk Parse(BytecodeReader reader)
		{
			Debug.Assert(reader.ReadInt32() == 2); // TODO: Unknown
			return new Sfi0Chunk();
		}
	}
}