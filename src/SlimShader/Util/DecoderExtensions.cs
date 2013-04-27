using System;

namespace SlimShader.Util
{
	public static class DecoderExtensions
	{
		/// <summary>
		/// Generates mask and shift values to unpack a value using bitwise operators.
		/// For example, the Sampler Declaration contains these values (d3d10tokenizedprogramformat.hpp):
		/// 
		/// [10:00] D3D10_SB_OPCODE_DCL_SAMPLER
		/// [14:11] D3D10_SB_SAMPLER_MODE
		/// [23:15] Ignored, 0
		/// [30:24] Instruction length in DWORDs including the opcode token.
		/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
		///         contains extended operand description.  This dcl is currently not
		///         extended.
		/// 
		/// To extract the D3D10_SB_SAMPLER_MODE value, use start = 11 and end = 14.
		/// </summary>
		/// <param name="token"></param>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns></returns>
		public static uint DecodeValue(this uint token, byte start, byte end)
		{
			uint mask = GenerateMask(start, end);
			int shift = start;

			return (token & mask) >> shift;
		}

		public static T DecodeValue<T>(this uint token, byte start, byte end)
			where T : struct
		{
			var decodedValue = DecodeValue(token, start, end);
			if (typeof(T).IsEnum)
				return (T) Enum.ToObject(typeof(T), decodedValue);
			return (T) Convert.ChangeType(decodedValue, typeof(T));
		}

		private static uint GenerateMask(byte start, byte end)
		{
			uint mask = 0;
			for (int i = start; i <= end; i++)
				mask |= (uint) Math.Pow(2, i);
			return mask;
		}

		public static sbyte DecodeSigned4BitValue(this uint token, byte start, byte end)
		{
			if (end - start != 3)
				throw new ParseException("DecodeSigned4BitValue can only be called for 4-bit intervals");
			var value = token.DecodeValue<sbyte>(start, end);
			if (value > 7)
				return (sbyte) (value - 16);
			return value;
		}

		public static uint ToFourCc(this string fourCc)
		{
			if (string.IsNullOrEmpty(fourCc) || fourCc.Length != 4)
				throw new ArgumentOutOfRangeException("fourCc", "Invalid FOURCC: " + fourCc);
			var a = (byte) fourCc[0];
			var b = (byte) fourCc[1];
			var c = (byte) fourCc[2];
			var d = (byte) fourCc[3];
			return a | ((uint) (b << 8)) | ((uint) c << 16) | ((uint) d << 24);
		}

		public static string ToFourCcString(this uint fourCc)
		{
			var a = fourCc.DecodeValue<char>(00, 07);
			var b = fourCc.DecodeValue<char>(08, 15);
			var c = fourCc.DecodeValue<char>(16, 23);
			var d = fourCc.DecodeValue<char>(24, 31);

			return new string(new[] { a, b, c, d });
		}
	}
}