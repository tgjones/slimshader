#include "PCH.h"
#include "RegisterComponentType.h"

using namespace std;
using namespace SlimShader;

string SlimShader::ToString(RegisterComponentType value)
{
	switch (value)
	{
	case RegisterComponentType::UInt32 :
		return "uint";
	case RegisterComponentType::SInt32 :
		return "int";
	case RegisterComponentType::Float32 :
		return "float";
	default :
		throw runtime_error("Unsupported value: " + to_string((int) value));
	}
}