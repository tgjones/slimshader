namespace SlimShader.Chunks.Shex
{
	public class OpcodeHeader
	{
		public OpcodeType OpcodeType { get; set; }
		public uint Length { get; set; }
		public bool IsExtended { get; set; }
	}
}