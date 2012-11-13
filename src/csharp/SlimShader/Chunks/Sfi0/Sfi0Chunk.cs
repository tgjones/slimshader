using System.Diagnostics;
using System.Text;
using SlimShader.Util;

namespace SlimShader.Chunks.Sfi0
{
	/// <summary>
	/// TODO: No idea, but this is present in ParticleDrawVS.asm, which is unique for including
	/// the enableRawAndStructuredBuffers global flag. I think this is because it includes both a normal
	/// Texture2D, and a StructuredBuffer.
	/// 
	/// When shader includes enableDoublePrecisionFloatOps global flag, then value is 1.
	/// </summary>
	public class Sfi0Chunk : BytecodeChunk
	{
		public bool RequiresDoublePrecisionFloatingPoint { get; private set; }

		public static Sfi0Chunk Parse(BytecodeReader reader)
		{
			var flags = reader.ReadInt32();
			Debug.Assert(flags == 1 || flags == 2); // TODO: Unknown

			var result = new Sfi0Chunk();

			if (flags == 1)
				result.RequiresDoublePrecisionFloatingPoint = true;
			
			return result;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			if (RequiresDoublePrecisionFloatingPoint)
			{
				sb.AppendLine("// Note: shader requires additional functionality:");
				sb.AppendLine("//       Double-precision floating point");
				sb.AppendLine("//");
				sb.AppendLine("//");
			}
			return sb.ToString();
		}
	}
}