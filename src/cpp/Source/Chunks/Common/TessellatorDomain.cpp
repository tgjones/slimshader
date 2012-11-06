#include "PCH.h"
#include "TessellatorDomain.h"

using namespace std;
using namespace SlimShader;

string SlimShader::ToStringShex(TessellatorDomain value)
{
	switch (value)
	{
	case TessellatorDomain::Isoline :
		return "domain_isoline";
	case TessellatorDomain::Tri :
		return "domain_tri";
	case TessellatorDomain::Quadrilateral :
		return "domain_quad";
	default :
		throw runtime_error("Unsupported value: " + to_string((int) value));
	}
}

string SlimShader::ToStringStat(TessellatorDomain value)
{
	switch (value)
	{
	case TessellatorDomain::Isoline :
		return "Isoline";
	case TessellatorDomain::Tri :
		return "Tri";
	case TessellatorDomain::Quadrilateral :
		return "Quadrilateral";
	default :
		throw runtime_error("Unsupported value: " + to_string((int) value));
	}
}