using SlimShader;
using SlimShader.VirtualMachine.Resources;

namespace HlslUnit
{
    internal class FakeTexture : ITexture
    {
        private readonly ResourceCallback<Number4> _callback;

        public TextureDimension Dimension
        {
            get { return TextureDimension.Texture2D; }
        }

        public int MipMapCount
        {
            get { return 1; }
        }

        public FakeTexture(ResourceCallback<Number4> callback)
        {
            _callback = callback;
        }

        public ITextureMipMap GetMipMap(int arraySlice, int mipLevel)
        {
            return new FakeTextureMipMap(_callback);
        }

        private class FakeTextureMipMap : ITextureMipMap
        {
            private readonly ResourceCallback<Number4> _callback;

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

            public FakeTextureMipMap(ResourceCallback<Number4> callback)
            {
                _callback = callback;
            }

            public Number4 GetData(ref Number4 coords)
            {
                return _callback(coords.X, coords.Y, coords.Z, coords.W);
            }
        }
    }
}