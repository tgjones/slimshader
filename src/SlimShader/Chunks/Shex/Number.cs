using System;
using SlimShader.Util;

namespace SlimShader.Chunks.Shex
{
	/// <summary>
	/// Represents an int, float, uint or double.
	/// </summary>
	public struct Number
	{
		public readonly byte[] RawBytes;
		public readonly NumberType Type;

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

		public double AsDouble
		{
			get { return BitConverter.ToDouble(RawBytes, 0); }
		}

		public Number(byte[] rawBytes, NumberType type)
		{
			RawBytes = rawBytes;
			Type = type;
		}

		public static Number Parse32(BytecodeReader reader, NumberType type)
		{
			return ParseInternal(reader, type, 4);
		}

		public static Number Parse64(BytecodeReader reader, NumberType type)
		{
			return ParseInternal(reader, type, 8);
		}

		private static Number ParseInternal(BytecodeReader reader, NumberType type, byte byteCount)
		{
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
				case NumberType.Double:
					return AsDouble.ToString("F6");
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