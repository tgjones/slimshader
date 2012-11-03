#include "PCH.h"
#include "ResourceReturnType.h"

using namespace std;
using namespace SlimShader;

string SlimShader::ToString(ResourceReturnType value)
{
	switch (value)
	{
	case ResourceReturnType::NotApplicable :
		return "NA";
	case ResourceReturnType::UNorm :
		return "unorm";
	case ResourceReturnType::SNorm :
		return "snorm";
	case ResourceReturnType::SInt :
		return "sint";
	case ResourceReturnType::UInt :
		return "uint";
	case ResourceReturnType::Float :
		return "float";
	case ResourceReturnType::Double :
		return "double";
	default :
		throw runtime_error("Unsupported value: " + to_string((int) value));
	}
}