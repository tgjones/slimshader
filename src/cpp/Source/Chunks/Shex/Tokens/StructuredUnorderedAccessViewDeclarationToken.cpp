#include "PCH.h"
#include "StructuredUnorderedAccessViewDeclarationToken.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

shared_ptr<StructuredUnorderedAccessViewDeclarationToken> StructuredUnorderedAccessViewDeclarationToken::Parse(BytecodeReader& reader)
{
	auto token0 = reader.ReadUInt32();
	auto coherency = DecodeValue<UnorderedAccessViewCoherency>(token0, 16, 16);
	auto operand = Operand::Parse(reader, DecodeValue<OpcodeType>(token0, 0, 10));
	auto result = shared_ptr<StructuredUnorderedAccessViewDeclarationToken>(new StructuredUnorderedAccessViewDeclarationToken(coherency, operand));
	result->_hasOrderPreservingCounter = (DecodeValue(token0, 23, 23) == 0),
	result->_byteStride = reader.ReadUInt32();
	return result;
}

bool StructuredUnorderedAccessViewDeclarationToken::HasOrderPreservingCounter() const { return _hasOrderPreservingCounter; }
uint32_t StructuredUnorderedAccessViewDeclarationToken::GetByteStride() const { return _byteStride; }

void StructuredUnorderedAccessViewDeclarationToken::Print(std::ostream& out) const
{
	out << GetTypeDescription() << " " << GetOperand() << ", " << _byteStride;
}

StructuredUnorderedAccessViewDeclarationToken::StructuredUnorderedAccessViewDeclarationToken(UnorderedAccessViewCoherency coherency, Operand operand)
	: UnorderedAccessViewDeclarationTokenBase(coherency, operand)
{

}