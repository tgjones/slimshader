using SharpDX.D3DCompiler;

namespace HlslUnit.Tests
{
    internal static class ShaderTestUtility
    {
        public static byte[] CompileShader(
            string fileName, string entryPoint, string profile,
            ShaderFlags shaderFlags = ShaderFlags.None)
        {
            var compiledShader = ShaderBytecode.CompileFromFile(
                fileName, entryPoint, profile, shaderFlags);
            return compiledShader.Bytecode.Data;
        }
    }
}