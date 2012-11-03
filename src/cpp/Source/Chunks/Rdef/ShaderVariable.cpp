#include "PCH.h"
#include "ShaderVariable.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

ShaderVariable ShaderVariable::Parse(const BytecodeReader& reader,
									 BytecodeReader& variableReader,
									 const ShaderVersion& target,
									 bool isFirst)
{
	auto nameOffset = variableReader.ReadUInt32();
	auto nameReader = reader.CopyAtOffset(nameOffset);

	auto startOffset = variableReader.ReadUInt32();
	auto size = variableReader.ReadUInt32();
	auto flags = static_cast<ShaderVariableFlags>(variableReader.ReadUInt32());

	auto typeOffset = variableReader.ReadUInt32();
	auto typeReader = reader.CopyAtOffset((int) typeOffset);
	auto shaderType = ShaderType::Parse(reader, typeReader, target, 2, isFirst, startOffset);

	auto defaultValueOffset = variableReader.ReadUInt32();
	if (defaultValueOffset != 0)
	{
		auto defaultValueReader = reader.CopyAtOffset(defaultValueOffset);
		// TODO: Read default value
		// https://github.com/mirrors/wine/blob/master/dlls/d3dcompiler_43/reflection.c#L1362
	}

	auto name = nameReader.ReadString();

	ShaderVariable result(ShaderTypeMember(0, name, startOffset, shaderType));
	result._baseType = nameReader.ReadString();
	result._size = size;
	result._flags = flags;

	if (target.GetMajorVersion() >= 5)
	{
		result._startTexture = variableReader.ReadInt32();
		result._textureSize = variableReader.ReadInt32();
		result._startSampler = variableReader.ReadInt32();
		result._samplerSize = variableReader.ReadInt32();
	}

	return result;
}

const std::string& ShaderVariable::GetName() const { return _member.GetName(); }
uint32_t ShaderVariable::GetStartOffset() const { return _member.GetOffset(); }
ShaderType ShaderVariable::GetShaderType() const { return _member.GetType(); }
const std::string& ShaderVariable::GetBaseType() const { return _baseType; }
uint32_t ShaderVariable::GetSize() const { return _size; }
ShaderVariableFlags ShaderVariable::GetFlags() const { return _flags; }
int ShaderVariable::GetStartTexture() const { return _startTexture; }
int ShaderVariable::GetTextureSize() const { return _textureSize; }
int ShaderVariable::GetStartSampler() const { return _startSampler; }
int ShaderVariable::GetSamplerSize() const { return _samplerSize; }

ShaderVariable::ShaderVariable(ShaderTypeMember member)
	: _member(member)
{

}

std::ostream& SlimShader::operator<<(std::ostream& out, const ShaderVariable& value)
{
	out << value._member << boost::format(" Size: %5i") % value._size;

	if (!HasFlag(value._flags, ShaderVariableFlags::Used))
		out << " [unused]";

	out << endl;

	return out;
}