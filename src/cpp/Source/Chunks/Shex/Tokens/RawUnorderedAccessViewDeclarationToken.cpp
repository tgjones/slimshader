#include "PCH.h"
#include "RawUnorderedAccessViewDeclarationToken.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

shared_ptr<RawUnorderedAccessViewDeclarationToken> RawUnorderedAccessViewDeclarationToken::Parse(BytecodeReader& reader)
{
	auto token0 = reader.ReadUInt32();
	auto coherency = DecodeValue<UnorderedAccessViewCoherency>(token0, 16, 16);
	auto operand = Operand::Parse(reader, DecodeValue<OpcodeType>(token0, 0, 10));
	auto result = shared_ptr<RawUnorderedAccessViewDeclarationToken>(new RawUnorderedAccessViewDeclarationToken(coherency, operand));
	return result;
}

void RawUnorderedAccessViewDeclarationToken::Print(ostream& out) const
{
	out << GetTypeDescription() << " " << GetOperand();
}

RawUnorderedAccessViewDeclarationToken::RawUnorderedAccessViewDeclarationToken(UnorderedAccessViewCoherency coherency, Operand operand)
	: UnorderedAccessViewDeclarationTokenBase(coherency, operand)
{

}