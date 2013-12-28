namespace SlimShader.VirtualMachine.Resources
{
	internal class TextureCubeSampler : Texture2DSampler
	{
		protected override void GetMipMapAndTransformedCoordinates(
			ITexture texture, ref Number4 location,
			int level, out ITextureMipMap mipMap,
			out Number4 textureCoordinates)
		{
			int arrayIndex;
			CubeMapUtility.GetCubeMapCoordinates(ref location,
				out arrayIndex, out textureCoordinates);

			mipMap = texture.GetMipMap(arrayIndex, level);

			textureCoordinates.X *= mipMap.Width;
			textureCoordinates.Y *= mipMap.Height;
		}
	}
}