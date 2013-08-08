using System;

namespace SlimShader.VirtualMachine.Resources
{
    internal static class TextureSamplerFactory
    {
        public static TextureSampler Create(TextureDimension dimension)
        {
            switch (dimension)
            {
                case TextureDimension.Texture1D:
                    throw new NotImplementedException();
                case TextureDimension.Texture1DArray:
                    throw new NotImplementedException();
                case TextureDimension.Texture2D:
                    return new Texture2DSampler();
                case TextureDimension.Texture2DArray:
                    throw new NotImplementedException();
                case TextureDimension.Texture3D:
                    throw new NotImplementedException();break;
                case TextureDimension.TextureCube:
                    throw new NotImplementedException();
                case TextureDimension.TextureCubeArray:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException("dimension");
            }
        }
    }
}