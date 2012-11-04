#include "PCH.h"
#include "Name.h"

using namespace std;
using namespace SlimShader;

string SlimShader::ToString(Name value)
{
	switch (value)
	{
	case Name::Undefined :
		return "NONE";
	case Name::Position :
		return "POS";
	case Name::RenderTargetArrayIndex :
		return "RTINDEX";
	case Name::VertexID :
		return "VERTID";
	case Name::PrimitiveID :
		return "PRIMID";
	case Name::FinalQuadEdgeTessFactor :
		return "QUADEDGE";
	case Name::FinalQuadInsideTessFactor :
		return "QUADINT";
	case Name::FinalTriEdgeTessFactor :
		return "TRIEDGE";
	case Name::FinalTriInsideTessFactor :
		return "TRIINT";
	case Name::FinalLineDetailTessFactor :
		return "LINEDET";
	case Name::FinalLineDensityTessFactor :
		return "LINEDEN";
	case Name::Target :
		return "TARGET";
	case Name::Depth :
		return "DEPTH";
	case Name::DepthGreaterEqual :
		return "DEPTHGE";
	case Name::DepthLessEqual :
		return "DEPTHLE";
	default :
		throw runtime_error("Unsupported value: " + to_string((int) value));
	}
}