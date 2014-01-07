using System;
using System.Linq;
using System.Runtime.InteropServices;
using SlimShader.Chunks.Common;
using SlimShader.Chunks.Shex;

namespace SlimShader
{
	/// <summary>
	/// Represents four Numbers, or two doubles.
	/// </summary>
	[StructLayout(LayoutKind.Explicit, Size = Number.SizeInBytes * 4)]
	public struct Number4
	{
        public static Number4 FromByteArray(byte[] bytes, int startIndex)
        {
            return new Number4
            {
                Number0 = Number.FromByteArray(bytes, startIndex + 0),
                Number1 = Number.FromByteArray(bytes, startIndex + 4),
                Number2 = Number.FromByteArray(bytes, startIndex + 8),
                Number3 = Number.FromByteArray(bytes, startIndex + 12),
            };
        }

		public static Number4 Abs(Number4 original, NumberType type)
		{
            switch (type)
			{
                case NumberType.Float:
                case NumberType.Int:
                case NumberType.UInt:
					return new Number4(
						Number.Abs(original.Number0, type),
                        Number.Abs(original.Number1, type),
                        Number.Abs(original.Number2, type),
                        Number.Abs(original.Number3, type));
				case NumberType.Double:
					return new Number4(
						Math.Abs(original.Double0),
						Math.Abs(original.Double1));
				default:
                    throw new InvalidOperationException(string.Format("Abs is not a valid operation for number type '{0}'.", type));
			}
		}

        public static Number4 Average(ref Number4 v0, ref Number4 v1, ref Number4 v2, ref Number4 v3)
        {
            return new Number4(
                (v0.X + v1.X + v2.X + v3.X) / 4.0f,
                (v0.Y + v1.Y + v2.Y + v3.Y) / 4.0f,
                (v0.Z + v1.Z + v2.Z + v3.Z) / 4.0f,
                (v0.W + v1.W + v2.W + v3.W) / 4.0f);
        }

        public static Number4 Invert(Number4 value)
        {
            return new Number4(1 - value.R, 1 - value.G, 1 - value.B, 1 - value.A);
        }

        public static Number4 Invert(ref Number4 value)
        {
            return new Number4(1 - value.R, 1 - value.G, 1 - value.B, 1 - value.A);
        }

        public static Number4 Lerp(ref Number4 left, ref Number4 right, float value)
        {
            return new Number4(
                left.X * (1 - value) + right.X * value,
                left.Y * (1 - value) + right.Y * value,
                left.Z * (1 - value) + right.Z * value,
                left.W * (1 - value) + right.W * value);
        }

        public static Number4 Multiply(ref Number4 left, ref Number4 right)
        {
            return new Number4(
                left.X * right.X,
                left.Y * right.Y,
                left.Z * right.Z,
                left.W * right.W);
        }

		public static Number4 Negate(Number4 original, NumberType type)
		{
            switch (type)
			{
				case NumberType.Float:
			        return NegateFloat(original);
                case NumberType.Int:
			        return NegateInt(original);
				case NumberType.Double:
                    return NegateDouble(original);
				default:
                    throw new InvalidOperationException(string.Format("Negate is not a valid operation for number type '{0}'.", type));
			}
		}

        public static Number4 NegateDouble(Number4 original)
        {
            return new Number4(-original.Double0, -original.Double1);
        }

        public static Number4 NegateFloat(Number4 original)
        {
            return new Number4(-original.Float0, -original.Float1, -original.Float2, -original.Float3);
        }

        public static Number4 NegateInt(Number4 original)
        {
            return new Number4(-original.Int0, -original.Int1, -original.Int2, -original.Int3);
        }

	    public static Number4 Saturate(ref Number4 original)
	    {
            return new Number4
            {
                Number0 = Number.FromFloat(original.Number0.Float, true),
                Number1 = Number.FromFloat(original.Number1.Float, true),
                Number2 = Number.FromFloat(original.Number2.Float, true),
                Number3 = Number.FromFloat(original.Number3.Float, true)
            };
	    }

        public static Number4 Saturate(Number4 original)
        {
            return new Number4
            {
                Number0 = Number.FromFloat(original.Number0.Float, true),
                Number1 = Number.FromFloat(original.Number1.Float, true),
                Number2 = Number.FromFloat(original.Number2.Float, true),
                Number3 = Number.FromFloat(original.Number3.Float, true)
            };
        }

        public static Number4 Subtract(ref Number4 left, ref Number4 right)
        {
            return new Number4(
                Number.FromFloat(left.Number0.Float - right.Number0.Float),
                Number.FromFloat(left.Number1.Float - right.Number1.Float),
                Number.FromFloat(left.Number2.Float - right.Number2.Float),
                Number.FromFloat(left.Number3.Float - right.Number3.Float));
        }

		public static Number4 Swizzle(Number4 original, Operand4ComponentName[] swizzles)
		{
			return new Number4(
				original.GetNumber((int) swizzles[0]),
				original.GetNumber((int) swizzles[1]),
				original.GetNumber((int) swizzles[2]),
				original.GetNumber((int) swizzles[3]));
		}

        public const int SizeInBytes = sizeof(byte) * 16;

        // The following FieldOffset attributes are there because there
        // are multiple ways of "looking at" a Number4. It can be used
        // to store:
        // (1) 2 doubles
        // (2) 4 ints
        // (3) 4 uints
        // (4) 4 floats
        // (5) 16 bytes

		[FieldOffset(0)]
		public Number Number0;

		[FieldOffset(Number.SizeInBytes * 1)]
		public Number Number1;

		[FieldOffset(Number.SizeInBytes * 2)]
		public Number Number2;

		[FieldOffset(Number.SizeInBytes * 3)]
		public Number Number3;


		[FieldOffset(0)]
		public double Double0;

		[FieldOffset(sizeof(double))]
		public double Double1;


        [FieldOffset(0)]
	    public float X;

        [FieldOffset(sizeof(float) * 1)]
        public float Y;

        [FieldOffset(sizeof(float) * 2)]
        public float Z;

        [FieldOffset(sizeof(float) * 3)]
        public float W;


        [FieldOffset(0)]
        public float R;

        [FieldOffset(sizeof(float) * 1)]
        public float G;

        [FieldOffset(sizeof(float) * 2)]
        public float B;

        [FieldOffset(sizeof(float) * 3)]
        public float A;


        [FieldOffset(0)]
        public float Float0;

        [FieldOffset(sizeof(float) * 1)]
        public float Float1;

        [FieldOffset(sizeof(float) * 2)]
        public float Float2;

        [FieldOffset(sizeof(float) * 3)]
        public float Float3;


        [FieldOffset(0)]
        public int Int0;

        [FieldOffset(sizeof(int) * 1)]
        public int Int1;

        [FieldOffset(sizeof(int) * 2)]
        public int Int2;

        [FieldOffset(sizeof(int) * 3)]
        public int Int3;


		public byte[] RawBytes
		{
			get { return new[] { Number0, Number1, Number2, Number3 }.SelectMany(x => x.RawBytes).ToArray(); }
		}


		public bool AllZero
		{
			get { return Number0.UInt == 0 && Number1.UInt == 0 && Number2.UInt == 0 && Number3.UInt == 0; }
		}

		public bool AnyNonZero
		{
			get { return Number0.UInt != 0 || Number1.UInt != 0 || Number2.UInt != 0 || Number3.UInt != 0; }
		}

        #region Swizzles

        public Number4 Xxxx
        {
            get { return new Number4(Number0, Number0, Number0, Number0); }
        }

        public Number4 Xxyz
        {
            get { return new Number4(Number0, Number0, Number1, Number2); }
        }

        public Number4 Xyxx
        {
            get { return new Number4(Number0, Number1, Number0, Number0); }
        }

        public Number4 Xyzx
        {
            get { return new Number4(Number0, Number1, Number2, Number0); }
        }

	    public Number4 Yyyy
	    {
	        get { return new Number4(Number1, Number1, Number1, Number1); }
	    }

        public Number4 Yyzw
        {
            get { return new Number4(Number1, Number1, Number2, Number3); }
        }

        public Number4 Yzwy
        {
            get { return new Number4(Number1, Number2, Number3, Number1); }
        }

        public Number4 Zzzz
        {
            get { return new Number4(Number2, Number2, Number2, Number2); }
        }

        public Number4 Wwww
        {
            get { return new Number4(Number3, Number3, Number3, Number3); }
        }

        #endregion

        public Number4(Number number0, Number number1, Number number2, Number number3)
            : this()
		{
			Number0 = number0;
			Number1 = number1;
			Number2 = number2;
			Number3 = number3;
		}

		public Number4(float float0, float float1, float float2, float float3)
            : this()
		{
			X = float0;
			Y = float1;
			Z = float2;
			W = float3;
		}

		public Number4(double double0, double double1)
            : this()
		{
			Double0 = double0;
			Double1 = double1;
		}

		public double GetDouble(int i)
		{
			switch (i)
			{
				case 0:
					return Double0;
				case 2:
					return Double1;
				default:
					throw new ArgumentOutOfRangeException("i", string.Format("Index '{0}' is out of range.", i));
			}
		}

		public Number GetNumber(int i)
		{
			switch (i)
			{
				case 0:
					return Number0;
				case 1:
					return Number1;
				case 2:
					return Number2;
				case 3:
					return Number3;
				default:
					throw new ArgumentOutOfRangeException("i", string.Format("Index '{0}' is out of range.", i));
			}
		}

		public void SetNumber(int i, Number value)
		{
			switch (i)
			{
				case 0:
					Number0 = value;
					break;
				case 1:
					Number1 = value;
					break;
				case 2:
					Number2 = value;
					break;
				case 3:
					Number3 = value;
					break;
				default :
					throw new ArgumentOutOfRangeException("i", string.Format("Index '{0}' is out of range.", i));
			}
		}

		public void SetDouble(int i, double value)
		{
			switch (i)
			{
				case 0:
					Double0 = value;
					break;
				case 1:
					Double1 = value;
					break;
				default:
					throw new ArgumentOutOfRangeException("i", string.Format("Index '{0}' is out of range.", i));
			}
		}

		public Number GetMaskedNumber(ComponentMask mask)
		{
			if (mask.HasFlag(ComponentMask.X))
				return Number0;
			if (mask.HasFlag(ComponentMask.Y))
				return Number1;
			if (mask.HasFlag(ComponentMask.Z))
				return Number2;
			if (mask.HasFlag(ComponentMask.W))
				return Number3;
			throw new ArgumentOutOfRangeException("mask");
		}

		public Number GetSwizzledNumber(Operand4ComponentName name)
		{
			switch (name)
			{
				case Operand4ComponentName.X:
					return Number0;
				case Operand4ComponentName.Y:
					return Number1;
				case Operand4ComponentName.Z:
					return Number2;
				case Operand4ComponentName.W:
					return Number3;
				default:
					throw new ArgumentOutOfRangeException("name");
			}
		}

		public void WriteMaskedValue(Number4 value, ComponentMask mask)
		{
			if (mask.HasFlag(ComponentMask.X))
				Number0 = value.Number0;
			if (mask.HasFlag(ComponentMask.Y))
				Number1 = value.Number1;
			if (mask.HasFlag(ComponentMask.Z))
				Number2 = value.Number2;
			if (mask.HasFlag(ComponentMask.W))
				Number3 = value.Number3;
		}

		public override string ToString()
		{
			return string.Format("{0},{1},{2},{3}",
				Number0, Number1, Number2, Number3);
		}
	}
}