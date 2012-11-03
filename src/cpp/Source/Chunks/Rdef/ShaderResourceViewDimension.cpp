#include "PCH.h"
#include "ShaderResourceViewDimension.h"

using namespace std;
using namespace SlimShader;

string SlimShader::ToString(ShaderResourceViewDimension value)
{
	switch (value)
	{
	case ShaderResourceViewDimension::Unknown :
		return "NA";
	case ShaderResourceViewDimension::Buffer :
		return "buf";
	case ShaderResourceViewDimension::Texture1D :
		return "1d";
	case ShaderResourceViewDimension::Texture1DArray :
		return "1darray";
	case ShaderResourceViewDimension::Texture2D:
		return "2d";
	case ShaderResourceViewDimension::Texture2DArray :
		return "2darray";
	case ShaderResourceViewDimension::Texture2DMultiSampled :
		return "2dMS";
	case ShaderResourceViewDimension::Texture2DMultiSampledArray :
		return "2dMSarray";
	case ShaderResourceViewDimension::Texture3D :
		return "3d";
	case ShaderResourceViewDimension::TextureCube :
		return "cube";
	case ShaderResourceViewDimension::TextureCubeArray :
		return "cubearray";
	default:
		throw runtime_error("Shader resource view dimension '" + to_string((int) value) + "' is not supported.");
	}
}