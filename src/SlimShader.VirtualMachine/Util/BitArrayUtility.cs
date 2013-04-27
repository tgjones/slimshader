using System.Collections;

namespace SlimShader.VirtualMachine.Util
{
	internal static class BitArrayUtility
	{
		public static BitArray CreateAllZero(int length)
		{
			var result = new BitArray(length);
			result.SetAll(false);
			return result;
		}

		public static BitArray CreateAllOne(int length)
		{
			var result = new BitArray(length);
			result.SetAll(true);
			return result;
		}
	}
}