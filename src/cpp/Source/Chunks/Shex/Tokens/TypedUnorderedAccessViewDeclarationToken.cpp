#include "PCH.h"
#include "TypedUnorderedAccessViewDeclarationToken.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

shared_ptr<TypedUnorderedAccessViewDeclarationToken> TypedUnorderedAccessViewDeclarationToken::Parse(BytecodeReader& reader)
{
	auto token0 = reader.ReadUInt32();
	auto coherency = DecodeValue<UnorderedAccessViewCoherency>(token0, 16, 16);
	auto operand = Operand::Parse(reader, DecodeValue<OpcodeType>(token0, 0, 10));
	auto result = shared_ptr<TypedUnorderedAccessViewDeclarationToken>(new TypedUnorderedAccessViewDeclarationToken(coherency, operand));
	result->_resourceDimension = DecodeValue<ResourceDimension>(token0, 11, 15);
	result->_returnType = ResourceReturnTypeToken::Parse(reader);
	return result;
}

ResourceDimension TypedUnorderedAccessViewDeclarationToken::GetResourceDimension() const { return _resourceDimension; }
const ResourceReturnTypeToken& TypedUnorderedAccessViewDeclarationToken::GetReturnType() const { return _returnType; }

void TypedUnorderedAccessViewDeclarationToken::Print(std::ostream& out) const
{
	out << GetTypeDescription() << " " << GetOperand();
}

TypedUnorderedAccessViewDeclarationToken::TypedUnorderedAccessViewDeclarationToken(UnorderedAccessViewCoherency coherency, Operand operand)
	: UnorderedAccessViewDeclarationTokenBase(coherency, operand)
{

}