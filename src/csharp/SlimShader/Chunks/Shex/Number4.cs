using System;
using System.Runtime.InteropServices;

namespace SlimShader.Chunks.Shex
{
	/// <summary>
	/// Represents four Numbers, or two doubles.
	/// </summary>
	[StructLayout(LayoutKind.Explicit, Size = Number.SizeInBytes * 4)]
	public struct Number4
	{
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

		public double GetDouble(int i)
		{
			switch (i)
			{
				case 0:
					return Double0;
				case 1:
					return Double1;
				default:
					throw new ArgumentOutOfRangeException("i");
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
					throw new ArgumentOutOfRangeException("i");
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
					throw new ArgumentOutOfRangeException("i");
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
					throw new ArgumentOutOfRangeException("i");
			}
		}
	}
}