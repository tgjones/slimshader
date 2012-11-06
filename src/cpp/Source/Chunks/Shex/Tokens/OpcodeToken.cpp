#include "PCH.h"
#include "OpcodeToken.h"

using namespace std;
using namespace SlimShader;

const OpcodeHeader& OpcodeToken::GetHeader() const { return _header; }
void OpcodeToken::SetHeader(OpcodeHeader header) { _header = header; }

ostream& SlimShader::operator<<(ostream& out, const OpcodeToken& value)
{
	value.Print(out);
	return out;
}

string OpcodeToken::GetTypeDescription() const
{
	return ToString(_header.OpcodeType);
}

void OpcodeToken::Print(std::ostream& out) const
{
	out << GetTypeDescription();
}