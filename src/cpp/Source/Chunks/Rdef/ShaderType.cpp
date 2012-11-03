#include "PCH.h"
#include "ShaderType.h"

using namespace std;
using namespace SlimShader;

shared_ptr<ShaderType> ShaderType::Parse(const BytecodeReader& reader, 
										 BytecodeReader& typeReader, 
										 const ShaderVersion& target,
										 int indent, 
										 bool isFirst, 
										 uint32_t parentOffset)
{
	auto result = shared_ptr<ShaderType>(new ShaderType(indent, isFirst));
	result->_variableClass = static_cast<ShaderVariableClass>(typeReader.ReadUInt16());
	result->_variableType = static_cast<ShaderVariableType>(typeReader.ReadUInt16());
	result->_rows = typeReader.ReadUInt16();
	result->_columns = typeReader.ReadUInt16();
	result->_elementCount = typeReader.ReadUInt16();

	auto memberCount = typeReader.ReadUInt16();
	auto memberOffset = typeReader.ReadUInt32();

	if (target.GetMajorVersion() >= 5)
	{
		auto parentTypeOffset = typeReader.ReadUInt32(); // Guessing
		auto parentTypeReader = reader.CopyAtOffset(parentTypeOffset);
		auto parentTypeClass = static_cast<ShaderVariableClass>(parentTypeReader.ReadUInt16());
		auto unknown4 = parentTypeReader.ReadUInt16();

		auto unknown1 = typeReader.ReadUInt32();
		if (unknown1 != 0)
		{
			auto unknownReader = reader.CopyAtOffset(unknown1);
			uint32_t unknown5 = unknownReader.ReadUInt32();
		}

		auto unknown2 = typeReader.ReadUInt32();
		auto unknown3 = typeReader.ReadUInt32();

		auto parentNameOffset = typeReader.ReadUInt32();
		if (parentNameOffset > 0)
		{
			auto parentNameReader = reader.CopyAtOffset(parentNameOffset);
			result->_baseTypeName = parentNameReader.ReadString();
		}
	}

	if (memberCount > 0)
	{
		auto memberReader = reader.CopyAtOffset(memberOffset);
		for (int i = 0; i < memberCount; i++)
			result->_members.push_back(ShaderTypeMember::Parse(reader, memberReader, target, indent + 4, i == 0,
				parentOffset));
	}

	return result;
}

ShaderVariableClass ShaderType::GetVariableClass() const { return _variableClass; }
ShaderVariableType ShaderType::GetVariableType() const { return _variableType; }
uint16_t ShaderType::GetRows() const { return _rows; }
uint16_t ShaderType::GetColumns() const { return _columns; }
uint16_t ShaderType::GetElementCount() const { return _elementCount; }
const std::vector<ShaderTypeMember>& ShaderType::GetMembers() const { return _members; }
const std::string& ShaderType::GetBaseTypeName() const { return _baseTypeName; }

ShaderType::ShaderType(int indent, bool isFirst)
	: _indent(indent), _isFirst(isFirst)
{

}

std::ostream& SlimShader::operator<<(std::ostream& out, const ShaderType& value)
{
	auto indentString = "// " + string(value._indent, ' ');
	if (value._isFirst)
		out << indentString << endl;
	switch (value._variableClass)
	{
	case ShaderVariableClass::InterfacePointer:
	case ShaderVariableClass::MatrixColumns:
	case ShaderVariableClass::MatrixRows:
		{
			out << indentString;
			if (value._baseTypeName.size() > 0) // BaseTypeName is only populated in SM 5.0
			{
				out << ToString(value._variableClass) << value._baseTypeName;
			}
			else
			{
				out << ToString(value._variableClass);
				out << ToString(value._variableType);
				if (value._columns > 1)
				{
					out << value._columns;
					if (value._rows > 1)
						out << "x" << value._rows;
				}
			}
			break;
		}
	case ShaderVariableClass::Vector:
		{
			out << indentString << ToString(value._variableType);
			if (value._columns > 1)
				out << value._columns;
			break;
		}
	case ShaderVariableClass::Struct:
		{
			if (!value._isFirst)
				out << indentString << endl;
			out << indentString << "struct " << value._baseTypeName << endl;
			out << indentString << "{" << endl;
			for (auto& member : value._members)
				out << member << endl;
			out << "//" << endl;
			out << indentString << "}";
			break;
		}
	case ShaderVariableClass::Scalar:
		{
			out << indentString << ToString(value._variableType);
			break;
		}
	default:
		throw new runtime_error("Variable class '" + to_string((int) value._variableClass) + "' is not currently supported.");
	}
	return out;
}