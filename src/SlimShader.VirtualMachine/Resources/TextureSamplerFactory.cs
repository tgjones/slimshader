using System;
using SlimShader.Chunks.Shex;

namespace SlimShader.VirtualMachine.Resources
{
    internal static class TextureSamplerFactory
    {
        public static TextureSampler Create(ResourceDimension dimension)
        {
            switch (dimension)
            {
				case ResourceDimension.Texture1D:
                    throw new NotImplementedException();
				case ResourceDimension.Texture1DArray:
                    throw new NotImplementedException();
				case ResourceDimension.Texture2D:
                    return new Texture2DSampler();
				case ResourceDimension.Texture2DArray:
					return new Texture2DArraySampler();
				case ResourceDimension.Texture3D:
                    throw new NotImplementedException();
				case ResourceDimension.TextureCube:
					return new TextureCubeSampler();
				case ResourceDimension.TextureCubeArray:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException("dimension");
            }
        }
    }
}