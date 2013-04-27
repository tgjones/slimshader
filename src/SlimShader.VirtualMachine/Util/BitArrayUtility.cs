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

		public static bool Any(this BitArray bitArray)
		{
			for (int i = 0; i < bitArray.Length; i++)
				if (bitArray[i])
					return true;
			return false;
		}
	}
}