struct GS_CUBE_IN
{
    float3 position : TEXCOORD0;
    float3 worldPosition: TEXCOORD1;
    float3 normal : TEXCOORD2;
    float2 uv : TEXCOORD3;
};

struct PS_CUBE_IN
{
    float4 position : SV_POSITION;
    float3 worldPosition : TEXCOORD0;
    float3 normal : TEXCOORD1;
    float2 uv : TEXCOORD2;
    uint RTIndex : SV_RenderTargetArrayIndex;
};

float4x4 TransformMatrixArray[6];

[maxvertexcount(24)]
void GS_CubeMap(triangle GS_CUBE_IN input[3], inout TriangleStream<PS_CUBE_IN> CubeMapStream)
{
    for (int f = 0; f < 6; f++)
    {
        // Compute screen coordinates
        PS_CUBE_IN output;
        output.RTIndex = f;
        for (int v = 0; v < 3; v++)
        {
            output.position = mul(TransformMatrixArray[f], float4(input[v].position, 1));
            output.worldPosition = input[v].worldPosition;
            output.uv = input[v].uv;
            output.normal = input[v].normal;
            CubeMapStream.Append(output);
        }
        CubeMapStream.RestartStrip();
    }
}