#include "PCH.h"

namespace SlimShader
{
	/// <summary>
	/// Values that identify domain options for tessellator data.
	/// Based on D3D_TESSELLATOR_DOMAIN.
	/// </summary>
	enum class TessellatorDomain
	{
		/// <summary>
		/// The data type is undefined.
		/// </summary>
		Undefined = 0,

		/// <summary>
		/// Isoline data.
		/// </summary>
		Isoline = 1,

		/// <summary>
		/// Triangle data.
		/// </summary>
		Tri = 2,

		/// <summary>
		/// Quad data.
		/// </summary>
		Quadrilateral = 3
	};
};