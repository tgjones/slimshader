#include "PCH.h"
#include "GeometryShaderMaxOutputVertexCountDeclarationToken.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

shared_ptr<GeometryShaderMaxOutputVertexCountDeclarationToken> GeometryShaderMaxOutputVertexCountDeclarationToken::Parse(BytecodeReader& reader)
{
	auto token0 = reader.ReadUInt32();
	auto result = shared_ptr<GeometryShaderMaxOutputVertexCountDeclarationToken>(new GeometryShaderMaxOutputVertexCountDeclarationToken());
	result->_maxPrimitives = reader.ReadUInt32();
	return result;
}

uint32_t GeometryShaderMaxOutputVertexCountDeclarationToken::GetMaxPrimitives() const { return _maxPrimitives; }

void GeometryShaderMaxOutputVertexCountDeclarationToken::Print(std::ostream& out) const
{
	out << GetTypeDescription() << " " << _maxPrimitives;
}