#include "PCH.h"
#include "ClassType.h"

using namespace SlimShader;

ClassType ClassType::Parse(const BytecodeReader& reader, BytecodeReader& classTypeReader)
{
	auto nameOffset = classTypeReader.ReadUInt32();
	auto nameReader = reader.CopyAtOffset(nameOffset);

	ClassType result;

	result._name = nameReader.ReadString();
	result._id = classTypeReader.ReadUInt16();
	result._constantBufferStride = classTypeReader.ReadUInt16();
	result._texture = classTypeReader.ReadUInt16();
	result._sampler = classTypeReader.ReadUInt16();

	return result;
}

std::string ClassType::GetName() const { return _name; }
uint32_t ClassType::GetID() const { return _id; }
uint32_t ClassType::GetConstantBufferStride() const { return _constantBufferStride; }
uint32_t ClassType::GetTexture() const { return _texture; }
uint32_t ClassType::GetSampler() const { return _sampler; }

std::ostream& SlimShader::operator<<(std::ostream& out, const ClassType& value)
{
	// For example:
	// Name                             ID CB Stride Texture Sampler");
	// ------------------------------ ---- --------- ------- -------");
	// cUnchangedColour                  0         0       0       0");
	out << boost::format("%-30 %4 %9 %7 %7")
		% value._name % value._id % value._constantBufferStride % value._texture % value._sampler;
	return out;
}