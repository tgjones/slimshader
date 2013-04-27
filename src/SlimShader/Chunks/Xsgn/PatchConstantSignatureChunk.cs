using System.Text;

namespace SlimShader.Chunks.Xsgn
{
	public class PatchConstantSignatureChunk : InputOutputSignatureChunk
	{
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine("// Patch Constant signature:");
			sb.AppendLine("//");
			sb.Append(base.ToString());
			return sb.ToString();
		}
	}
}