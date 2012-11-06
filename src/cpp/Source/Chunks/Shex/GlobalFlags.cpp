#include "PCH.h"
#include "GlobalFlags.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

void AppendGlobalFlag(string& result, string value)
{
	if (!result.empty())
		result += " | ";
	result += value;
}

string SlimShader::ToString(GlobalFlags value)
{
	string result;

	if (HasFlag(value, GlobalFlags::RefactoringAllowed))
		AppendGlobalFlag(result, "refactoringAllowed");
	if (HasFlag(value, GlobalFlags::EnableRawAndStructuredBuffersInNonCsShaders))
		AppendGlobalFlag(result, "enableRawAndStructuredBuffers");

	return result;
}