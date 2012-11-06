#include "PCH.h"
#include "ResourceReturnTypeToken.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

ResourceReturnTypeToken ResourceReturnTypeToken::Parse(BytecodeReader& reader)
{
	auto token = reader.ReadUInt32();
	ResourceReturnTypeToken result;
	result._x = DecodeValue<ResourceReturnType>(token, 0, 3);
	result._y = DecodeValue<ResourceReturnType>(token, 4, 7);
	result._z = DecodeValue<ResourceReturnType>(token, 8, 11);
	result._w = DecodeValue<ResourceReturnType>(token, 12, 15);
	return result;
}

ResourceReturnType ResourceReturnTypeToken::GetX() const { return _x; }
ResourceReturnType ResourceReturnTypeToken::GetY() const { return _y; }
ResourceReturnType ResourceReturnTypeToken::GetZ() const { return _z; }
ResourceReturnType ResourceReturnTypeToken::GetW() const { return _w; }

ostream& SlimShader::operator<<(ostream& out, const ResourceReturnTypeToken& value)
{
	out << boost::format("%s,%s,%s,%s")
		% ToString(value._x)
		% ToString(value._y)
		% ToString(value._z)
		% ToString(value._w);
	return out;
}