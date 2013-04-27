using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SlimShader.Chunks.Xsgn
{
	public class SignatureParameterDescriptionCollection : Collection<SignatureParameterDescription>
	{
		public uint FindRegister(string semanticName, uint semanticIndex)
		{
			var parameter = this.SingleOrDefault(x => x.SemanticName == semanticName && x.SemanticIndex == semanticIndex);
			if (parameter == null)
				throw new Exception(string.Format("No matching input parameter for semantic name '{0}' and index '{1}'.",
					semanticName, semanticIndex));
			return parameter.Register;
		}
	}
}