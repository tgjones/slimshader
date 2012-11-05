#include "PCH.h"
#include "InputOutputSignatureChunk.h"

#include "InputSignatureChunk.h"
#include "OutputSignatureChunk.h"
#include "PatchConstantSignatureChunk.h"

using namespace std;
using namespace SlimShader;

shared_ptr<InputOutputSignatureChunk> InputOutputSignatureChunk::Parse(BytecodeReader& reader,
																	   ChunkType chunkType,
																	   ProgramType programType)
{
	shared_ptr<InputOutputSignatureChunk> result;
	switch (chunkType)
	{
	case ChunkType::Isgn :
		result = shared_ptr<InputOutputSignatureChunk>(new InputSignatureChunk());
		break;
	case ChunkType::Osgn :
		result = shared_ptr<InputOutputSignatureChunk>(new OutputSignatureChunk());
		break;
	case ChunkType::Pcsg :
		result = shared_ptr<InputOutputSignatureChunk>(new PatchConstantSignatureChunk());
		break;
	default :
		throw runtime_error("chunkType");
	}

	BytecodeReader chunkReader(reader);
	auto elementCount = chunkReader.ReadUInt32();
	auto uniqueKey = chunkReader.ReadUInt32();

	SignatureElementSize elementSize;
	switch (chunkType)
	{
	case ChunkType::Osg5 :
		elementSize = SignatureElementSize::_7;
		break;
	case ChunkType::Isgn:
	case ChunkType::Osgn:
	case ChunkType::Pcsg:
		elementSize = SignatureElementSize::_6;
		break;
	default:
		throw runtime_error("chunkType");
	}

	for (uint32_t i = 0; i < elementCount; i++)
		result->_parameters.push_back(
			SignatureParameterDescription::Parse(reader, chunkReader, chunkType, elementSize, programType));

	return result;
}

const std::vector<SignatureParameterDescription>& InputOutputSignatureChunk::GetParameters() const
{
	return _parameters;
}

ostream& SlimShader::operator<<(ostream& out, const InputOutputSignatureChunk& value)
{
	out << "// Output signature:" << endl;
	out << "//" << endl;

	out << "// Name                 Index   Mask Register SysValue Format   Used" << endl;
	out << "// -------------------- ----- ------ -------- -------- ------ ------" << endl;

	for (auto& parameter : value._parameters)
		out << "// " << parameter << endl;

	if (value._parameters.size() > 0)
		out << "//" << endl;
	else
		out << "// no " << value.GetOutputDescription() << endl;

	return out;
}