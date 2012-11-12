namespace SlimShader.Chunks
{
	public enum ChunkType
	{
		Unknown,

		/// <summary>
		/// Interfaces
		/// </summary>
		Ifce,

		/// <summary>
		/// Input signature
		/// </summary>
		Isgn,

		/// <summary>
		/// Output signature (SM5)
		/// </summary>
		Osg5,

		/// <summary>
		/// Output signature
		/// </summary>
		Osgn,

		/// <summary>
		/// Patch constant signature
		/// </summary>
		Pcsg,

		/// <summary>
		/// Resource definition
		/// </summary>
		Rdef,

		/// <summary>
		/// Shader debugging info (old-style)
		/// </summary>
		Sdbg,

		/// <summary>
		/// ?
		/// </summary>
		Sfi0,

		/// <summary>
		/// Shader (SM 4.0)
		/// </summary>
		Shdr,

		/// <summary>
		/// Shader (SM 5.0)
		/// </summary>
		Shex,

		/// <summary>
		/// Shader debugging info (new-style)
		/// </summary>
		Spdb,

		/// <summary>
		/// Statistics
		/// </summary>
		Stat
	}
}