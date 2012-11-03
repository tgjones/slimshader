#include "PCH.h"

namespace SlimShader
{
	enum class ResourceReturnType
	{
		NotApplicable = 0,

		/// <summary>
		/// Return type is an unsigned integer value normalized to a value between 0 and 1.
		/// </summary>
		UNorm = 1,

		/// <summary>
		/// Return type is a signed integer value normalized to a value between -1 and 1.
		/// </summary>
		SNorm = 2,

		/// <summary>
		/// Return type is a signed integer.
		/// </summary>
		SInt = 3,

		/// <summary>
		/// Return type is an unsigned integer.
		/// </summary>
		UInt = 4,

		Float = 5,

		/// <summary>
		/// Return type is unknown.
		/// </summary>
		Mixed = 6,

		/// <summary>
		/// Return type is a double-precision value.
		/// </summary>
		Double = 7,

		/// <summary>
		/// Return type is a multiple-dword type, such as a double or uint64, and the component is continued from the 
		/// previous component that was declared. The first component represents the lower bits.
		/// </summary>
		Continued = 8
	};

	std::string ToString(ResourceReturnType value);
};