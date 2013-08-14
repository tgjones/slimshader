using System.Collections.ObjectModel;
using System.Linq;

namespace SlimShader.Chunks.Xsgn
{
	public class SignatureParameterDescriptionCollection : Collection<SignatureParameterDescription>
	{
		public uint? FindRegister(string semanticName, uint semanticIndex)
		{
			var parameter = this.SingleOrDefault(x => x.SemanticName == semanticName && x.SemanticIndex == semanticIndex);
            if (parameter == null)
                return null;
			return parameter.Register;
		}
	}
}