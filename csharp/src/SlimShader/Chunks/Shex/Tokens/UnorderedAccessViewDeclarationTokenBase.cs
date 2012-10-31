namespace SlimShader.Chunks.Shex.Tokens
{
	public abstract class UnorderedAccessViewDeclarationTokenBase : DeclarationToken
	{
		public UnorderedAccessViewCoherency Coherency { get; protected set; }
	}
}