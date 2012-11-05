#include "PCH.h"
#include "SamplerMode.h"

using namespace std;
using namespace SlimShader;

string SlimShader::ToString(SamplerMode value)
{
	switch (value)
	{
	case SamplerMode::Default :
		return "mode_default";
	case SamplerMode::Comparison :
		return "comparison";
	case SamplerMode::Mono :
		return "mono";
	default :
		throw runtime_error("Unsupported value: " + to_string((int) value));
	}
}