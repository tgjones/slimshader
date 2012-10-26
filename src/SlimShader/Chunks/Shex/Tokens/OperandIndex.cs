namespace SlimShader.Chunks.Shex.Tokens
{
	public class OperandIndex
	{
		public ulong Value { get; set; }
		public Operand Register { get; set; }

		public override string ToString()
		{
			string result = string.Empty;
			if (Register != null)
				result += Register + " + ";
			result += Value;
			return result;
		}
	}
}