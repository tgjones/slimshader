using SlimShader.Util;

namespace SlimShader
{
	public class BytecodeContainerHeader
	{
		public uint FourCc { get; private set; }
		public uint[] UniqueKey { get; private set; }
		public uint One { get; private set; }
		public uint TotalSize { get; private set; }
		public uint ChunkCount { get; private set; }

		public static BytecodeContainerHeader Parse(BytecodeReader reader)
		{
			var fourCc = reader.ReadUInt32();
			if (fourCc != "DXBC".ToFourCc())
				throw new ParseException("Invalid FourCC");

			var uniqueKey = new uint[4];
			uniqueKey[0] = reader.ReadUInt32();
			uniqueKey[1] = reader.ReadUInt32();
			uniqueKey[2] = reader.ReadUInt32();
			uniqueKey[3] = reader.ReadUInt32();

			return new BytecodeContainerHeader
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