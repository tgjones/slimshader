#include "PCH.h"
#include "ImmediateConstantBufferDeclarationToken.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

shared_ptr<ImmediateConstantBufferDeclarationToken> ImmediateConstantBufferDeclarationToken::Parse(BytecodeReader& reader)
{
	auto token0 = reader.ReadUInt32();
	auto length = reader.ReadUInt32() - 2;

	auto result = shared_ptr<ImmediateConstantBufferDeclarationToken>(new ImmediateConstantBufferDeclarationToken());
	result->_declarationLength = length;

	for (uint32_t i = 0; i < length; i++)
		result->_data.push_back(Number::Parse(reader, NumberType::Unknown));

	return result;
}

const vector<Number>& ImmediateConstantBufferDeclarationToken::GetData() const { return _data; }

void ImmediateConstantBufferDeclarationToken::Print(std::ostream& out) const
{
	out << "dcl_immediateConstantBuffer { ";

	for (size_t i = 0; i < _data.size(); i += 4)
	{
		if (i > 0)
			out << ",\n" << string(30, ' ');
		out << "{ " << _data[i] << ", " << _data[i + 1] << ", "
			<< _data[i + 2] << ", " << _data[i + 3] << "}";
	}
			
	out << " }";
}