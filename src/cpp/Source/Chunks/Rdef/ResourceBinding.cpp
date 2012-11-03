#include "PCH.h"
#include "ResourceBinding.h"

using namespace std;
using namespace SlimShader;

ResourceBinding ResourceBinding::Parse(const BytecodeReader& reader, BytecodeReader& resourceBindingReader)
{
	auto nameOffset = resourceBindingReader.ReadUInt32();
	auto nameReader = reader.CopyAtOffset(nameOffset);

	ResourceBinding result;

	result._name = nameReader.ReadString();
	result._type = static_cast<ShaderInputType>(resourceBindingReader.ReadUInt32());
	result._returnType = static_cast<ResourceReturnType>(resourceBindingReader.ReadUInt32());
	result._dimension = static_cast<ShaderResourceViewDimension>(resourceBindingReader.ReadUInt32());
	result._numSamples = resourceBindingReader.ReadUInt32();
	result._bindPoint = resourceBindingReader.ReadUInt32();
	result._bindCount = resourceBindingReader.ReadUInt32();
	result._flags = static_cast<ShaderInputFlags>(resourceBindingReader.ReadUInt32());

	return result;
}

const std::string& ResourceBinding::GetName() const { return _name; }
ShaderInputType ResourceBinding::GetType() const { return _type; }
uint32_t ResourceBinding::GetBindPoint() const { return _bindPoint; }
uint32_t ResourceBinding::GetBindCount() const { return _bindCount; }
ShaderInputFlags ResourceBinding::GetFlags() const { return _flags; }
ShaderResourceViewDimension ResourceBinding::GetDimension() const { return _dimension; }
ResourceReturnType ResourceBinding::GetReturnType() const { return _returnType; }
uint32_t ResourceBinding::GetNumSamples() { return _numSamples; }

string ToString(ResourceReturnType value, ShaderInputType shaderInputType)
{
	if (value == ResourceReturnType::Mixed)
	{
		switch (shaderInputType)
		{
		case ShaderInputType::Structured:
		case ShaderInputType::UavRwStructured:
			return "struct";
		case ShaderInputType::ByteAddress:
		case ShaderInputType::UavRwByteAddress:
			return "byte";
		default:
			throw runtime_error("Shader input type '" + to_string((int) shaderInputType) + "' is not supported.");
		}
	}
	return ToString(value);
}

string ToString(ShaderResourceViewDimension value, ShaderInputType shaderInputType,
	ResourceReturnType format)
{
	switch (shaderInputType)
	{
	case ShaderInputType::ByteAddress :
	case ShaderInputType::Structured:
		return "r/o";
	case ShaderInputType::UavRwByteAddress :
	case ShaderInputType::UavRwStructured :
	case ShaderInputType::UavRwStructuredWithCounter :
	case ShaderInputType::UavRwTyped :
		return "r/w";
	default :
		return ToString(value);
	}
}

bool IsMultiSampled(ShaderResourceViewDimension value)
{
	switch (value)
	{
	case ShaderResourceViewDimension::Texture2DMultiSampled :
	case ShaderResourceViewDimension::Texture2DMultiSampledArray :
		return true;
	default :
		return false;
	}
}

ostream& SlimShader::operator<<(ostream& out, const ResourceBinding& value)
{
	string returnType = ::ToString(value._returnType, value._type);
	if (value._returnType != ResourceReturnType::NotApplicable && value._returnType != ResourceReturnType::Mixed)
		returnType += "4";
	out << boost::format("// %-30s %10s %7s %11s %4i %8i")
		% value._name
		% ToString(value._type)
		% returnType
		% (::ToString(value._dimension, value._type, value._returnType) + (IsMultiSampled(value._dimension) ? to_string(value._numSamples) : ""))
		% value._bindPoint
		% value._bindCount;
	return out;
}