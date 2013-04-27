using System;

namespace SlimShader.Chunks.Rdef
{
	[Flags]
	public enum ShaderRequiresFlags
	{
		/// <summary>
		/// Shader requires that the graphics driver and hardware support double data type.
		/// </summary>
		Doubles = 0x00000001,

		/// <summary>
		/// Shader requires an early depth stencil.
		/// </summary>
		EarlyDepthStencil = 0x00000002,

		/// <summary>
		/// Shader requires unordered access views (UAVs) at every pipeline stage.
		/// </summary>
		UavsAtEveryStage = 0x00000004,

		/// <summary>
		/// Shader requires 64 UAVs.
		/// </summary>
		Requires64Uavs = 0x00000008,

		/// <summary>
		/// Shader requires the graphics driver and hardware to support minimum precision.
		/// </summary>
		MinimumPrecision = 0x00000010,

		/// <summary>
		/// Shader requires that the graphics driver and hardware support extended doubles instructions.
		/// </summary>
		DoubleExtensionsFor11Point1 = 0x00000020,

		/// <summary>
		/// Shader requires that the graphics driver and hardware support the msad4 intrinsic function in shaders.
		/// </summary>
		ShaderExtensionsFor11Point1 = 0x00000040,

		/// <summary>
		/// Shader requires that the graphics driver and hardware support Direct3D 9 shadow support.
		/// </summary>
		Level9ComparisonFiltering = 0x00000080
	}
}