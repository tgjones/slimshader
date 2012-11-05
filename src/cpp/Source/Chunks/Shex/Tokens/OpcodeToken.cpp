#include "PCH.h"
#include "OpcodeToken.h"

using namespace std;
using namespace SlimShader;

ostream& SlimShader::operator<<(ostream& out, const OpcodeToken& value)
{
	value.Print(out);
	return out;
}