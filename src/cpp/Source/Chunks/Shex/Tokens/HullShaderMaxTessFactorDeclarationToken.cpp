#include "PCH.h"
#include "HullShaderMaxTessFactorDeclarationToken.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

shared_ptr<HullShaderMaxTessFactorDeclarationToken> HullShaderMaxTessFactorDeclarationToken::Parse(BytecodeReader& reader)
{
	auto token0 = reader.ReadUInt32();
	auto result = shared_ptr<HullShaderMaxTessFactorDeclarationToken>(new HullShaderMaxTessFactorDeclarationToken());
	result->_maxTessFactor = reader.ReadFloat();
	return result;
}

float HullShaderMaxTessFactorDeclarationToken::GetMaxTessFactor() const { return _maxTessFactor; }

void HullShaderMaxTessFactorDeclarationToken::Print(std::ostream& out) const
{
	
}