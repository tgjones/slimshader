using System;
using System.Runtime.InteropServices;
using SlimShader.Chunks.Shex;
using SlimShader.Util;

namespace SlimShader
{
	/// <summary>
	/// Represents an int, float or uint.
	/// </summary>
	[StructLayout(LayoutKind.Explicit, Size = SizeInBytes)]
	public struct Number
	{
		public static Number Abs(Number value, NumberType type)
		{
            switch (type)
			{
				case NumberType.Float:
					return FromFloat(Math.Abs(value.Float));
				default:
                    throw new InvalidOperationException(string.Format("Abs is not a valid operation for number type '{0}'.", type));
			}
		}

		public static Number Negate(Number value, NumberType type)
		{
            switch (type)
			{
				case NumberType.Int:
					return FromInt(-value.Int);
				case NumberType.Float:
					return FromFloat(-value.Float);
				default:
                    throw new InvalidOperationException(string.Format("Negate is not a valid operation for number type '{0}'.", type));
			}
		}

		private static float Saturate(float value)
		{
			return Math.Min(1.0f, Math.Max(0.0f, value));
		}

        public static Number FromByteArray(byte[] bytes, int startIndex)
        {
			if (startIndex >= bytes.Length)
				return new Number();

            return new Number
            {
                Byte0 = bytes[startIndex + 0],
                Byte1 = bytes[startIndex + 1],
                Byte2 = bytes[startIndex + 2],
                Byte3 = bytes[startIndex + 3]
            };
        }

		public static Number FromFloat(float value, bool saturate)
		{
			return FromFloat((saturate) ? Saturate(value) : value);
		}

		public static Number FromFloat(float value)
		{
			return new Number
			{
				Float = value
			};
		}

		public static Number FromInt(int value)
		{
			return new Number
			{
				Int = value
			};
		}

		public static Number FromUInt(uint value)
		{
			return new Number
			{
				UInt = value
			};
		}

		public static Number Parse(BytecodeReader reader)
		{
			const int byteCount = 4;
			var bytes = new byte[byteCount];
			for (int i = 0; i < byteCount; i++)
				bytes[i] = reader.ReadByte();
			return new Number(bytes);
		}

		public const int SizeInBytes = sizeof(byte) * 4;

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

		public byte[] RawBytes
		{
			get { return new[] { Byte0, Byte1, Byte2, Byte3 }; }
		    set
		    {
		        Byte0 = value[0];
		        Byte1 = value[1];
		        Byte2 = value[2];
		        Byte3 = value[3];
		    }
		}

		public Number(byte[] rawBytes)
			: this()
		{
			Byte0 = rawBytes[0];
			Byte1 = rawBytes[1];
			Byte2 = rawBytes[2];
			Byte3 = rawBytes[3];
		}

        public override string ToString()
        {
            return ToString(NumberType.Unknown);
        }

		public string ToString(NumberType type)
		{
			const int hexThreshold = 10000; // This is the correct value, derived through fxc.exe and a bisect-search.
			const uint negThreshold = 0xFFFFFFF0; // TODO: Work out the actual negative threshold.
			const int floatThresholdPos = 0x00700000; // TODO: Work out the actual float threshold.
			const int floatThresholdNeg = -0x00700000; // TODO: Work out the actual float threshold.
			const uint uintThresholdPos = 0xfff00000; // TODO: Work out the actual float threshold.
            switch (type)
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
					if (Math.Abs(Float) > 10000000000000000.0) // TODO: Threshold is guessed
						return DoubleConverter.ToExactString(Float);
					var result = ((double) Float).ToString("F6");
					if (!result.StartsWith("-") && Float < 0.0f)
						result = "-" + result;
					return result;
				case NumberType.Unknown:
					// fxc.exe has some strange rules for formatting output of numbers of 
					// unknown type - for example, as operands to the mov op. It only matters for string output -
					// when actually executing these instructions that can have operands of unknown type, they simply
					// move bytes around without interpreting them - this is from the mov doc page:
					// "The modifiers, other than swizzle, assume the data is floating point. The absence of modifiers 
					// just moves data without altering bits."
					if (UInt > uintThresholdPos)
						goto case NumberType.UInt;
					if (Int < floatThresholdNeg || Int > floatThresholdPos)
						goto case NumberType.Float;
					goto case NumberType.Int;
				default:
					throw new InvalidOperationException(string.Format("Type '{0}' is not supported.", type));
			}
		}
	}
}