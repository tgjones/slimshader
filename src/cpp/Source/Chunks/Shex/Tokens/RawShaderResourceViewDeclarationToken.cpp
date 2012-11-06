#include "PCH.h"
#include "RawShaderResourceViewDeclarationToken.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

shared_ptr<RawShaderResourceViewDeclarationToken> RawShaderResourceViewDeclarationToken::Parse(BytecodeReader& reader)
{
	auto token0 = reader.ReadUInt32();
	auto operand = Operand::Parse(reader, DecodeValue<OpcodeType>(token0, 0, 10));
	return shared_ptr<RawShaderResourceViewDeclarationToken>(new RawShaderResourceViewDeclarationToken(operand));
}

void RawShaderResourceViewDeclarationToken::Print(ostream& out) const
{
	out << GetTypeDescription() << " " << GetOperand();
}

RawShaderResourceViewDeclarationToken::RawShaderResourceViewDeclarationToken(Operand operand)
	: DeclarationToken(operand)
{

}