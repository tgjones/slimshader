#include "PCH.h"
#include "InterfaceSlot.h"

#include <sstream>

using namespace std;
using namespace SlimShader;

InterfaceSlot InterfaceSlot::Parse(const BytecodeReader& reader, BytecodeReader& interfaceSlotReader)
{
	assert(interfaceSlotReader.ReadUInt32() == 1); // Unknown

	auto count = interfaceSlotReader.ReadUInt32();

	auto typeIDsOffset = interfaceSlotReader.ReadUInt32();
	auto typeIDsReader = reader.CopyAtOffset(typeIDsOffset);

	auto tableIDsOffset = interfaceSlotReader.ReadUInt32();
	auto tableIDsReader = reader.CopyAtOffset(tableIDsOffset);

	InterfaceSlot result;

	vector<uint32_t> typeIDs, tableIDs;
	for (uint32_t i = 0; i < count; i++)
	{
		result._typeIDs.push_back(typeIDsReader.ReadUInt16());
		result._tableIDs.push_back(tableIDsReader.ReadUInt32());
	}

	return result;
}

const uint32_t InterfaceSlot::GetID() const { return _id; }
void InterfaceSlot::SetID(uint32_t id) { _id = id; }

const std::vector<uint32_t>& InterfaceSlot::GetTypeIDs() const { return _typeIDs; }
const std::vector<uint32_t>& InterfaceSlot::GetTableIDs() const { return _tableIDs; }

std::ostream& SlimShader::operator<<(std::ostream& out, const InterfaceSlot& value)
{
	// For example:
	// | Type ID  |   0     |0    1    2    
	// | Table ID |         |0    1    2    
	// +----------+---------+---------------------------------------

	stringstream typeIDsStream;
	for (auto typeID : value._typeIDs)
		typeIDsStream << boost::format("%-4i ") % typeID;

	stringstream tableIDsStream;
	for (auto tableID : value._tableIDs)
		tableIDsStream << boost::format("%-4i ") % tableID;

	out << (boost::format("// | Type ID  |   %i     |%s")
		% value._id
		% typeIDsStream.str())
		<< endl;
	out << (boost::format("// | Table ID |         |%s")
		% tableIDsStream.str())
		<< endl;
	out << "// +----------+---------+---------------------------------------" << endl;

	return out;
}