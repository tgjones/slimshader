using SlimShader.VirtualMachine.Util;

namespace SlimShader.VirtualMachine.Resources
{
	internal class Texture2DArraySampler : Texture2DSampler
	{
		protected override void GetMipMapAndTransformedCoordinates(
			ITexture texture, ref Number4 location,
			int level, out ITextureMipMap mipMap,
			out Number4 textureCoordinates)
		{
			var arraySlice = MathUtility.Round(location.Float2);
			mipMap = texture.GetMipMap(arraySlice, level);
			textureCoordinates = new Number4(
				location.Float0 * mipMap.Width,
				location.Float1 * mipMap.Height,
				0, 0);
		}
	}
}