using SlimShader.Chunks.Shex;
using SlimShader.Util;

namespace SlimShader.Chunks.Xsgn
{
	/// <summary>
	/// Values that identify shader parameters that use system-value semantics.
	/// Based on D3D_NAME. This is similar to, but different from, <see cref="SystemValueName"/>
	/// </summary>
	public enum Name
	{
		[Description("NONE")]
		Undefined = 0,

		[Description("POS")]
		Position = 1,

		ClipDistance = 2,
		CullDistance = 3,

		[Description("RTINDEX")]
		RenderTargetArrayIndex = 4,

		ViewportArrayIndex = 5,

		[Description("VERTID")]
		VertexID = 6,

		[Description("PRIMID")]
		PrimitiveID = 7,

		InstanceID = 8,
		IsFrontFace = 9,
		SampleIndex = 10,

		[Description("QUADEDGE")]
		FinalQuadEdgeTessFactor = 11,

		[Description("QUADINT")]
		FinalQuadInsideTessFactor = 12,

		[Description("TRIEDGE")]
		FinalTriEdgeTessFactor = 13,

		[Description("TRIINT")]
		FinalTriInsideTessFactor = 14,

		[Description("LINEDET")]
		FinalLineDetailTessFactor = 15,

		[Description("LINEDEN")]
		FinalLineDensityTessFactor = 16,

		[Description("TARGET")]
		Target = 64,

		[Description("DEPTH")]
		Depth = 65,

		Coverage = 66,

		[Description("DEPTHGE")]
		DepthGreaterEqual = 67,

		[Description("DEPTHLE")]
		DepthLessEqual = 68
	}
}