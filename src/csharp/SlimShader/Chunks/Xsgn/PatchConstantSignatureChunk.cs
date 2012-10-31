namespace SlimShader.Chunks.Xsgn
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