using System;
using SlimShader.Util;

namespace SlimShader.Chunks.Shex
{
	/// <summary>
	/// Represents an int, float or uint.
	/// </summary>
	public struct Number
	{
		private readonly NumberType _type;
		private readonly byte _byte0;
		private readonly byte _byte1;
		private readonly byte _byte2;
		private readonly byte _byte3;

		public NumberType Type
		{
			get { return _type; }
		}

		public byte[] RawBytes
		{
			get { return new[] { _byte0, _byte1, _byte2, _byte3 }; }
		}

		public int AsInt
		{
			get { return BitConverter.ToInt32(RawBytes, 0); }
		}

		public uint AsUInt
		{
			get { return BitConverter.ToUInt32(RawBytes, 0); }
		}

		public float AsFloat
		{
			get { return BitConverter.ToSingle(RawBytes, 0); }
		}

		public Number(byte[] rawBytes, NumberType type)
		{
			_byte0 = rawBytes[0];
			_byte1 = rawBytes[1];
			_byte2 = rawBytes[2];
			_byte3 = rawBytes[3];
			_type = type;
		}

		public Number(float value)
			: this(BitConverter.GetBytes(value), NumberType.Float)
		{

		}

		public static Number Parse(BytecodeReader reader, NumberType type)
		{
			const int byteCount = 4;
			var bytes = new byte[byteCount];
			for (int i = 0; i < byteCount; i++)
				bytes[i] = reader.ReadByte();
			return new Number(bytes, type);
		}

		public override string ToString()
		{
			const int hexThreshold = 10000; // This is the correct value, derived through fxc.exe and a bisect-search.
			const uint negThreshold = 0xFFFFFFF0; // TODO: Work out the actual negative threshold.
			const int floatThresholdPos = 0x00700000; // TODO: Work out the actual float threshold.
			const int floatThresholdNeg = -0x00700000; // TODO: Work out the actual float threshold.
			switch (Type)
			{
				case NumberType.Int:
					if (AsInt > hexThreshold)
						return "0x" + AsInt.ToString("x8");
					return AsInt.ToString();
				case NumberType.UInt:
					if (AsUInt > negThreshold)
						return AsInt.ToString();
					if (AsUInt > hexThreshold)
						return "0x" + AsUInt.ToString("x8");
					return AsUInt.ToString();
				case NumberType.Float:
					if (RawBytes[0] == 0 && RawBytes[1] == 0 && RawBytes[2] == 0 && RawBytes[3] == 128)
						return "-0.000000"; // "Negative" zero
					return ((double) AsFloat).ToString("F6");
				case NumberType.Unknown:
					// fxc.exe has some strange rules for formatting output of numbers of 
					// unknown type - for example, as operands to the mov op. It only matters for string output -
					// when actually executing these instructions that can have operands of unknown type, they simply
					// move bytes around without interpreting them - this is from the mov doc page:
					// "The modifiers, other than swizzle, assume the data is floating point. The absence of modifiers 
					// just moves data without altering bits."
					if (AsInt < floatThresholdNeg || AsInt > floatThresholdPos)
						goto case NumberType.Float;
					goto case NumberType.Int;
				default:
					throw new ArgumentException("Type '" + Type + "' is not supported.");
			}
		}
	}
}