#include "PCH.h"
#include "TempRegisterDeclarationToken.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

shared_ptr<TempRegisterDeclarationToken> TempRegisterDeclarationToken::Parse(BytecodeReader& reader)
{
	auto token0 = reader.ReadUInt32();
	auto result = shared_ptr<TempRegisterDeclarationToken>(new TempRegisterDeclarationToken());
	result->_tempCount = reader.ReadUInt32();
	return result;
};

uint32_t TempRegisterDeclarationToken::GetTempCount() const { return _tempCount; }

void TempRegisterDeclarationToken::Print(std::ostream& out) const
{
	out << GetTypeDescription() << " " << _tempCount;
};