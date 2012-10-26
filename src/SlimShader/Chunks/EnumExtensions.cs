using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SlimShader.Util;

namespace SlimShader.Chunks
{
	internal static class EnumExtensions
	{
		private static readonly Dictionary<Type, Dictionary<Type, Dictionary<Enum, object>>> AttributeValues;

		static EnumExtensions()
		{
			AttributeValues = new Dictionary<Type, Dictionary<Type, Dictionary<Enum, object>>>();
		}

		public static string GetDescription(this Enum value)
		{
			return value.GetAttributeValue<DescriptionAttribute, string>((a, v) =>
			{
				if (a == null)
					return v.ToString();
				return a.Description;
			});
		}

		public static TValue GetAttributeValue<TAttribute, TValue>(this Enum value,
			Func<TAttribute, Enum, TValue> getValueCallback)
			where TAttribute : Attribute
		{
			Type type = value.GetType();

			if (!AttributeValues.ContainsKey(type))
				AttributeValues[type] = new Dictionary<Type, Dictionary<Enum, object>>();

			var attributeValuesForType = AttributeValues[type];

			var attributeType = typeof(TAttribute);
			if (!attributeValuesForType.ContainsKey(attributeType))
				attributeValuesForType[attributeType] = Enum.GetValues(type).Cast<Enum>().Distinct()
					.ToDictionary(x => x, x => (object) GetAttributeValueInternal(x, getValueCallback));

			var attributeValues = attributeValuesForType[attributeType];
			if (!attributeValues.ContainsKey(value))
				throw new ArgumentOutOfRangeException("value",
					string.Format("Could not find attribute value for type '{0}' and value '{1}'.", type, value));
			return (TValue) attributeValues[value];
		}

		private static TValue GetAttributeValueInternal<TAttribute, TValue>(Enum value,
			Func<TAttribute, Enum, TValue> getValueCallback)
			where TAttribute : Attribute
		{
			FieldInfo field = value.GetType().GetField(value.ToString());
			var attribute = Attribute.GetCustomAttribute(
				field, typeof(TAttribute)) as TAttribute;
			return getValueCallback(attribute, value);
		}
	}
}