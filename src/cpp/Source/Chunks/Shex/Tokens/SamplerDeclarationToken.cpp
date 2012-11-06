#include "PCH.h"
#include "SamplerDeclarationToken.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

shared_ptr<SamplerDeclarationToken> SamplerDeclarationToken::Parse(BytecodeReader& reader)
{
	auto token0 = reader.ReadUInt32();
	auto operand = Operand::Parse(reader, DecodeValue<OpcodeType>(token0, 0, 10));
	auto result = shared_ptr<SamplerDeclarationToken>(new SamplerDeclarationToken(operand));
	result->_samplerMode = DecodeValue<SamplerMode>(token0, 11, 14);
	return result;
}

SamplerMode SamplerDeclarationToken::GetSamplerMode() const { return _samplerMode; }

void SamplerDeclarationToken::Print(std::ostream& out) const
{
	out << GetTypeDescription() << " " << GetOperand() << ", " << ToString(_samplerMode);
}

SamplerDeclarationToken::SamplerDeclarationToken(Operand operand)
	: DeclarationToken(operand)
{

}