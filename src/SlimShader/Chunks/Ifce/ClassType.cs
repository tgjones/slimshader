using SlimShader.Util;

namespace SlimShader.Chunks.Ifce
{
	public class ClassType
	{
		public string Name { get; set; }
		public uint ID { get; set; }
		public uint ConstantBufferStride { get; set; }
		public uint Texture { get; set; }
		public uint Sampler { get; set; }

		public static ClassType Parse(BytecodeReader reader, BytecodeReader classTypeReader)
		{
			var nameOffset = classTypeReader.ReadUInt32();
			var nameReader = reader.CopyAtOffset((int) nameOffset);

			return new ClassType
			{
				Name = nameReader.ReadString(),
				ID = classTypeReader.ReadUInt16(),
				ConstantBufferStride = classTypeReader.ReadUInt16(),
				Texture = classTypeReader.ReadUInt16(),
				Sampler = classTypeReader.ReadUInt16()
			};
		}

		public override string ToString()
		{
			// For example:
			// Name                             ID CB Stride Texture Sampler");
			// ------------------------------ ---- --------- ------- -------");
			// cUnchangedColour                  0         0       0       0");
			return string.Format("{0,-30} {1,4} {2,9} {3,7} {4,7}", Name, ID, ConstantBufferStride, Texture, Sampler);
		}
	}
}