#include "PCH.h"
#include "InterfacesChunk.h"

using namespace std;
using namespace SlimShader;

shared_ptr<InterfacesChunk> InterfacesChunk::Parse(BytecodeReader& reader, uint32_t sizeInBytes)
{
	BytecodeReader headerReader(reader);

	auto result = shared_ptr<InterfacesChunk>(new InterfacesChunk());

	auto classInstanceCount = headerReader.ReadUInt32();
	auto classTypeCount = headerReader.ReadUInt32();
	auto interfaceSlotCount = headerReader.ReadUInt32();

	assert(headerReader.ReadUInt32() == interfaceSlotCount); // Unknown

	headerReader.ReadUInt32(); // Think this is offset to start of interface slot info, but we don't need it.

	auto classTypeOffset = headerReader.ReadUInt32();
	auto availableClassReader = reader.CopyAtOffset(classTypeOffset);

	auto interfaceSlotOffset = headerReader.ReadUInt32();
	auto interfaceSlotReader = reader.CopyAtOffset(interfaceSlotOffset);

	for (uint32_t i = 0; i < classTypeCount; i++)
	{
		auto classType = ClassType::Parse(reader, availableClassReader);
		classType.SetID(i); // TODO: Really??
		result->_availableClassTypes.push_back(classType);
	}

	for (uint32_t i = 0; i < classInstanceCount; i++)
	{
		auto classInstance = ClassInstance::Parse(reader, availableClassReader);
		result->_availableClassInstances.push_back(classInstance);
	}

	for (uint32_t i = 0; i < interfaceSlotCount; i++)
	{
		auto interfaceSlot = InterfaceSlot::Parse(reader, interfaceSlotReader);
		interfaceSlot.SetID(i); // TODO: Really??
		result->_interfaceSlots.push_back(interfaceSlot);
	}

	return result;
}

const std::vector<ClassType>& InterfacesChunk::GetAvailableClassTypes() const { return _availableClassTypes; }
const std::vector<ClassInstance>& InterfacesChunk::GetAvailableClassInstances() const { return _availableClassInstances; }
const std::vector<InterfaceSlot>& InterfacesChunk::GetInterfaceSlots() const { return _interfaceSlots; }

std::ostream& SlimShader::operator<<(std::ostream& out, const InterfacesChunk& value)
{
	out << "//" << endl;
	out << "// Available Class Types:" << endl;
	out << "//" << endl;
	out << "// Name                             ID CB Stride Texture Sampler" << endl;
	out << "// ------------------------------ ---- --------- ------- -------" << endl;

	for (auto& classType : value._availableClassTypes)
		out << "// " << classType << endl;

	out << "//" << endl;

	if (!value._availableClassInstances.empty())
	{
		out << "// Available Class Instances:" << endl;
		out << "//" << endl;
		out << "// Name                        Type CB CB Offset Texture Sampler" << endl;
		out << "// --------------------------- ---- -- --------- ------- -------" << endl;

		for (auto& classInstance : value._availableClassInstances)
			out << "// " << classInstance << endl;

		out << "//" << endl;
	}

	out << boost::format("// Interface slots, %i total:") % value._interfaceSlots.size() << endl;
	out << "//" << endl;
	out << "//             Slots" << endl;
	out << "// +----------+---------+---------------------------------------" << endl;

	for (auto& interfaceSlot : value._interfaceSlots)
		out << interfaceSlot;

	return out;
}