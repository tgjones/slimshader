#include "PCH.h"
#include "GeometryShaderOutputPrimitiveTopologyDeclarationToken.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

shared_ptr<GeometryShaderOutputPrimitiveTopologyDeclarationToken> GeometryShaderOutputPrimitiveTopologyDeclarationToken::Parse(BytecodeReader& reader)
{
	auto token0 = reader.ReadUInt32();
	auto result = shared_ptr<GeometryShaderOutputPrimitiveTopologyDeclarationToken>(new GeometryShaderOutputPrimitiveTopologyDeclarationToken());
	result->_primitiveTopology = DecodeValue<PrimitiveTopology>(token0, 11, 17);
	return result;
}

PrimitiveTopology GeometryShaderOutputPrimitiveTopologyDeclarationToken::GetPrimitiveTopology() const { return _primitiveTopology; }

void GeometryShaderOutputPrimitiveTopologyDeclarationToken::Print(std::ostream& out) const
{
	out << GetTypeDescription() << " " << ToStringShex(_primitiveTopology);
}