using System.Diagnostics;
using SlimShader.Util;

namespace SlimShader.Chunks.Sdbg
{
	/// <summary>
	/// TODO
	/// Contains the original source code of the shader, and links source code lines to ASM ops.
	/// </summary>
	public class DebuggingChunk : DxbcChunk
	{
		public string OriginalFileName { get; private set; }
		public string OriginalSourceCode { get; private set; }

		public static DebuggingChunk Parse(BytecodeReader reader)
		{
			var result = new DebuggingChunk();

			var chunkReader = reader.CopyAtCurrentPosition();

			for (int i = 0; i < 22; i++)
			{
				var unknown = chunkReader.ReadUInt32();
			}

			var originalFileNameLength = chunkReader.ReadUInt32();
			{
				var unknown = chunkReader.ReadUInt32();
				Debug.Assert(originalFileNameLength == unknown);
			}
			var originalSourceCodeLength = chunkReader.ReadUInt32();

			var originalFileLengthReader = reader.CopyAtOffset(4136);
			var originalFileLength = originalFileLengthReader.ReadUInt32();
			var originalFileReader = reader.CopyAtOffset(5204, (int) originalFileLength);
			var originalFile = originalFileReader.ReadString();

			result.OriginalFileName = originalFile.Substring(0, (int) originalFileNameLength);
			result.OriginalSourceCode = originalFile.Substring((int) originalFileNameLength);
			Debug.Assert(result.OriginalSourceCode.Length == originalSourceCodeLength);

			return result;
		}
	}
}