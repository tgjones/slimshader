using System;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Shex.Tokens;

namespace SlimShader.VirtualMachine.Util
{
	internal static class OperandUtility
	{
		public static Number4 ApplyOperandModifier(Number4 value, NumberType numberType, OperandModifier modifier)
		{
			switch (modifier)
			{
				case OperandModifier.None:
					return value;
				case OperandModifier.Neg:
                    return Number4.Negate(value, numberType);
				case OperandModifier.Abs:
                    return Number4.Abs(value, numberType);
				case OperandModifier.AbsNeg:
                    return Number4.Negate(Number4.Abs(value, numberType), numberType);
				default:
					throw new ArgumentOutOfRangeException("modifier");
			}
		}

		public static Number4 ApplyOperandSelectionMode(Number4 value, Operand operand)
		{
			if (operand.SelectionMode != Operand4ComponentSelectionMode.Swizzle)
				return value;
			return Number4.Swizzle(value, operand.Swizzles);
		}
	}
}