namespace SlimShader.VirtualMachine.Resources
{
    public class SamplerState
    {
        public Filter Filter;
        public TextureAddressMode AddressU;
        public TextureAddressMode AddressV;
        public TextureAddressMode AddressW;
        public float MinimumLod;
        public float MaximumLod;
        public float MipLodBias;
        public int MaximumAnisotropy;
        public Comparison ComparisonFunction;
        public Number4 BorderColor;
    }
}