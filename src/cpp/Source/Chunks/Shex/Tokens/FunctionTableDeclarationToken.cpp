#include "PCH.h"
#include "FunctionTableDeclarationToken.h"

#include "Decoder.h"

using namespace boolinq;
using namespace std;
using namespace SlimShader;

shared_ptr<FunctionTableDeclarationToken> FunctionTableDeclarationToken::Parse(BytecodeReader& reader)
{
	auto token0 = reader.ReadUInt32();
	auto result = shared_ptr<FunctionTableDeclarationToken>(new FunctionTableDeclarationToken());
	result->_identifier = reader.ReadUInt32();

	auto tableLength = reader.ReadUInt32();
	for (uint32_t i = 0; i < tableLength; i++)
		result->_functionBodyIndices.push_back(reader.ReadUInt32());

	return result;
}

uint32_t FunctionTableDeclarationToken::GetIdentifier() const { return _identifier; }
const vector<uint32_t>& FunctionTableDeclarationToken::GetFunctionBodyIndices() const { return _functionBodyIndices; }

void FunctionTableDeclarationToken::Print(ostream& out) const
{
	auto functionBodyIndices = from(_functionBodyIndices)
		.select([](uint32_t x) -> string { return "fb" + to_string(x); })
		.toVector();
	out << GetTypeDescription() << " ft" << _identifier << " = {"
		<< boost::algorithm::join(functionBodyIndices, ", ") << "}";
}