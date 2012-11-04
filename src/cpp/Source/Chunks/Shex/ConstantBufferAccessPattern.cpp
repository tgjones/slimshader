#include "PCH.h"
#include "ConstantBufferAccessPattern.h"

using namespace std;
using namespace SlimShader;

string SlimShader::ToString(ConstantBufferAccessPattern value)
{
	switch (value)
	{
	case ConstantBufferAccessPattern::ImmediateIndexed :
		return "immediateIndexed";
	case ConstantBufferAccessPattern::DynamicIndexed :
		return "dynamicIndexed";
	default :
		throw runtime_error("Unsupported value: " + to_string((int) value));
	}
}