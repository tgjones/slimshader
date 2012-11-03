#include "PCH.h"
#include "ShaderVersion.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

ShaderVersion ShaderVersion::ParseShex(BytecodeReader& reader)
{
	auto versionToken = reader.ReadUInt32();
	
	ShaderVersion result;
	result._minorVersion = DecodeValue<uint8_t>(versionToken, 0, 3);
	result._majorVersion = DecodeValue<uint8_t>(versionToken, 4, 7);
	result._programType = DecodeValue<ProgramType>(versionToken, 16, 31);
	return result;
}

ShaderVersion ShaderVersion::ParseRdef(BytecodeReader& reader)
{
	auto target = reader.ReadUInt32();

	auto programTypeValue = DecodeValue<uint16_t>(target, 16, 31);
	ProgramType programType;
	switch (programTypeValue)
	{
	case 0xFFFF:
		programType = ProgramType::PixelShader;
		break;
	case 0xFFFE:
		programType = ProgramType::VertexShader;
		break;
	case 0x4853:
		programType = ProgramType::HullShader;
		break;
	case 0x4753:
		programType = ProgramType::GeometryShader;
		break;
	case 0x4453:
		programType = ProgramType::DomainShader;
		break;
	case 0x4353:
		programType = ProgramType::ComputeShader;
		break;
	default:
		throw runtime_error("Unknown program type: " + to_string(programTypeValue));
	}

	ShaderVersion result;
	result._majorVersion = DecodeValue<uint8_t>(target, 8, 15);
	result._minorVersion = DecodeValue<uint8_t>(target, 0, 7);
	result._programType = programType;
	return result;
}

uint8_t ShaderVersion::GetMajorVersion() const { return _majorVersion; }
uint8_t ShaderVersion::GetMinorVersion() const { return _minorVersion; }
ProgramType ShaderVersion::GetProgramType() const { return _programType; }