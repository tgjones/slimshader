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
		/// Shader debugging info
		/// </summary>
		Sdbg,

		/// <summary>
		/// ?
		/// </summary>
		Sfi0,

		/// <summary>
		/// Shader
		/// </summary>
		Shdr,

		/// <summary>
		/// Shader
		/// </summary>
		Shex,

		/// <summary>
		/// Statistics
		/// </summary>
		Stat
	}
}