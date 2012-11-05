#pragma once

#include "PCH.h"
#include "BytecodeReader.h"
#include "ResourceReturnType.h"

namespace SlimShader
{
	/// <summary>
	/// Resource Return Type Token (ResourceReturnTypeToken) (used in resource
	/// declaration statements)
	///
	/// [03:00] D3D10_SB_RESOURCE_RETURN_TYPE for component X
	/// [07:04] D3D10_SB_RESOURCE_RETURN_TYPE for component Y
	/// [11:08] D3D10_SB_RESOURCE_RETURN_TYPE for component Z
	/// [15:12] D3D10_SB_RESOURCE_RETURN_TYPE for component W
	/// [31:16] Reserved, 0
	/// </summary>
	class ResourceReturnTypeToken
	{
	public :
		static ResourceReturnTypeToken Parse(BytecodeReader& reader);

		ResourceReturnType GetX() const;
		ResourceReturnType GetY() const;
		ResourceReturnType GetZ() const;
		ResourceReturnType GetW() const;

		friend std::ostream& operator<<(std::ostream& out, const ResourceReturnTypeToken& value);

	private :
		ResourceReturnType _x;
		ResourceReturnType _y;
		ResourceReturnType _z;
		ResourceReturnType _w;
	};
};