using System.Collections.Generic;
using System.Linq;
using SlimShader.Chunks.Shex.Tokens;

namespace SlimShader.Chunks.Shex
{
	public class RegisterCounts
	{
		private readonly IEnumerable<DeclarationToken> _declarations;

		public uint[] ConstantBuffers
		{
			get
			{
				return _declarations.OfType<ConstantBufferDeclarationToken>()
					.Select(x => (uint) x.Operand.Indices[1].Value)
					.ToArray();
			}
		}

		public int Resources
		{
			get { return _declarations.OfType<ResourceDeclarationToken>().Count(); }
		}

		public int Samplers
		{
			get { return _declarations.OfType<SamplerDeclarationToken>().Count(); }
		}

		public int Inputs
		{
			get { return _declarations.OfType<InputRegisterDeclarationToken>().Count(); }
		}

		public int Outputs
		{
			get { return _declarations.OfType<OutputRegisterDeclarationToken>().Count(); }
		}

		public int Temps
		{
			get { return _declarations.OfType<TempRegisterDeclarationToken>().Count(); }
		}

		public uint[] IndexableTemps
		{
			get
			{
				return _declarations.OfType<IndexableTempRegisterDeclarationToken>()
					.Select(x => x.RegisterCount)
					.ToArray();
			}
		}

		public RegisterCounts(IEnumerable<DeclarationToken> declarations)
		{
			_declarations = declarations;
		}
	}
}