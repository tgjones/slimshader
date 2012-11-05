#include "PCH.h"
#include "SystemValueName.h"

using namespace std;
using namespace SlimShader;

string SlimShader::ToString(SystemValueName value)
{
	switch (value)
	{
	case SystemValueName::Undefined :
		return "NONE";
	case SystemValueName::Position :
		return "position";
	case SystemValueName::ClipDistance :
		return "clipdistance";
	case SystemValueName::CullDistance :
		return "culldistance";
	case SystemValueName::RenderTargetArrayIndex :
		return "rendertarget_array_index";
	case SystemValueName::ViewportArrayIndex :
		return "viewportarrayindex";
	case SystemValueName::VertexID :
		return "vertex_id";
	case SystemValueName::PrimitiveID :
		return "primitive_id";
	case SystemValueName::InstanceID :
		return "instanceid";
	case SystemValueName::IsFrontFace :
		return "isfrontface";
	case SystemValueName::SampleIndex :
		return "sampleindex";

	// The following are added for D3D11

	case SystemValueName::FinalQuadUEq0EdgeTessFactor :
		return "finalQuadUeq0EdgeTessFactor";
	case SystemValueName::FinalQuadVEq0EdgeTessFactor :
		return "finalQuadVeq0EdgeTessFactor";
	case SystemValueName::FinalQuadUEq1EdgeTessFactor :
		return "finalQuadUeq1EdgeTessFactor";
	case SystemValueName::FinalQuadVEq1EdgeTessFactor :
		return "finalQuadVeq1EdgeTessFactor";
	case SystemValueName::FinalQuadUInsideTessFactor :
		return "finalQuadUInsideTessFactor";
	case SystemValueName::FinalQuadVInsideTessFactor :
		return "finalQuadVInsideTessFactor";
	case SystemValueName::FinalTriUEq0EdgeTessFactor :
		return "finalTriUeq0EdgeTessFactor";
	case SystemValueName::FinalTriVEq0EdgeTessFactor :
		return "finalTriVeq0EdgeTessFactor";
	case SystemValueName::FinalTriWEq0EdgeTessFactor :
		return "finalTriWeq0EdgeTessFactor";
	case SystemValueName::FinalTriInsideTessFactor :
		return "finalTriInsideTessFactor";
	case SystemValueName::FinalLineDetailTessFactor :
		return "finalLineDetailTessFactor";
	case SystemValueName::FinalLineDensityTessFactor :
		return "finalLineDensityTessFactor";
	default :
		throw runtime_error("Unsupported value: " + to_string((int) value));
	}
}