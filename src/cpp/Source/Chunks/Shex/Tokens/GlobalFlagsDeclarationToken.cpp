#include "PCH.h"
#include "GlobalFlagsDeclarationToken.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

shared_ptr<GlobalFlagsDeclarationToken> GlobalFlagsDeclarationToken::Parse(BytecodeReader& reader)
{
	auto token0 = reader.ReadUInt32();
	auto result = shared_ptr<GlobalFlagsDeclarationToken>(new GlobalFlagsDeclarationToken());
	result->_flags = DecodeValue<GlobalFlags>(token0, 11, 18);
	return result;
};

GlobalFlags GlobalFlagsDeclarationToken::GetFlags() const { return _flags; }

void GlobalFlagsDeclarationToken::Print(std::ostream& out) const
{
	out << GetTypeDescription() << " " << ToString(_flags);
};