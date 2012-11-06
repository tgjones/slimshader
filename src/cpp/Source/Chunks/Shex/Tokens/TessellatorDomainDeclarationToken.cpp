#include "PCH.h"
#include "TessellatorDomainDeclarationToken.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

shared_ptr<TessellatorDomainDeclarationToken> TessellatorDomainDeclarationToken::Parse(BytecodeReader& reader)
{
	auto token0 = reader.ReadUInt32();
	auto result = shared_ptr<TessellatorDomainDeclarationToken>(new TessellatorDomainDeclarationToken());
	result->_domain = DecodeValue<TessellatorDomain>(token0, 11, 12);
	return result;
};

TessellatorDomain TessellatorDomainDeclarationToken::GetDomain() const { return _domain; }

void TessellatorDomainDeclarationToken::Print(std::ostream& out) const
{
	out << GetTypeDescription() << " " << ToStringShex(_domain);
};