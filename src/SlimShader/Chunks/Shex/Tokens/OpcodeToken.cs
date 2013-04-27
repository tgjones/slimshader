namespace SlimShader.Chunks.Shex.Tokens
{
	public abstract class OpcodeToken
	{
		public OpcodeHeader Header { get; internal set; }

		protected string TypeDescription
		{
			get { return Header.OpcodeType.GetDescription(); }
		}

		protected OpcodeToken()
		{
			Header = new OpcodeHeader();
		}

		public override string ToString()
		{
			return TypeDescription;
		}
	}
}