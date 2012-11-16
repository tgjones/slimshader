namespace SlimShader.Chunks.Shex
{
	public enum SystemValueName
	{
		[Description("NONE")]
		Undefined = 0,

		[Description("position")]
		Position = 1,

		[Description("clip_distance")]
		ClipDistance = 2,

		[Description("cull_distance")]
		CullDistance = 3,

		[Description("rendertarget_array_index")]
		RenderTargetArrayIndex = 4,

		[Description("viewport_array_index")]
		ViewportArrayIndex = 5,

		[Description("vertex_id")]
		VertexID = 6,

		[Description("primitive_id")]
		PrimitiveID = 7,

		[Description("instance_id")]
		InstanceID = 8,

		[Description("is_front_face")]
		IsFrontFace = 9,

		[Description("sampleIndex")]
		SampleIndex = 10,

		// The following are added for D3D11

		[Description("finalQuadUeq0EdgeTessFactor")]
		FinalQuadUEq0EdgeTessFactor = 11,

		[Description("finalQuadVeq0EdgeTessFactor")]
		FinalQuadVEq0EdgeTessFactor = 12,

		[Description("finalQuadUeq1EdgeTessFactor")]
		FinalQuadUEq1EdgeTessFactor = 13,

		[Description("finalQuadVeq1EdgeTessFactor")]
		FinalQuadVEq1EdgeTessFactor = 14,

		[Description("finalQuadUInsideTessFactor")]
		FinalQuadUInsideTessFactor = 15,

		[Description("finalQuadVInsideTessFactor")]
		FinalQuadVInsideTessFactor = 16,

		[Description("finalTriUeq0EdgeTessFactor")]
		FinalTriUEq0EdgeTessFactor = 17,

		[Description("finalTriVeq0EdgeTessFactor")]
		FinalTriVEq0EdgeTessFactor = 18,

		[Description("finalTriWeq0EdgeTessFactor")]
		FinalTriWEq0EdgeTessFactor = 19,

		[Description("finalTriInsideTessFactor")]
		FinalTriInsideTessFactor = 20,

		[Description("finalLineDetailTessFactor")]
		FinalLineDetailTessFactor = 21,

		[Description("finalLineDensityTessFactor")]
		FinalLineDensityTessFactor = 22
	}
}