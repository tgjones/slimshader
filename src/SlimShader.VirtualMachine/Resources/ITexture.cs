namespace SlimShader.VirtualMachine.Resources
{
	public interface ITexture
	{
        float CalculateLevelOfDetail(ISamplerState samplerState, ref Number4 ddx, ref Number4 ddy);

        Number4 SampleGrad(ISamplerState samplerState, ref Number4 location, ref Number4 ddx, ref Number4 ddy);
        Number4 SampleLevel(ISamplerState samplerState, ref Number4 location, float lod);
	}
}