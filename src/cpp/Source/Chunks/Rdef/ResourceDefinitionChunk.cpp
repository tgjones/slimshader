#include "PCH.h"
#include "ResourceDefinitionChunk.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

shared_ptr<ResourceDefinitionChunk> ResourceDefinitionChunk::Parse(BytecodeReader& reader)
{
	BytecodeReader headerReader(reader);

	auto constantBufferCount = headerReader.ReadUInt32();
	auto constantBufferOffset = headerReader.ReadUInt32();
	auto resourceBindingCount = headerReader.ReadUInt32();
	auto resourceBindingOffset = headerReader.ReadUInt32();
	auto target = ShaderVersion::ParseRdef(headerReader);
	auto flags = static_cast<ShaderFlags>(headerReader.ReadUInt32());

	auto creatorOffset = headerReader.ReadUInt32();
	auto creatorReader = reader.CopyAtOffset(creatorOffset);
	auto creator = creatorReader.ReadString();

	// TODO: Parse Direct3D 11 resource definition stuff.
	// https://github.com/mirrors/wine/blob/master/dlls/d3dcompiler_43/reflection.c#L1429

	if (target.GetMajorVersion() >= 5)
	{
		auto rd11 = ToFourCcString(headerReader.ReadUInt32());
		if (rd11 != "RD11")
			throw runtime_error("Expected RD11.");
		auto unknown1 = headerReader.ReadUInt32();
		auto unknown2 = headerReader.ReadUInt32();
		auto unknown3 = headerReader.ReadUInt32();
		auto unknown4 = headerReader.ReadUInt32();
		auto unknown5 = headerReader.ReadUInt32();
		auto unknown6 = headerReader.ReadUInt32();
		auto unknown7 = headerReader.ReadUInt32();
	}

	auto result = shared_ptr<ResourceDefinitionChunk>(new ResourceDefinitionChunk(target));
	result->_flags = flags;
	result->_creator = creator;

	auto constantBufferReader = reader.CopyAtOffset(constantBufferOffset);
	for (uint32_t i = 0; i < constantBufferCount; i++)
		result->_constantBuffers.push_back(ConstantBuffer::Parse(reader, constantBufferReader, result->_target));

	auto resourceBindingReader = reader.CopyAtOffset(resourceBindingOffset);
	for (uint32_t i = 0; i < resourceBindingCount; i++)
		result->_resourceBindings.push_back(ResourceBinding::Parse(reader, resourceBindingReader));

	return result;
}

const std::vector<ConstantBuffer>& ResourceDefinitionChunk::GetConstantBuffers() { return _constantBuffers; }
const std::vector<ResourceBinding>& ResourceDefinitionChunk::GetResourceBindings() { return _resourceBindings; }
const ShaderVersion& ResourceDefinitionChunk::GetTarget() const { return _target; }
ShaderFlags ResourceDefinitionChunk::GetFlags() const { return _flags; }
const std::string& ResourceDefinitionChunk::GetCreator() const { return _creator; }

ResourceDefinitionChunk::ResourceDefinitionChunk(ShaderVersion target)
	: _target(target)
{

}

std::ostream& SlimShader::operator<<(std::ostream& out, const ResourceDefinitionChunk& value)
{
	if (!value._constantBuffers.empty())
	{
		out << "// Buffer Definitions: " << endl;
		out << "//" << endl;

		for (auto& constantBuffer : value._constantBuffers)
			out << constantBuffer;

		out << "//" << endl;
	}

	if (!value._resourceBindings.empty())
	{
		out << "// Resource Bindings:" << endl;
		out << "//" << endl;
		out << "// Name                                 Type  Format         Dim Slot Elements" << endl;
		out << "// ------------------------------ ---------- ------- ----------- ---- --------" << endl;

		for (auto& resourceBinding : value._resourceBindings)
			out << resourceBinding << endl;

		out << "//" << endl;
		out << "//" << endl;
	}

	return out;
}