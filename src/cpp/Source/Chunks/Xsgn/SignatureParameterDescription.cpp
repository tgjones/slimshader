#include "PCH.h"
#include "SignatureParameterDescription.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

SignatureParameterDescription SignatureParameterDescription::Parse(const BytecodeReader& reader,
																   BytecodeReader& parameterReader,
																   ChunkType chunkType,
																   SignatureElementSize size,
																   ProgramType programType)
{
	auto stream = 0;
	if (size == SignatureElementSize::_7)
		stream = parameterReader.ReadUInt32(); // TODO: Different from C# version
 
	auto nameOffset = parameterReader.ReadUInt32();
	auto nameReader = reader.CopyAtOffset(nameOffset);

	SignatureParameterDescription result;
	result._semanticName = nameReader.ReadString();
	result._semanticIndex = parameterReader.ReadUInt32();
	result._systemValueType = static_cast<Name>(parameterReader.ReadUInt32());
	result._componentType = static_cast<RegisterComponentType>(parameterReader.ReadUInt32());
	result._register = parameterReader.ReadUInt32();
	result._stream = stream;

	auto mask = parameterReader.ReadUInt32();
	result._mask = DecodeValue<ComponentMask>(mask, 0, 7);
	result._readWriteMask = DecodeValue<ComponentMask>(mask, 8, 15);

	// Maybe something in the higher order of mask that indicates used or unused...

	// TODO: This is completely made up by me...
	if (chunkType == ChunkType::Osg5 || chunkType == ChunkType::Osgn
		|| (chunkType == ChunkType::Pcsg && programType == ProgramType::HullShader))
		result._readWriteMask = static_cast<ComponentMask>(
			static_cast<int>(ComponentMask::All) - static_cast<int>(result._readWriteMask));

	// Vertex and pixel shaders need special handling for SystemValueType in the output signature (thanks Wine!)
	// http://source.winehq.org/source/dlls/d3dcompiler_43/reflection.c
	if ((programType == ProgramType::PixelShader || programType == ProgramType::VertexShader)
		&& (chunkType == ChunkType::Osg5 || chunkType == ChunkType::Osgn))
	{
		if (result._register == 0xffffffff)
		{
			auto str = result._semanticName;
			std::transform(str.begin(), str.end(), str.begin(), ::toupper);
			if (str == "SV_DEPTH")
				result._systemValueType = Name::Depth;
			else if (str == "SV_COVERAGE")
				result._systemValueType = Name::Coverage;
			else if (str =="SV_DEPTHGREATEREQUAL")
				result._systemValueType = Name::DepthGreaterEqual;
			else if (str == "SV_DEPTHLESSEQUAL")
				result._systemValueType = Name::DepthLessEqual;
		}
		else if (programType == ProgramType::PixelShader)
		{
			result._systemValueType = Name::Target;
		}
	}

	return result;
}

const std::string& SignatureParameterDescription::GetSemanticName() const { return _semanticName; }
uint32_t SignatureParameterDescription::GetSemanticIndex() const { return _semanticIndex; }
uint32_t SignatureParameterDescription::GetRegister() const { return _register; }
Name SignatureParameterDescription::GetSystemValueType() const { return _systemValueType; }
RegisterComponentType SignatureParameterDescription::GetComponentType() const { return _componentType; }
ComponentMask SignatureParameterDescription::GetMask() const { return _mask; }
ComponentMask SignatureParameterDescription::GetReadWriteMask() const { return _readWriteMask; }
uint32_t SignatureParameterDescription::GetStream() const { return _stream; }

bool RequiresMask(Name value)
{
	switch (value)
	{
	case Name::Coverage:
	case Name::Depth:
	case Name::DepthGreaterEqual :
	case Name::DepthLessEqual :
		return false;
	default :
		return true;
	}
}

string GetRegisterName(Name value)
{
	switch (value)
	{
	case Name::DepthGreaterEqual:
		return "oDepthGE";
	case Name::DepthLessEqual:
		return "oDepthLE";
	default:
		throw runtime_error("Unsupported value: " + to_string((int) value));
	}
}

string ToString(ComponentMask value)
{
	string result;

	result += (HasFlag(value, ComponentMask::X)) ? "x" : " ";
	result += (HasFlag(value, ComponentMask::Y)) ? "y" : " ";
	result += (HasFlag(value, ComponentMask::Z)) ? "z" : " ";
	result += (HasFlag(value, ComponentMask::W)) ? "w" : " ";

	return result;
}

ostream& SlimShader::operator<<(ostream& out, const SignatureParameterDescription& value)
{
	// For example:
	// Name                 Index   Mask Register SysValue Format   Used
	// TEXCOORD                 0   xyzw        0     NONE  float   xyzw
	// SV_DepthGreaterEqual     0    N/A oDepthGE  DEPTHGE  float    YES
	if (RequiresMask(value._systemValueType))
	{
		out << boost::format("%-20s %5i   %-4s %8s %8s %6s   %-4s")
			% value._semanticName % value._semanticIndex
			% ToStringShex(value._mask)
			% value._register
			% ToString(value._systemValueType)
			% ToString(value._componentType)
			% ::ToString(value._readWriteMask);
	}
	else
	{
		out << boost::format("%-20s %5i   %5s %8s %8s %6s   %4s")
			% value._semanticName % value._semanticIndex
			% "N/A"
			% GetRegisterName(value._systemValueType)
			% ToString(value._systemValueType)
			% ToString(value._componentType)
			% "YES";
	}
	return out;
}