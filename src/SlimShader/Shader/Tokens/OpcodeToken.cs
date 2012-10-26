namespace SlimShader.Shader.Tokens
{
	public abstract class OpcodeToken
	{
		public OpcodeHeader Header { get; internal set; }

		protected string TypeDescription
		{
			get { return Header.OpcodeType.GetDescription(); }
		}

		public override string ToString()
		{
			return TypeDescription;
		}
	}
}