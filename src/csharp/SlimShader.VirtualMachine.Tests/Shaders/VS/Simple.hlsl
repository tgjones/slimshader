matrix WorldViewProjection;

struct VS_INPUT
{
    float3 Position : POSITION;
	float3 Normal : NORMAL;
	float2 TexCoord : TEXCOORD0;
};

struct VS_OUTPUT
{
	float4 Position : SV_Position;
	float3 Normal : NORMAL;
	float2 TexCoord : TEXCOORD0;
};

VS_OUTPUT main(in VS_INPUT input)
{
    VS_OUTPUT output;
    output.Position = mul(WorldViewProjection, input.Position);
	output.Normal = input.Normal;
	output.TexCoord = input.TexCoord;
	return output;
}