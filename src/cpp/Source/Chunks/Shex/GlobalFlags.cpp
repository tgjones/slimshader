#include "PCH.h"
#include "GlobalFlags.h"

using namespace std;
using namespace SlimShader;

string SlimShader::ToString(GlobalFlags value)
{
	switch (value)
	{
	case GlobalFlags::RefactoringAllowed :
		return "refactoringAllowed";
	case GlobalFlags::EnableRawAndStructuredBuffersInNonCsShaders :
		return "enableRawAndStructuredBuffers";
	default :
		throw runtime_error("Unsupported value: " + to_string((int) value));
	}
}