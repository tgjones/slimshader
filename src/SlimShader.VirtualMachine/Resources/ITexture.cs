namespace SlimShader.VirtualMachine.Resources
{
	public interface ITexture
	{
        TextureDimension Dimension { get; }

        int MipMapCount { get; }
	    ITextureMipMap GetMipMap(int level);
	}
}