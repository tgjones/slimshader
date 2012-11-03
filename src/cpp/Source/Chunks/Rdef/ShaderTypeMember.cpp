#include "PCH.h"
#include "ShaderTypeMember.h"

#include <numeric>
#include <sstream>
#include "ShaderType.h"

using namespace std;
using namespace SlimShader;

ShaderTypeMember ShaderTypeMember::Parse(const BytecodeReader& reader,
										 BytecodeReader& memberReader,
										 const ShaderVersion& target,
										 int indent,
										 bool isFirst,
										 uint32_t parentOffset)
{
	auto nameOffset = memberReader.ReadUInt32();
	auto nameReader = reader.CopyAtOffset(nameOffset);
	auto name = nameReader.ReadString();

	auto memberTypeOffset = memberReader.ReadUInt32();

	auto offset = memberReader.ReadUInt32();

	auto memberTypeReader = reader.CopyAtOffset(memberTypeOffset);
	auto memberType = ShaderType::Parse(reader, memberTypeReader, target, indent, isFirst, parentOffset + offset);

	return ShaderTypeMember(parentOffset, name, offset, memberType);
}

ShaderTypeMember::ShaderTypeMember(uint32_t parentOffset,
								   std::string name,
								   uint32_t offset,
								   shared_ptr<ShaderType> type)
	: _parentOffset(parentOffset), _name(name), _offset(offset), _type(type)
{

}

const string& ShaderTypeMember::GetName() const { return _name; }
uint32_t ShaderTypeMember::GetOffset() const { return _offset; }
const ShaderType& ShaderTypeMember::GetType() const { return *_type; }

ostream& SlimShader::operator<<(ostream& out, const ShaderTypeMember& value)
{
	stringstream declarationStream;
	declarationStream << *value._type << " " << value._name;
	if (value._type->GetElementCount() > 0)
		declarationStream << "[" << value._type->GetElementCount() << "]";
	declarationStream << ";";

	// Split declaration into separate lines, so that we can put the "// Offset" comment at the right place.
	string line;
	vector<string> declarationLines;
	while (getline(declarationStream, line))
		declarationLines.push_back(line);
	declarationLines[declarationLines.size() - 1] = (boost::format("%-40s// Offset: %4i")
		% declarationLines[declarationLines.size() - 1]
		% (value._parentOffset + value._offset)).str();
	
	for (int i = 0; i < declarationLines.size(); i++)
	{
		out << declarationLines[i];
		if (i < declarationLines.size() - 1)
			out << endl;
	}

	return out;
}