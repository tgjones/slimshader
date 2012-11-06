#include "PCH.h"
#include "PrimitiveTopology.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

string SlimShader::ToStringShex(PrimitiveTopology value)
{
	switch (value)
	{
	case PrimitiveTopology::PointList :
		return "pointlist";
	case PrimitiveTopology::LineList :
		return "linelist";
	case PrimitiveTopology::LineStrip :
		return "linestrip";
	case PrimitiveTopology::TriangleList :
		return "trianglelist";
	case PrimitiveTopology::TriangleStrip :
		return "trianglestrip";
	default :
		throw runtime_error("Unsupported value: " + to_string((int) value));
	}
}