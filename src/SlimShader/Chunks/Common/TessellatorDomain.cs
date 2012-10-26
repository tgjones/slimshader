using SlimShader.Util;

namespace SlimShader.Chunks.Common
{
	/// <summary>
	/// Values that identify domain options for tessellator data.
	/// Based on D3D_TESSELLATOR_DOMAIN.
	/// </summary>
	public enum TessellatorDomain
	{
		/// <summary>
		/// The data type is undefined.
		/// </summary>
		Undefined = 0,

		/// <summary>
		/// Isoline data.
		/// </summary>
		[Description("domain_isoline")]
		Isoline = 1,

		/// <summary>
		/// Triangle data.
		/// </summary>
		Tri = 2,

		/// <summary>
		/// Quad data.
		/// </summary>
		Quad = 3
	}
}