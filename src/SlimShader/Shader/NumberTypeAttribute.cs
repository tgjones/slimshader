using System;

namespace SlimShader.Shader
{
	[AttributeUsage(AttributeTargets.Field)]
	public class NumberTypeAttribute : Attribute
	{
		private readonly NumberType _type;

		public NumberType Type
		{
			get { return _type; }
		}

		public NumberTypeAttribute(NumberType type)
		{
			_type = type;
		}
	}
}