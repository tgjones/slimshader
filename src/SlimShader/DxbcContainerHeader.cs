namespace SlimShader
{
	public class DxbcContainerHeader
	{
		public uint FourCc { get; internal set; }
		public uint[] UniqueKey { get; internal set; }
		public uint One { get; internal set; }
		public uint TotalSize { get; internal set; }
		public uint ChunkCount { get; internal set; }
	}
}