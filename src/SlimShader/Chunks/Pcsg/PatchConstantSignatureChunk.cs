using SlimShader.InputOutputSignature;

namespace SlimShader.Chunks.Pcsg
{
	public class PatchConstantSignatureChunk : InputOutputSignatureChunk
	{
		public override string ToString()
		{
			return @"// Patch Constant signature:
//
" + base.ToString();
		}
	}
}