#include "PCH.h"
#include "IndexingRangeDeclarationToken.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

shared_ptr<IndexingRangeDeclarationToken> IndexingRangeDeclarationToken::Parse(BytecodeReader& reader)
{
	auto token0 = reader.ReadUInt32();
	auto operand = Operand::Parse(reader, DecodeValue<OpcodeType>(token0, 0, 10));
	auto result = shared_ptr<IndexingRangeDeclarationToken>(new IndexingRangeDeclarationToken(operand));
	result->_registerCount = reader.ReadUInt32();
	return result;
}

uint32_t IndexingRangeDeclarationToken::GetRegisterCount() const { return _registerCount; }

void IndexingRangeDeclarationToken::Print(std::ostream& out) const
{
	out << GetTypeDescription() << " " << GetOperand() << " " << _registerCount;
}

IndexingRangeDeclarationToken::IndexingRangeDeclarationToken(Operand operand)
	: DeclarationToken(operand)
{

}