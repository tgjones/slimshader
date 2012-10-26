RWTexture2D<float4> Output;

[numthreads(32, 32, 1)]
void main( uint3 threadID : SV_DispatchThreadID )
{
    Output[threadID.xy] = float4(threadID.xy / 1024.0f, 0, 1);
}