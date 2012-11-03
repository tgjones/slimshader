#include "PCH.h"
#include "ClassInstance.h"

using namespace std;
using namespace SlimShader;

ClassInstance ClassInstance::Parse(const BytecodeReader& reader, BytecodeReader& classInstanceReader)
{
	auto nameOffset = classInstanceReader.ReadUInt32();
	auto nameReader = reader.CopyAtOffset(nameOffset);
	auto name = nameReader.ReadString();

	auto type = classInstanceReader.ReadUInt16();
	assert(classInstanceReader.ReadUInt16() == 1); // Unknown, perhaps the class instance type?

	ClassInstance result;

	result._constantBuffer = classInstanceReader.ReadUInt16();
	result._constantBufferOffset = classInstanceReader.ReadUInt16();
	result._texture = classInstanceReader.ReadUInt16();
	result._sampler = classInstanceReader.ReadUInt16();

	return result;
}

std::string ClassInstance::GetName() const { return _name; }
uint16_t ClassInstance::GetType() const { return _type; }
uint16_t ClassInstance::GetConstantBuffer() const { return _constantBuffer; }
uint16_t ClassInstance::GetConstantBufferOffset() const { return _constantBufferOffset; }
uint16_t ClassInstance::GetTexture() const { return _texture; }
uint16_t ClassInstance::GetSampler() const { return _sampler; }

std::ostream& SlimShader::operator<<(std::ostream& out, const ClassInstance& value)
{
	// For example:
	// Name                        Type CB CB Offset Texture Sampler
	// --------------------------- ---- -- --------- ------- -------
	// g_ambientLight                12  0         0       -       -

	out << boost::format("%-27 %4 %2 %9 %7 %7")
		% value._name % value._type % value._constantBuffer % value._constantBufferOffset
		% ((value._texture == 0xFFFF) ? "-" : to_string(value._texture))
		% ((value._sampler == 0xFFFF) ? "-" : to_string(value._sampler));
	return out;
}