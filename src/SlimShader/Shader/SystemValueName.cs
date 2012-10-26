using SlimShader.Util;

namespace SlimShader.Shader
{
	public enum SystemValueName
	{
		[Description("NONE")]
		Undefined = 0,

		[Description("position")]
		Position = 1,

		[Description("clipdistance")]
		ClipDistance = 2,

		[Description("culldistance")]
		CullDistance = 3,

		[Description("rendertarget_array_index")]
		RenderTargetArrayIndex = 4,

		[Description("viewportarrayindex")]
		ViewportArrayIndex = 5,

		[Description("vertexid")]
		VertexID = 6,

		[Description("primitive_id")]
		PrimitiveID = 7,

		[Description("instanceid")]
		InstanceID = 8,

		[Description("isfrontface")]
		IsFrontFace = 9,

		[Description("sampleindex")]
		SampleIndex = 10,

		// The following are added for D3D11

		FinalQuadUEq0EdgeTessFactor = 11,
		FinalQuadVEq0EdgeTessFactor = 12,
		FinalQuadUEq1EdgeTessFactor = 13,
		FinalQuadVEq1EdgeTessFactor = 14,
		FinalQuadUInsideTessFactor = 15,
		FinalQuadVInsideTessFactor = 16,
		FinalTriUEq0EdgeTessFactor = 17,
		FinalTriVEq0EdgeTessFactor = 18,
		FinalTriWEq0EdgeTessFactor = 19,
		FinalTriInsideTessFactor = 20,

		[Description("finalLineDetailTessFactor")]
		FinalLineDetailTessFactor = 21,

		[Description("finalLineDensityTessFactor")]
		FinalLineDensityTessFactor = 22
	}
}