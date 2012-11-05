#pragma once

#include "PCH.h"
#include "BytecodeReader.h"
#include "CustomDataClass.h"
#include "OpcodeToken.h"

namespace SlimShader
{
	/// <summary>
	/// Custom-Data Block Format
	///
	/// DWORD 0 (CustomDataDescTok):
	/// [10:00] == D3D10_SB_OPCODE_CUSTOMDATA
	/// [31:11] == D3D10_SB_CUSTOMDATA_CLASS
	///
	/// DWORD 1: 
	///          32-bit unsigned integer count of number
	///          of DWORDs in custom-data block,
	///          including DWORD 0 and DWORD 1.
	///          So the minimum value is 0x00000002,
	///          meaning empty custom-data.
	///
	/// Layout of custom-data contents, for the various meta-data classes,
	/// not defined in this file.
	/// </summary>
	class CustomDataToken : public OpcodeToken
	{
	public :
		static std::shared_ptr<CustomDataToken> Parse(BytecodeReader& reader, uint32_t token0);

		CustomDataClass GetCustomDataClass() const;

	private :
		CustomDataClass _customDataClass;
	};
};