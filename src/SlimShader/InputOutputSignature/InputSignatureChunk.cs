using System.Linq;
using System.Text;

namespace SlimShader.InputOutputSignature
{
	public class InputSignatureChunk : InputOutputSignatureChunk
	{
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine("// Input signature:");
			sb.AppendLine("//");
			sb.Append(base.ToString());

			if (!Parameters.Any())
				sb.AppendLine("// no Input");

			return sb.ToString();
		}
	}
}