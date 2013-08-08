namespace SlimShader.VirtualMachine.Resources
{
    public interface ITextureMipMap
    {
        int Width { get; }
        int Height { get; }
        int Depth { get; }

        Number4 GetData(ref Number4 coords);
    }
}