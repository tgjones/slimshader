#include "PCH.h"
#include "GeometryShaderInputPrimitiveDeclarationToken.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

shared_ptr<GeometryShaderInputPrimitiveDeclarationToken> GeometryShaderInputPrimitiveDeclarationToken::Parse(BytecodeReader& reader)
{
	auto token0 = reader.ReadUInt32();
	auto result = shared_ptr<GeometryShaderInputPrimitiveDeclarationToken>(new GeometryShaderInputPrimitiveDeclarationToken());
	result->_primitive = DecodeValue<Primitive>(token0, 11, 16);
	return result;
}

Primitive GeometryShaderInputPrimitiveDeclarationToken::GetPrimitive() const { return _primitive; }

void GeometryShaderInputPrimitiveDeclarationToken::Print(std::ostream& out) const
{
	out << GetTypeDescription() << " " << ToStringShex(_primitive);
}