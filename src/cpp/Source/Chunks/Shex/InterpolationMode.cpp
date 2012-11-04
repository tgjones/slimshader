#include "PCH.h"
#include "InterpolationMode.h"

using namespace std;
using namespace SlimShader;

string SlimShader::ToString(InterpolationMode value)
{
	switch (value)
	{
	case InterpolationMode::Constant :
		return "constant";
	case InterpolationMode::Linear :
		return "linear";
	case InterpolationMode::LinearCentroid :
		return "linear centroid";
	case InterpolationMode::LinearNoPerspective :
		return "linear noperspective";
	case InterpolationMode::LinearNoPerspectiveCentroid :
		return "linear noperspective centroid";
	default :
		throw runtime_error("Unsupported value: " + to_string((int) value));
	}
}