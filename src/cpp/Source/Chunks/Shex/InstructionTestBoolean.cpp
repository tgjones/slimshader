#include "PCH.h"
#include "InstructionTestBoolean.h"

using namespace std;
using namespace SlimShader;

string SlimShader::ToString(InstructionTestBoolean value)
{
	switch (value)
	{
	case InstructionTestBoolean::Zero :
		return "z";
	case InstructionTestBoolean::NonZero :
		return "nz";
	default :
		throw runtime_error("Unsupported value: " + to_string((int) value));
	}
}