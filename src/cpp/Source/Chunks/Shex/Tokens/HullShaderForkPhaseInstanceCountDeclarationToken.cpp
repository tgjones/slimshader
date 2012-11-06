#include "PCH.h"
#include "HullShaderForkPhaseInstanceCountDeclarationToken.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

shared_ptr<HullShaderForkPhaseInstanceCountDeclarationToken> HullShaderForkPhaseInstanceCountDeclarationToken::Parse(BytecodeReader& reader)
{
	auto token0 = reader.ReadUInt32();
	auto result = shared_ptr<HullShaderForkPhaseInstanceCountDeclarationToken>(new HullShaderForkPhaseInstanceCountDeclarationToken());
	result->_instanceCount = reader.ReadUInt32();
	return result;
}

uint32_t HullShaderForkPhaseInstanceCountDeclarationToken::GetInstanceCount() const { return _instanceCount; }

void HullShaderForkPhaseInstanceCountDeclarationToken::Print(std::ostream& out) const
{
	out << GetTypeDescription() << " " << _instanceCount;
}