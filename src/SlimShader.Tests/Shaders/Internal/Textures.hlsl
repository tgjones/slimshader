struct VertexShaderInput
{
	float4 pos : POSITION;
	float2 tex : TEXCOORD;
};

struct PixelShaderInput
{
	float4 pos : SV_POSITION;
	float2 tex : TEXCOORD;
};

float4x4 WorldViewProjection;

Texture2D Picture;
SamplerState PictureSampler;
SamplerComparisonState PictureSamplerComparison;

Texture2DMS<float4, 32> PictureMS;

PixelShaderInput VS(VertexShaderInput input)
{
	PixelShaderInput output = (PixelShaderInput) 0;
	
	output.pos = mul(input.pos, WorldViewProjection);
	output.tex = input.tex;
	
	return output;
}

float4 PS(PixelShaderInput input) : SV_Target
{
	float lod = Picture.CalculateLevelOfDetail(PictureSampler, input.tex);
	float lodUnclamped = Picture.CalculateLevelOfDetailUnclamped(PictureSampler, input.tex);
	float4 gathered = Picture.Gather(PictureSampler, input.tex, int2(0, 1));
	
	int width, height, numLevels;
	Picture.GetDimensions(1, width, height, numLevels);

	float2 samplePos = PictureMS.GetSamplePosition(0);

	float4 loaded = PictureMS.Load(int2(25, 10), 1, int2(0, 1));

	float4 sampled = Picture.Sample(PictureSampler, input.tex);
	float4 sampleBias = Picture.SampleBias(PictureSampler, input.tex, 0.5);
	float4 sampleCmp = Picture.SampleCmp(PictureSamplerComparison, input.tex, 0.4);
	float4 sampleCmpLevelZero = Picture.SampleCmpLevelZero(PictureSamplerComparison, input.tex, 0.6);
	float4 sampleGrad = Picture.SampleGrad(PictureSampler, input.tex, 0.1, 0.2);
	float4 sampleLevel = Picture.SampleLevel(PictureSampler, input.tex, 1.5);

	return float4(lod, lodUnclamped, width, height)
		+ gathered
		+ float4(samplePos, 0, 0)
		+ sampled
		+ sampleBias
		+ sampleCmp
		+ sampleCmpLevelZero
		+ sampleGrad
		+ sampleLevel;
}