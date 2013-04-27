namespace SlimShader.Chunks.Shex
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