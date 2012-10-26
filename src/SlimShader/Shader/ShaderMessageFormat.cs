namespace SlimShader.Shader
{
	public enum ShaderMessageFormat
	{
		/// <summary>
		/// No formatting, just a text string.  Operands are ignored.
		/// </summary>
		AnsiText,

		/// <summary>
		/// Format string follows C/C++ printf conventions.
		/// </summary>
		AnsiPrintf
	}
}