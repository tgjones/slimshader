using System;
using System.Runtime.InteropServices;
using SlimShader.Util;

namespace SlimShader.Chunks.Shex
{
	///// <summary>
	///// Represents four Numbers, or two doubles.
	///// </summary>
	//[StructLayout(LayoutKind.Explicit, Size = sizeof(byte) * 16)]
	//public struct Number4
	//{
	//	private readonly byte _byte0;
	//	private readonly byte _byte1;
	//	private readonly byte _byte2;
	//	private readonly byte _byte3;

	//	public static Number4 FromRawBytes(byte[] rawBytes)
	//	{

	//	}

	//	Number Numbers[4];
	//	double Doubles[2];
	//};

	/// <summary>
	/// Represents an int, float or uint.
	/// </summary>
	[StructLayout(LayoutKind.Explicit, Size = sizeof(byte) * 4 + sizeof(int))]
	public struct Number
	{
		[FieldOffset(0)]
		public byte Byte0;

		[FieldOffset(1)]
		public byte Byte1;

		[FieldOffset(2)]
		public byte Byte2;

		[FieldOffset(3)]
		public byte Byte3;

		[FieldOffset(0)]
		public int Int;

		[FieldOffset(0)]
		public uint UInt;

		[FieldOffset(0)]
		public float Float;

		[FieldOffset(4)]
		public NumberType Type;

		public byte[] RawBytes
		{
			get { return new[] { Byte0, Byte1, Byte2, Byte3 }; }
		}

		public Number(byte[] rawBytes, NumberType type)
			: this()
		{
			Type = type;
			Byte0 = rawBytes[0];
			Byte1 = rawBytes[1];
			Byte2 = rawBytes[2];
			Byte3 = rawBytes[3];
		}

		public Number(float value)
			: this()
		{
			Type = NumberType.Float;
			Float = value;
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
					if (Int > hexThreshold)
						return "0x" + Int.ToString("x8");
					return Int.ToString();
				case NumberType.UInt:
					if (UInt > negThreshold)
						return Int.ToString();
					if (UInt > hexThreshold)
						return "0x" + UInt.ToString("x8");
					return UInt.ToString();
				case NumberType.Float:
					if (RawBytes[0] == 0 && RawBytes[1] == 0 && RawBytes[2] == 0 && RawBytes[3] == 128)
						return "-0.000000"; // "Negative" zero
					return ((double) Float).ToString("F6");
				case NumberType.Unknown:
					// fxc.exe has some strange rules for formatting output of numbers of 
					// unknown type - for example, as operands to the mov op. It only matters for string output -
					// when actually executing these instructions that can have operands of unknown type, they simply
					// move bytes around without interpreting them - this is from the mov doc page:
					// "The modifiers, other than swizzle, assume the data is floating point. The absence of modifiers 
					// just moves data without altering bits."
					if (Int < floatThresholdNeg || Int > floatThresholdPos)
						goto case NumberType.Float;
					goto case NumberType.Int;
				default:
					throw new ArgumentException("Type '" + Type + "' is not supported.");
			}
		}
	}
}