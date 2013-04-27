using SharpDX.D3DCompiler;

namespace SlimShader.Compiler
{
	public static class ShaderCompiler
	{
		public static BytecodeContainer CompileFromFile(string fileName, string entryPoint, string profile)
		{
			var compiledShader = ShaderBytecode.CompileFromFile(fileName, entryPoint, profile);
			return BytecodeContainer.Parse(compiledShader.Bytecode.Data);
		}
	}
}