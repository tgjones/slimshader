#include "PCH.h"
#include "ShaderVariableType.h"

using namespace std;
using namespace SlimShader;

string SlimShader::ToString(ShaderVariableType value)
{
	switch (value)
	{
	case ShaderVariableType::Bool :
		return "bool";
	case ShaderVariableType::Int :
		return "int";
	case ShaderVariableType::Float :
		return "float";
	case ShaderVariableType::UInt :
		return "uint";
	case ShaderVariableType::InterfacePointer :
		return "interface";
	default:
		throw runtime_error("Shader variable type '" + to_string((int) value) + "' is not supported.");
	}
}