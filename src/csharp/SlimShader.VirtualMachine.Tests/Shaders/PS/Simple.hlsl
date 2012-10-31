struct PS_INPUT
{
    float4 Position : POSITION;
};

struct PS_OUTPUT
{
    float4 Color : SV_Target;
};

PS_OUTPUT main(in PS_INPUT In)
{
    PS_OUTPUT Out;
    Out.Color = float4(1.0f, 0.5f, 0.4f, 1);
    return Out;
}