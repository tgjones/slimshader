#include "PCH.h"
#include "Operand4ComponentName.h"

using namespace std;
using namespace SlimShader;

string SlimShader::ToString(Operand4ComponentName value)
{
	switch (value)
	{
	case Operand4ComponentName::X :
		return "x";
	case Operand4ComponentName::Y :
		return "y";
	case Operand4ComponentName::Z :
		return "z";
	case Operand4ComponentName::W :
		return "w";
	default :
		throw runtime_error("Unsupported value: " + to_string((int) value));
	}
}