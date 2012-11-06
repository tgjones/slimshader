#include "PCH.h"
#include "GeometryShaderInstanceCountDeclarationToken.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

shared_ptr<GeometryShaderInstanceCountDeclarationToken> GeometryShaderInstanceCountDeclarationToken::Parse(BytecodeReader& reader)
{
	auto token0 = reader.ReadUInt32();
	auto result = shared_ptr<GeometryShaderInstanceCountDeclarationToken>(new GeometryShaderInstanceCountDeclarationToken());
	result->_instanceCount = reader.ReadUInt32();
	return result;
}

uint32_t GeometryShaderInstanceCountDeclarationToken::GetInstanceCount() const { return _instanceCount; }

void GeometryShaderInstanceCountDeclarationToken::Print(std::ostream& out) const
{
	
}