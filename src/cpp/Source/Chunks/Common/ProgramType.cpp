#include "PCH.h"
#include "ProgramType.h"

using namespace std;
using namespace SlimShader;

string SlimShader::ToString(ProgramType value)
{
	switch (value)
	{
	case ProgramType::PixelShader :
		return "ps";
	case ProgramType::VertexShader :
		return "vs";
	case ProgramType::GeometryShader :
		return "gs";
	case ProgramType::HullShader :
		return "hs";
	case ProgramType::DomainShader :
		return "ds";
	case ProgramType::ComputeShader :
		return "cs";
	default :
		throw runtime_error("Unsupported value: " + to_string((int) value));
	}
}