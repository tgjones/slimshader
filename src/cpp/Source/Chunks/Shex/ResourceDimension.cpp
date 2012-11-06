#include "PCH.h"
#include "ResourceDimension.h"

using namespace std;
using namespace SlimShader;

string SlimShader::ToString(ResourceDimension value)
{
	switch (value)
	{
	case ResourceDimension::Buffer :
		return "buffer";
	case ResourceDimension::Texture1D :
		return "texture1d";
	case ResourceDimension::Texture2D :
		return "texture2d";
	case ResourceDimension::Texture2DMultiSampled :
		return "texture2dms";
	case ResourceDimension::Texture3D :
		return "texture3d";
	case ResourceDimension::TextureCube :
		return "texturecube";
	case ResourceDimension::Texture1DArray :
		return "texture1darray";
	case ResourceDimension::Texture2DArray :
		return "texture2darray";
	case ResourceDimension::Texture2DMultiSampledArray :
		return "texture2dmsarray";
	case ResourceDimension::TextureCubeArray :
		return "texturecubearray";
	case ResourceDimension::RawBuffer :
		return "raw_buffer";
	case ResourceDimension::StructuredBuffer :
		return "structured_buffer";
	default :
		throw runtime_error("Unsupported value: " + to_string((int) value));
	}
}