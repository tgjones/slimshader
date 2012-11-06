#include "PCH.h"
#include "InterfaceDeclarationToken.h"

#include "Decoder.h"

using namespace boolinq;
using namespace std;
using namespace SlimShader;

shared_ptr<InterfaceDeclarationToken> InterfaceDeclarationToken::Parse(BytecodeReader& reader)
{
	auto token0 = reader.ReadUInt32();

	auto result = shared_ptr<InterfaceDeclarationToken>(new InterfaceDeclarationToken());
	result->_isDynamicallyIndexed = (DecodeValue(token0, 11, 11) == 1);
	result->_identifier = reader.ReadUInt32();
	result->_expectedFunctionTableLength = reader.ReadUInt32();

	auto token3 = reader.ReadUInt32();
	result->_tableLength = DecodeValue<uint16_t>(token3, 0, 15);
	result->_arrayLength = DecodeValue<uint16_t>(token3, 16, 31);

	for (uint16_t i = 0; i < result->_tableLength; i++)
		result->_functionTableIdentifiers.push_back(reader.ReadUInt32());

	return result;
}

bool InterfaceDeclarationToken::IsDynamicallyIndexed() const { return _isDynamicallyIndexed; }
uint32_t InterfaceDeclarationToken::GetIdentifier() const { return _identifier; }
uint32_t InterfaceDeclarationToken::GetExpectedFunctionTableLength() const { return _expectedFunctionTableLength; }
uint16_t InterfaceDeclarationToken::GetTableLength() const { return _tableLength; }
uint16_t InterfaceDeclarationToken::GetArrayLength() const { return _arrayLength; }
const std::vector<uint32_t>& InterfaceDeclarationToken::GetFunctionTableIdentifiers() const { return _functionTableIdentifiers; }

void InterfaceDeclarationToken::Print(std::ostream& out) const
{
	auto functionTableIdentifiers = from(_functionTableIdentifiers)
		.select([](uint32_t x) -> string { return "ft" + to_string(x); })
		.toVector();
	out << boost::format("%s fp%i[%i][%i] = {%s}")
		% GetTypeDescription() % _identifier % _arrayLength
		% _expectedFunctionTableLength
		% boost::algorithm::join(functionTableIdentifiers, ", ");
}