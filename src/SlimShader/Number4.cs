using System;
using System.Runtime.InteropServices;
using SlimShader.Chunks.Common;
using SlimShader.Chunks.Shex;

namespace SlimShader
{
	/// <summary>
	/// Represents four Numbers, or two doubles.
	/// </summary>
	[StructLayout(LayoutKind.Explicit, Size = Number.SizeInBytes * 4 + 4)]
	public struct Number4
	{
		public static Number4 Abs(Number4 original)
		{
			switch (original.Type)
			{
				case Number4Type.Number:
					return new Number4(
						Number.Abs(original.Number0),
						Number.Abs(original.Number1),
						Number.Abs(original.Number2),
						Number.Abs(original.Number3));
				case Number4Type.Double:
					return new Number4(
						Math.Abs(original.Double0),
						Math.Abs(original.Double1));
				default:
					throw new InvalidOperationException(string.Format("Abs is not a valid operation for number type '{0}'.", original.Type));
			}
		}

		public static Number4 Negate(Number4 original)
		{
			switch (original.Type)
			{
				case Number4Type.Number:
					return new Number4(
						Number.Negate(original.Number0),
						Number.Negate(original.Number1),
						Number.Negate(original.Number2),
						Number.Negate(original.Number3));
				case Number4Type.Double:
					return new Number4(
						-original.Double0,
						-original.Double1);
				default:
					throw new InvalidOperationException(string.Format("Negate is not a valid operation for number type '{0}'.", original.Type));
			}
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

		[FieldOffset(Number.SizeInBytes * 4)]
		public Number4Type Type;

		public bool AllZero
		{
			get { return Number0.UInt == 0 && Number1.UInt == 0 && Number2.UInt == 0 && Number3.UInt == 0; }
		}

		public bool AnyNonZero
		{
			get { return Number0.UInt != 0 || Number1.UInt != 0 || Number2.UInt != 0 || Number3.UInt != 0; }
		}

		public Number4(Number number0, Number number1, Number number2, Number number3)
			: this()
		{
			Type = Number4Type.Number;
			Number0 = number0;
			Number1 = number1;
			Number2 = number2;
			Number3 = number3;
		}

		public Number4(float float0, float float1, float float2, float float3)
			: this()
		{
			Type = Number4Type.Number;
			Number0 = Number.FromFloat(float0);
			Number1 = Number.FromFloat(float1);
			Number2 = Number.FromFloat(float2);
			Number3 = Number.FromFloat(float3);
		}

		public Number4(double double0, double double1)
			: this()
		{
			Type = Number4Type.Double;
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
			Type = Number4Type.Number;
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
			Type = Number4Type.Double;
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