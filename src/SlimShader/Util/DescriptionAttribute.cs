using System;

namespace SlimShader.Util
{
	/// <summary>
	/// Provided here because Portable Class Libraries don't include
	/// System.ComponentModel.DescriptionAttribute.
	/// </summary>
	public class DescriptionAttribute : Attribute
	{
		private readonly string _description;

		public string Description
		{
			get { return _description; }
		}

		public DescriptionAttribute(string description)
		{
			_description = description;
		}
	}
}