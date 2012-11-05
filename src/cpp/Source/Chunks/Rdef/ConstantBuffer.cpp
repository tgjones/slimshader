#include "PCH.h"
#include "ConstantBuffer.h"

using namespace std;
using namespace SlimShader;

ConstantBuffer ConstantBuffer::Parse(const BytecodeReader& reader,
									 BytecodeReader& constantBufferReader,
									 const ShaderVersion& target)
{
	auto nameOffset = constantBufferReader.ReadUInt32();
	auto nameReader = reader.CopyAtOffset(nameOffset);

	auto variableCount = constantBufferReader.ReadUInt32();
	auto variableOffset = constantBufferReader.ReadUInt32();

	ConstantBuffer result;

	result._name = nameReader.ReadString();
	result._size = constantBufferReader.ReadUInt32();
	result._flags = static_cast<ConstantBufferFlags>(constantBufferReader.ReadUInt32());
	result._bufferType = static_cast<ConstantBufferType>(constantBufferReader.ReadUInt32());

	auto variableReader = reader.CopyAtOffset(variableOffset);
	for (uint32_t i = 0; i < variableCount; i++)
		result._variables.push_back(ShaderVariable::Parse(reader, variableReader, target, i == 0));

	return result;
}

const string& ConstantBuffer::GetName() const { return _name; }
ConstantBufferType ConstantBuffer::GetBufferType() const { return _bufferType; }
const vector<ShaderVariable>& ConstantBuffer::GetVariables() const { return _variables; }
uint32_t ConstantBuffer::GetSize() const { return _size; }
ConstantBufferFlags ConstantBuffer::GetFlags() const { return _flags; }

ostream& SlimShader::operator<<(ostream& out, const ConstantBuffer& value)
{
	out << "// " << ToString(value._bufferType) << " " << value._name << endl;
	out << "// {" << endl;

	for (auto& variable : value._variables)
		out << variable;

	out << "//" << endl;
	out << "// }" << endl;
	out << "//" << endl;

	return out;
}