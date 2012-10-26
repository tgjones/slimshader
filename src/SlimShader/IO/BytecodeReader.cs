using System.IO;
using System.Text;

namespace SlimShader.IO
{
	public class BytecodeReader
	{
		private readonly byte[] _buffer;
		private readonly int _offset;
		private readonly BinaryReader _reader;

		public bool EndOfBuffer
		{
			get { return _reader.BaseStream.Position >= _reader.BaseStream.Length; }
		}

		public long CurrentPosition
		{
			get { return _reader.BaseStream.Position; }
		}

		public BytecodeReader(byte[] buffer, int index, int count)
		{
			_buffer = buffer;
			_offset = index;
			_reader = new BinaryReader(new MemoryStream(buffer, index, count));
		}

		public byte ReadByte()
		{
			return _reader.ReadByte();
		}

		public float ReadSingle()
		{
			return _reader.ReadSingle();
		}

		public double ReadDouble()
		{
			return _reader.ReadDouble();
		}

		public int ReadInt32()
		{
			return _reader.ReadInt32();
		}

		public ushort ReadUInt16()
		{
			return _reader.ReadUInt16();
		}

		public uint ReadUInt32()
		{
			return _reader.ReadUInt32();
		}

		public ulong ReadUInt64()
		{
			return _reader.ReadUInt64();
		}

		public string ReadString()
		{
			var nextCharacter = _reader.ReadChar();
			var sb = new StringBuilder();
			while (nextCharacter != 0)
			{
				sb.Append(nextCharacter);
				nextCharacter = _reader.ReadChar();
			}
			return sb.ToString();
		}

		public BytecodeReader CopyAtCurrentPosition(int? count = null)
		{
			return CopyAtOffset((int) _reader.BaseStream.Position, count);
		}

		public BytecodeReader CopyAtOffset(int offset, int? count = null)
		{
			count = count ?? (int) (_reader.BaseStream.Length - offset);
			return new BytecodeReader(_buffer, _offset + offset, count.Value);
		}
	}
}