using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SlimShader.Chunks
{
	internal static class EnumExtensions
	{
		private static readonly Dictionary<Type, Dictionary<Type, Dictionary<Enum, Attribute[]>>> AttributeValues;

		static EnumExtensions()
		{
			AttributeValues = new Dictionary<Type, Dictionary<Type, Dictionary<Enum, Attribute[]>>>();
		}

		public static string GetDescription(this Enum value, ChunkType chunkType = ChunkType.Unknown)
		{
			return value.GetAttributeValue<DescriptionAttribute, string>((a, v) =>
			{
				var attribute = a.FirstOrDefault(x => x.ChunkType == chunkType);
				if (attribute == null)
					return v.ToString();
				return attribute.Description;
			});
		}

		public static TValue GetAttributeValue<TAttribute, TValue>(this Enum value,
			Func<TAttribute[], Enum, TValue> getValueCallback)
			where TAttribute : Attribute
		{
			Type type = value.GetType();

			if (!AttributeValues.ContainsKey(type))
				AttributeValues[type] = new Dictionary<Type, Dictionary<Enum, Attribute[]>>();

			var attributeValuesForType = AttributeValues[type];

			var attributeType = typeof(TAttribute);
			if (!attributeValuesForType.ContainsKey(attributeType))
				attributeValuesForType[attributeType] = Enum.GetValues(type).Cast<Enum>().Distinct()
					.ToDictionary(x => x, GetAttribute<TAttribute>);

			var attributeValues = attributeValuesForType[attributeType];
			if (!attributeValues.ContainsKey(value))
				throw new ArgumentException(string.Format("Could not find attribute value for type '{0}' and value '{1}'.", type, value));
			return getValueCallback((TAttribute[]) attributeValues[value], value);
		}

		private static Attribute[] GetAttribute<TAttribute>(Enum value)
			where TAttribute : Attribute
		{
			FieldInfo field = value.GetType().GetField(value.ToString());
			return Attribute.GetCustomAttributes(field, typeof(TAttribute));
		}
	}
}