#include "PCH.h"
#include "ShaderVariableClass.h"

using namespace std;
using namespace SlimShader;

string SlimShader::ToString(ShaderVariableClass value)
{
	switch (value)
	{
	case ShaderVariableClass::Vector :
		return "";
	case ShaderVariableClass::MatrixRows :
		return "row_major ";
	case ShaderVariableClass::MatrixColumns :
		return "";
	case ShaderVariableClass::InterfacePointer :
		return "interface ";
	default:
		throw runtime_error("Shader variable class '" + to_string((int) value) + "' is not supported.");
	}
}