namespace SlimShader.Shader.Tokens
{
	public abstract class ImmediateDeclarationToken : CustomDataToken
	{
		public uint DeclarationLength { get; internal set; }
	}
}