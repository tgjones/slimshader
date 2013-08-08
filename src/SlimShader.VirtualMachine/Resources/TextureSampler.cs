using System;
using SlimShader.VirtualMachine.Util;

namespace SlimShader.VirtualMachine.Resources
{
    internal abstract class TextureSampler
    {
        public abstract float CalculateLevelOfDetail(
            ITexture texture, SamplerState samplerState,
            ref Number4 ddx, ref Number4 ddy);

        public Number4 SampleGrad(
            ITexture texture, SamplerState samplerState,
            ref Number4 location,
            ref Number4 ddx, ref Number4 ddy)
        {
            var lod = CalculateLevelOfDetail(texture, samplerState, ref ddx, ref ddy);
            return SampleLevel(texture, samplerState, ref location, lod);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="samplerState"></param>
        /// <param name="location"></param>
        /// <param name="lod">A number that specifies the mipmap level. If the value is &lt;=0, the zero'th (biggest map)
        /// is used. The fractional value (if supplied) is used to interpolate between two mipmap levels.</param>
        /// <returns></returns>
        public Number4 SampleLevel(
            ITexture texture, SamplerState samplerState,
            ref Number4 location,
            float lod)
        {
            // TODO: Don't always pass minifying=true to GetFilteredColor.

            switch (samplerState.Filter)
            {
                case Filter.MinPointMagLinearMipPoint:
                case Filter.MinLinearMagMipPoint:
                case Filter.MinMagLinearMipPoint:
                case Filter.MinMagMipPoint:
                {
                    // Calculate nearest mipmap level.
                    var nearestLevel = MathUtility.Round(lod);
                    return GetFilteredColor(texture, samplerState, true, nearestLevel, ref location);
                }
                case Filter.MinLinearMagPointMipLinear:
                case Filter.MinMagMipLinear:
                case Filter.MinMagPointMipLinear:
                case Filter.MinPointMagMipLinear:
                {
                    // Calculate nearest two levels and linearly filter between them.
                    var nearestLevelInt = (int) lod;
                    var d = lod - nearestLevelInt;
                    var c1 = GetFilteredColor(texture, samplerState, true, nearestLevelInt, ref location);
                    var c2 = GetFilteredColor(texture, samplerState, true, nearestLevelInt + 1, ref location);
                    return Number4.Lerp(ref c1, ref c2, d);
                }
                default:
                    throw new NotSupportedException();
            }
        }

        private Number4 GetFilteredColor(
            ITexture texture, SamplerState samplerState, bool minifying, int level,
            ref Number4 location)
        {
            var clampedLevel = MathUtility.Clamp(level, 0, texture.MipMapCount - 1);

            ITextureMipMap mipMap;
            Number4 texCoords;
            GetMipMapAndTransformedCoordinates(
                texture, ref location, clampedLevel,
                out mipMap, out texCoords);

            // Minifying
            if (minifying)
                switch (samplerState.Filter)
                {
                    case Filter.MinMagMipPoint:
                    case Filter.MinMagPointMipLinear:
                    case Filter.MinPointMagLinearMipPoint:
                    case Filter.MinPointMagMipLinear:
                        return GetNearestNeighbor(samplerState, mipMap, ref texCoords);
                    case Filter.MinLinearMagMipPoint:
                    case Filter.MinLinearMagPointMipLinear:
                    case Filter.MinMagLinearMipPoint:
                    case Filter.MinMagMipLinear:
                        return GetLinear(samplerState, mipMap, ref texCoords);
                    default:
                        throw new NotSupportedException();
                }

            // Magnifying
            switch (samplerState.Filter)
            {
                case Filter.MinLinearMagMipPoint:
                case Filter.MinLinearMagPointMipLinear:
                case Filter.MinMagMipPoint:
                case Filter.MinMagPointMipLinear:
                    return GetNearestNeighbor(samplerState, mipMap, ref texCoords);
                case Filter.MinMagLinearMipPoint:
                case Filter.MinMagMipLinear:
                case Filter.MinPointMagLinearMipPoint:
                case Filter.MinPointMagMipLinear:
                    return GetLinear(samplerState, mipMap, ref texCoords);
                default:
                    throw new NotSupportedException();
            }
        }

        protected abstract void GetMipMapAndTransformedCoordinates(
            ITexture texture, ref Number4 location, int level,
            out ITextureMipMap mipMap,
            out Number4 textureCoordinates);

        protected abstract Number4 GetNearestNeighbor(
            SamplerState samplerState, ITextureMipMap mipMap, 
            ref Number4 texCoords);

        protected abstract Number4 GetLinear(
            SamplerState samplerState, ITextureMipMap mipMap, 
            ref Number4 texCoords);

        protected static bool GetTextureAddress(int value, int maxValue, 
            TextureAddressMode textureAddressMode, out int result)
        {
            // If value is in the valid texture address range, return it straight away.
            if (value >= 0 && value < maxValue)
            {
                result = value;
                return true;
            }

            // Otherwise, we need to use the specified addressing mode.
            switch (textureAddressMode)
            {
                case TextureAddressMode.Border:
                    result = 0;
                    return false;
                case TextureAddressMode.Clamp:
                    result = (value < 0) ? 0 : maxValue - 1;
                    return true;
                case TextureAddressMode.Mirror:
                {
                    int temp = value % (2 * maxValue);
                    if (temp < 0)
                        temp += (2 * maxValue);
                    result = (temp > maxValue) ? (2 * maxValue) - temp : temp;
                    return true;
                }
                case TextureAddressMode.Wrap:
                {
                    int temp = value % maxValue;
                    if (temp < 0)
                        temp += maxValue;
                    result = temp;
                    return true;
                }
                default:
                    throw new ArgumentOutOfRangeException("textureAddressMode");
            }
        }
    }
}