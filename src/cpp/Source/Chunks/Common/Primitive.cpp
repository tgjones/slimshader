#include "PCH.h"
#include "Primitive.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

string SlimShader::ToStringShex(Primitive value)
{
	switch (value)
	{
	case Primitive::Point :
		return "point";
	case Primitive::Line :
		return "line";
	case Primitive::Triangle :
		return "triangle";
	default :
		throw runtime_error("Unsupported value: " + to_string((int) value));
	}
}