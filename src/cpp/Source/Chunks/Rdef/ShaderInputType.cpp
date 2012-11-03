#include "PCH.h"
#include "ShaderInputType.h"

using namespace std;
using namespace SlimShader;

string SlimShader::ToString(ShaderInputType value)
{
	switch (value)
	{
	case ShaderInputType::CBuffer :
		return "cbuffer";
	case ShaderInputType::Texture:
	case ShaderInputType::Structured:
	case ShaderInputType::ByteAddress:
		return "texture";
	case ShaderInputType::Sampler:
		return "sampler";
	case ShaderInputType::UavRwTyped:
	case ShaderInputType::UavRwStructured:
	case ShaderInputType::UavRwByteAddress:
	case ShaderInputType::UavAppendStructured:
	case ShaderInputType::UavConsumeStructured:
	case ShaderInputType::UavRwStructuredWithCounter:
		return "UAV";
	default:
		throw runtime_error("Shader input type '" + to_string((int) value) + "' is not supported.");
	}
}