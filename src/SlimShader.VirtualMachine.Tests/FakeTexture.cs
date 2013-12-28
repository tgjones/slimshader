using SlimShader.VirtualMachine.Resources;

namespace SlimShader.VirtualMachine.Tests
{
    public class FakeTexture : ITexture
    {
        private readonly Number4[] _mipColors;

        public TextureDimension Dimension
        {
            get { return TextureDimension.Texture2D; }
        }

        public int MipMapCount
        {
            get { return _mipColors.Length; }
        }

        public FakeTexture(params Number4[] mipColors)
        {
            _mipColors = mipColors;
        }

        public ITextureMipMap GetMipMap(int arraySlice, int mipLevel)
        {
			return new FakeTextureMipMap(_mipColors[mipLevel]);
        }

        private class FakeTextureMipMap : ITextureMipMap
        {
            private readonly Number4 _data;

            public int Width
            {
                get { return 1; }
            }

            public int Height
            {
                get { return 1; }
            }

            public int Depth
            {
                get { return 0; }
            }

            public FakeTextureMipMap(Number4 data)
            {
                _data = data;
            }

            public Number4 GetData(ref Number4 coords)
            {
                return _data;
            }
        }
    }
}