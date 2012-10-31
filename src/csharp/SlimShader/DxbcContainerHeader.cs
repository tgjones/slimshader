using SlimShader.Util;

namespace SlimShader
{
	public class DxbcContainerHeader
	{
		public uint FourCc { get; internal set; }
		public uint[] UniqueKey { get; internal set; }
		public uint One { get; internal set; }
		public uint TotalSize { get; internal set; }
		public uint ChunkCount { get; internal set; }

		public static DxbcContainerHeader Parse(BytecodeReader reader)
		{
			uint fourCc = reader.ReadUInt32();
			if (fourCc != "DXBC".ToFourCc())
				throw new ParseException("Invalid FourCC");

			var uniqueKey = new uint[4];
			uniqueKey[0] = reader.ReadUInt32();
			uniqueKey[1] = reader.ReadUInt32();
			uniqueKey[2] = reader.ReadUInt32();
			uniqueKey[3] = reader.ReadUInt32();

			return new DxbcContainerHeader
			{
				FourCc = fourCc,
				UniqueKey = uniqueKey,
				One = reader.ReadUInt32(),
				TotalSize = reader.ReadUInt32(),
				ChunkCount = reader.ReadUInt32()
			};
		}
	}
}