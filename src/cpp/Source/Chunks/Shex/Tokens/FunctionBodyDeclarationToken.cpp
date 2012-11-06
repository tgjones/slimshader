#include "PCH.h"
#include "FunctionBodyDeclarationToken.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

shared_ptr<FunctionBodyDeclarationToken> FunctionBodyDeclarationToken::Parse(BytecodeReader& reader)
{
	auto token0 = reader.ReadUInt32();
	auto result = shared_ptr<FunctionBodyDeclarationToken>(new FunctionBodyDeclarationToken());
	result->_identifier = reader.ReadUInt32();
	return result;
}

uint32_t FunctionBodyDeclarationToken::GetIdentifier() const { return _identifier; }

void FunctionBodyDeclarationToken::Print(std::ostream& out) const
{
	out << GetTypeDescription() << " fb" << _identifier;
}