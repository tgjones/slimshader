#include "PCH.h"
#include "StructuredThreadGroupSharedMemoryDeclarationToken.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

shared_ptr<StructuredThreadGroupSharedMemoryDeclarationToken> StructuredThreadGroupSharedMemoryDeclarationToken::Parse(BytecodeReader& reader)
{
	auto token0 = reader.ReadUInt32();
	auto operand = Operand::Parse(reader, DecodeValue<OpcodeType>(token0, 0, 10));
	auto result = shared_ptr<StructuredThreadGroupSharedMemoryDeclarationToken>(new StructuredThreadGroupSharedMemoryDeclarationToken(operand));
	result->_structByteSize = reader.ReadUInt32();
	result->_structCount = reader.ReadUInt32();
	return result;
}

uint32_t StructuredThreadGroupSharedMemoryDeclarationToken::GetStructByteSize() const { return _structByteSize; }

void StructuredThreadGroupSharedMemoryDeclarationToken::Print(std::ostream& out) const
{
	out << GetTypeDescription() << " " << GetOperand() << ", " << _structByteSize << ", " << _structCount;
}

StructuredThreadGroupSharedMemoryDeclarationToken::StructuredThreadGroupSharedMemoryDeclarationToken(Operand operand)
	: DeclarationToken(operand)
{

}