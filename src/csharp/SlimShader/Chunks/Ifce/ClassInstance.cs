using SlimShader.Util;
using System.Diagnostics;

namespace SlimShader.Chunks.Ifce
{
	public class ClassInstance
	{
		public string Name { get; private set; }
		public ushort Type { get; private set; }
		public ushort ConstantBuffer { get; private set; }
		public ushort ConstantBufferOffset { get; private set; }
		public ushort Texture { get; private set; }
		public ushort Sampler { get; private set; }

		public static ClassInstance Parse(BytecodeReader reader, BytecodeReader classInstanceReader)
		{
			var nameOffset = classInstanceReader.ReadUInt32();
			var nameReader = reader.CopyAtOffset((int) nameOffset);
			var name = nameReader.ReadString();

			var type = classInstanceReader.ReadUInt16();
			var unknown = classInstanceReader.ReadUInt16();
			Debug.Assert(unknown == 1); // Unknown, perhaps the class instance type?

			return new ClassInstance
			{
				Name = name,
				Type = type,
				ConstantBuffer = classInstanceReader.ReadUInt16(),
				ConstantBufferOffset = classInstanceReader.ReadUInt16(),
				Texture = classInstanceReader.ReadUInt16(),
				Sampler = classInstanceReader.ReadUInt16()
			};
		}

		public override string ToString()
		{
			// For example:
			// Name                        Type CB CB Offset Texture Sampler
			// --------------------------- ---- -- --------- ------- -------
			// g_ambientLight                12  0         0       -       -

			return string.Format("{0,-27} {1,4} {2,2} {3,9} {4,7} {5,7}",
				Name, Type, ConstantBuffer, ConstantBufferOffset,
				(Texture == 0xFFFF) ? "-" : Texture.ToString(),
				(Sampler == 0xFFFF) ? "-" : Sampler.ToString());
		}
	}
}