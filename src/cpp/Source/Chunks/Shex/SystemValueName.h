#pragma once

#include "PCH.h"

namespace SlimShader
{
	enum class SystemValueName
	{
		Undefined = 0,
		Position = 1,
		ClipDistance = 2,
		CullDistance = 3,
		RenderTargetArrayIndex = 4,
		ViewportArrayIndex = 5,
		VertexID = 6,
		PrimitiveID = 7,
		InstanceID = 8,
		IsFrontFace = 9,
		SampleIndex = 10,

		// The following are added for D3D11

		FinalQuadUEq0EdgeTessFactor = 11,
		FinalQuadVEq0EdgeTessFactor = 12,
		FinalQuadUEq1EdgeTessFactor = 13,
		FinalQuadVEq1EdgeTessFactor = 14,
		FinalQuadUInsideTessFactor = 15,
		FinalQuadVInsideTessFactor = 16,
		FinalTriUEq0EdgeTessFactor = 17,
		FinalTriVEq0EdgeTessFactor = 18,
		FinalTriWEq0EdgeTessFactor = 19,
		FinalTriInsideTessFactor = 20,
		FinalLineDetailTessFactor = 21,
		FinalLineDensityTessFactor = 22
	};

	std::string ToString(SystemValueName value);
};