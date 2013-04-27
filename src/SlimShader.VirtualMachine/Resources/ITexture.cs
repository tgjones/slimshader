namespace SlimShader.VirtualMachine.Resources
{
	public interface ITexture
	{
		float CalculateLevelOfDetail(ISampler sampler, ref Number4 ddx, ref Number4 ddy);

		Number4 SampleGrad(ISampler sampler, ref Number4 location, ref Number4 ddx, ref Number4 ddy);
		Number4 SampleLevel(ISampler sampler, ref Number4 location, float lod);
	}
}