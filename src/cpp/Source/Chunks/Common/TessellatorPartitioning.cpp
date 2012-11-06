#include "PCH.h"
#include "TessellatorPartitioning.h"

using namespace std;
using namespace SlimShader;

string SlimShader::ToStringShex(TessellatorPartitioning value)
{
	switch (value)
	{
	case TessellatorPartitioning::Integer :
		return "partitioning_integer";
	case TessellatorPartitioning::Pow2 :
		return "partitioning_pow2";
	case TessellatorPartitioning::FractionalOdd :
		return "partitioning_fractional_odd";
	case TessellatorPartitioning::FractionalEven :
		return "partitioning_fractional_even";
	default :
		throw runtime_error("Unsupported value: " + to_string((int) value));
	}
}

string SlimShader::ToStringStat(TessellatorPartitioning value)
{
	switch (value)
	{
	case TessellatorPartitioning::Integer :
		return "Integer";
	case TessellatorPartitioning::Pow2 :
		return "Pow2";
	case TessellatorPartitioning::FractionalOdd :
		return "FractionalOdd";
	case TessellatorPartitioning::FractionalEven :
		return "FractionalEven";
	default :
		throw runtime_error("Unsupported value: " + to_string((int) value));
	}
}