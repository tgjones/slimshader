#include "PCH.h"
#include "SignatureParameterDescription.h"

#include "Decoder.h"

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