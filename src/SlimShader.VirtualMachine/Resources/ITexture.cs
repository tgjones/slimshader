namespace SlimShader.VirtualMachine.Resources
{
	public interface ITexture
	{
        int MipMapCount { get; }
	    ITextureMipMap GetMipMap(int arraySlice, int mipLevel);
	}
}