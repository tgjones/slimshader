#include "PCH.h"
#include "StructuredShaderResourceViewDeclarationToken.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

shared_ptr<StructuredShaderResourceViewDeclarationToken> StructuredShaderResourceViewDeclarationToken::Parse(BytecodeReader& reader)
{
	auto token0 = reader.ReadUInt32();
	auto operand = Operand::Parse(reader, DecodeValue<OpcodeType>(token0, 0, 10));
	auto result = shared_ptr<StructuredShaderResourceViewDeclarationToken>(new StructuredShaderResourceViewDeclarationToken(operand));
	result->_structByteSize = reader.ReadUInt32();
	return result;
}

uint32_t StructuredShaderResourceViewDeclarationToken::GetStructByteSize() const { return _structByteSize; }

void StructuredShaderResourceViewDeclarationToken::Print(std::ostream& out) const
{
	out << GetTypeDescription() << " " << GetOperand() << ", " << _structByteSize;
}

StructuredShaderResourceViewDeclarationToken::StructuredShaderResourceViewDeclarationToken(Operand operand)
	: DeclarationToken(operand)
{

}