#include "PCH.h"
#include "ResourceDeclarationToken.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

shared_ptr<ResourceDeclarationToken> ResourceDeclarationToken::Parse(BytecodeReader& reader)
{
	auto token0 = reader.ReadUInt32();

	auto resourceDimension = DecodeValue<ResourceDimension>(token0, 11, 15);

	uint8_t sampleCount;
	switch (resourceDimension)
	{
	case ResourceDimension::Texture2DMultiSampled:
	case ResourceDimension::Texture2DMultiSampledArray:
		sampleCount = DecodeValue<uint8_t>(token0, 16, 22);
		break;
	default:
		sampleCount = 0;
		break;
	}

	auto operand = Operand::Parse(reader, DecodeValue<OpcodeType>(token0, 0, 10));
	auto returnType = ResourceReturnTypeToken::Parse(reader);

	auto result = shared_ptr<ResourceDeclarationToken>(new ResourceDeclarationToken(operand));
	result->_resourceDimension = resourceDimension;
	result->_sampleCount = sampleCount;
	result->_returnType = returnType;

	return result;
}

ResourceDimension ResourceDeclarationToken::GetResourceDimension() const { return _resourceDimension; }
uint8_t ResourceDeclarationToken::GetSampleCount() const { return _sampleCount; }
const ResourceReturnTypeToken& ResourceDeclarationToken::GetReturnType() const { return _returnType; }

bool ResourceDeclarationToken::IsMultiSampled() const
{
	switch (_resourceDimension)
	{
	case ResourceDimension::Texture2DMultiSampled:
	case ResourceDimension::Texture2DMultiSampledArray:
		return true;
	default:
		return false;
	}
}

void ResourceDeclarationToken::Print(std::ostream& out) const
{
	out << boost::format("%s_%s%s (%s) %s")
		% GetTypeDescription()
		% ToString(_resourceDimension)
		% ((IsMultiSampled()) ? "(" + to_string(_sampleCount) + ")" : "")
		% _returnType
		% GetOperand();
}

ResourceDeclarationToken::ResourceDeclarationToken(Operand operand)
	: DeclarationToken(operand)
{

}