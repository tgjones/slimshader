#include "PCH.h"
#include "InterfaceSlot.h"

#include <sstream>

using namespace std;
using namespace SlimShader;

InterfaceSlot InterfaceSlot::Parse(const BytecodeReader& reader, BytecodeReader& interfaceSlotReader)
{
	auto slotSpan = interfaceSlotReader.ReadUInt32();

	auto count = interfaceSlotReader.ReadUInt32();

	auto typeIDsOffset = interfaceSlotReader.ReadUInt32();
	auto typeIDsReader = reader.CopyAtOffset(typeIDsOffset);

	auto tableIDsOffset = interfaceSlotReader.ReadUInt32();
	auto tableIDsReader = reader.CopyAtOffset(tableIDsOffset);

	InterfaceSlot result;
	result._slotSpan = slotSpan;

	vector<uint32_t> typeIDs, tableIDs;
	for (uint32_t i = 0; i < count; i++)
	{
		result._typeIDs.push_back(typeIDsReader.ReadUInt16());
		result._tableIDs.push_back(tableIDsReader.ReadUInt32());
	}

	return result;
}

uint32_t InterfaceSlot::GetStartSlot() const { return _startSlot; }
void InterfaceSlot::SetStartSlot(uint32_t startSlot) { _startSlot = startSlot; }

uint32_t InterfaceSlot::GetSlotSpan() const { return _slotSpan; }

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

	string slotSpan = (value._slotSpan == 1) 
		? to_string(value._startSlot)
		: to_string(value._startSlot) + "-" + to_string(value._startSlot + value._slotSpan - 1);

	out << (boost::format("// | Type ID  |   %-5i |%s")
		% slotSpan
		% typeIDsStream.str())
		<< endl;
	out << (boost::format("// | Table ID |         |%s")
		% tableIDsStream.str())
		<< endl;
	out << "// +----------+---------+---------------------------------------" << endl;

	return out;
}