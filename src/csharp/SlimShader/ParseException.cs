using System;

namespace SlimShader
{
	public class ParseException : Exception
	{
		public ParseException(string format, params object[] args)
			: this(string.Format(format, args))
		{

		}

		public ParseException(string message)
			: base(message)
		{
			
		}
	}
}