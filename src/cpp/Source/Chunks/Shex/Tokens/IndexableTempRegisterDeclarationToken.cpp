#include "PCH.h"
#include "IndexableTempRegisterDeclarationToken.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

shared_ptr<IndexableTempRegisterDeclarationToken> IndexableTempRegisterDeclarationToken::Parse(BytecodeReader& reader)
{
	auto token0 = reader.ReadUInt32();
	auto result = shared_ptr<IndexableTempRegisterDeclarationToken>(new IndexableTempRegisterDeclarationToken());
	result->_registerIndex = reader.ReadUInt32();
	result->_registerCount = reader.ReadUInt32();
	result->_numComponents = reader.ReadUInt32();
	return result;
}

uint32_t IndexableTempRegisterDeclarationToken::GetRegisterIndex() const { return _registerIndex; }
uint32_t IndexableTempRegisterDeclarationToken::GetRegisterCount() const { return _registerCount; }
uint32_t IndexableTempRegisterDeclarationToken::GetNumComponents() const { return _numComponents; }

void IndexableTempRegisterDeclarationToken::Print(std::ostream& out) const
{
	out << boost::format("%s x%i[%i], %i")
		% GetTypeDescription()
		% _registerIndex
		% _registerCount
		% _numComponents;
}