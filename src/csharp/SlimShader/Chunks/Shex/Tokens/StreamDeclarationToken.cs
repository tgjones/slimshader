using SlimShader.Util;

namespace SlimShader.Chunks.Shex.Tokens
{
	/// <summary>
	/// Stream Declaration
	/// </summary>
	public class StreamDeclarationToken : DeclarationToken
	{
		public static StreamDeclarationToken Parse(BytecodeReader reader)
		{
			var token0 = reader.ReadUInt32();
			return new StreamDeclarationToken
			{
				Operand = Operand.Parse(reader, token0.DecodeValue<OpcodeType>(0, 10))
			};
		}

		public override string ToString()
		{
			return string.Format("{0} {1}", TypeDescription, Operand);
		}
	}
}