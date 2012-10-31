namespace SlimShader.Chunks.Shex.Tokens
{
	public abstract class ImmediateDeclarationToken : CustomDataToken
	{
		public uint DeclarationLength { get; internal set; }
	}
}