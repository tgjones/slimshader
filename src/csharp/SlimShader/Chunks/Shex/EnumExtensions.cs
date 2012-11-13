using System;
using System.Linq;
using SlimShader.Chunks.Common;

namespace SlimShader.Chunks.Shex
{
	internal static class EnumExtensions
	{
		public static string GetDescription(this ComponentMask value)
		{
			string result = string.Empty;

			if (value.HasFlag(ComponentMask.X))
				result += "x";
			if (value.HasFlag(ComponentMask.Y))
				result += "y";
			if (value.HasFlag(ComponentMask.Z))
				result += "z";
			if (value.HasFlag(ComponentMask.W))
				result += "w";
			
			return result;
		}

		public static string GetDescription(this Operand4ComponentName value)
		{
			switch (value)
			{
				case Operand4ComponentName.X:
					return "x";
				case Operand4ComponentName.Y:
					return "y";
				case Operand4ComponentName.Z:
					return "z";
				case Operand4ComponentName.W:
					return "w";
				default:
					throw new ArgumentOutOfRangeException("value", "Unrecognised value:" + value);
			}
		}

		public static string GetDescription(this SyncFlags value)
		{
			string result = string.Empty;

			if (value.HasFlag(SyncFlags.UnorderedAccessViewGlobal))
				result += "_uglobal";
			if (value.HasFlag(SyncFlags.UnorderedAccessViewGroup))
				result += "_ugroup";
			if (value.HasFlag(SyncFlags.SharedMemory))
				result += "_g";
			if (value.HasFlag(SyncFlags.ThreadsInGroup))
				result += "_t";

			return result;
		}

		public static string GetDescription(this GlobalFlags value)
		{
			string result = string.Empty;
			foreach (Enum enumValue in Enum.GetValues(typeof(GlobalFlags)))
				if ((int) value > 0 && value.HasFlag(enumValue))
				{
					if (!string.IsNullOrEmpty(result))
						result += " | ";
					result += enumValue.GetDescription();
				}
			return result;
		}

		public static bool IsConditionalInstruction(this OpcodeType type)
		{
			switch (type)
			{
				case OpcodeType.BreakC :
				case OpcodeType.CallC :
				case OpcodeType.Discard :
				case OpcodeType.If :
					return true;
				default :
					return false;
			}
		}

		public static bool IsDeclaration(this OpcodeType type)
		{
			return (type >= OpcodeType.DclResource && type <= OpcodeType.DclGlobalFlags)
				|| (type >= OpcodeType.DclStream && type <= OpcodeType.DclResourceStructured)
				|| type == OpcodeType.DclGsInstanceCount;
		}

		public static NumberType GetNumberType(this OpcodeType type)
		{
			return type.GetAttributeValue<NumberTypeAttribute, NumberType>((a, v) =>
			{
				if (!a.Any())
					return NumberType.Unknown;
				return a.First().Type;
			});	
		}

		public static bool RequiresRegisterNumberFor1DIndex(this OperandType type)
		{
			switch (type)
			{
				case OperandType.ImmediateConstantBuffer:
				case OperandType.ThisPointer:
					return false;
				default:
					return true;
			}
		}

		public static bool RequiresRegisterNumberFor2DIndex(this OperandType type)
		{
			switch (type)
			{
				case OperandType.InputControlPoint:
				case OperandType.Input:
					return false;
				default:
					return true;
			}
		}

		public static string Wrap(this OperandModifier modifier, string valueToWrap)
		{
			switch (modifier)
			{
				case OperandModifier.None:
					return valueToWrap;
				case OperandModifier.Neg:
					return "-" + valueToWrap;
				case OperandModifier.Abs:
					return "|" + valueToWrap + "|";
				case OperandModifier.AbsNeg:
					return "-|" + valueToWrap + "|";
				default:
					throw new ArgumentOutOfRangeException("modifier", "Unrecognised modifier:" + modifier);
			}
		}

		public static bool IsNestedSectionStart(this OpcodeType type)
		{
			switch (type)
			{
				case OpcodeType.Loop :
				case OpcodeType.If :
				case OpcodeType.Else :
				case OpcodeType.Switch :
					return true;
				default :
					return false;
			}
		}

		public static bool IsNestedSectionEnd(this OpcodeType type)
		{
			switch (type)
			{
				case OpcodeType.EndLoop:
				case OpcodeType.EndIf:
				case OpcodeType.Else:
				case OpcodeType.EndSwitch:
					return true;
				default:
					return false;
			}
		}
	}
}