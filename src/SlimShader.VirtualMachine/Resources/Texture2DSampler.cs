using System;
using SlimShader.VirtualMachine.Util;

namespace SlimShader.VirtualMachine.Resources
{
    internal class Texture2DSampler : TextureSampler
    {
        public override float CalculateLevelOfDetail(
            ITexture texture, SamplerState samplerState, 
            ref Number4 ddx, ref Number4 ddy)
        {
            var mostDetailedMipMap = texture.GetMipMap(0);
            int width = mostDetailedMipMap.Width;
            int height = mostDetailedMipMap.Height;
            float xBound2 = width * width;
            float yBound2 = height * height;

            float dudx2 = ddx.Float0 * ddx.Float0 * xBound2;
            float dvdx2 = ddx.Float1 * ddx.Float1 * yBound2;
            float dudy2 = ddy.Float0 * ddy.Float0 * xBound2;
            float dvdy2 = ddy.Float1 * ddy.Float1 * yBound2;

            // Proportional to the amount of a texel on display in a single pixel
            float pixelSizeTexelRatio2 = Math.Max(dudx2 + dvdx2, dudy2 + dvdy2);

            // Uses formula for p410 of Essential Mathematics for Games and Interactive Applications
            float result = 0.5f * MathUtility.Log2(pixelSizeTexelRatio2);

            // Clamp to >= 0.
            return Math.Max(result, 0.0f);
        }

        protected override void GetMipMapAndTransformedCoordinates(
            ITexture texture, ref Number4 location, int level,
            out ITextureMipMap mipMap,
            out Number4 textureCoordinates)
        {
            mipMap = texture.GetMipMap(level);
            textureCoordinates = new Number4(
                location.Float0 * mipMap.Width,
                location.Float1 * mipMap.Height,
                0, 0);
        }

        protected override Number4 GetNearestNeighbor(
            SamplerState samplerState, ITextureMipMap mipMap, 
            ref Number4 texCoords)
        {
            texCoords.Int0 = MathUtility.Floor(texCoords.Float0);
            texCoords.Int1 = MathUtility.Floor(texCoords.Float1);
            return GetColor(samplerState, mipMap, ref texCoords);
        }

        protected override Number4 GetLinear(
            SamplerState samplerState, ITextureMipMap mipMap, 
            ref Number4 texCoords)
        {
            int intTexelX = (int) texCoords.Float0;
            int intTexelY = (int) texCoords.Float1;

            float fracX = texCoords.Float0 - intTexelX;
            float fracY = texCoords.Float1 - intTexelY;

            texCoords.Int0 = intTexelX;
            texCoords.Int1 = intTexelY;
            var c00 = GetColor(samplerState, mipMap, ref texCoords);

            texCoords.Int0 = intTexelX + 1;
            texCoords.Int1 = intTexelY;
            var c10 = GetColor(samplerState, mipMap, ref texCoords);

            texCoords.Int0 = intTexelX;
            texCoords.Int1 = intTexelY + 1;
            var c01 = GetColor(samplerState, mipMap, ref texCoords);

            texCoords.Int0 = intTexelX + 1;
            texCoords.Int1 = intTexelY + 1;
            var c11 = GetColor(samplerState, mipMap, ref texCoords);

            var cMinV = Number4.Lerp(ref c00, ref c10, fracX);
            var cMaxV = Number4.Lerp(ref c01, ref c11, fracX);

            return Number4.Lerp(ref cMinV, ref cMaxV, fracY);
        }

        private static Number4 GetColor(SamplerState samplerState, ITextureMipMap mipMap, ref Number4 texCoords)
        {
            if (!GetTextureAddress(texCoords.Int0, mipMap.Width, samplerState.AddressU, out texCoords.Int0))
                return samplerState.BorderColor;
            if (!GetTextureAddress(texCoords.Int1, mipMap.Height, samplerState.AddressV, out texCoords.Int1))
                return samplerState.BorderColor;

            return mipMap.GetData(ref texCoords);
        }
    }
}