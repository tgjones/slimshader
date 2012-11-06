#include "PCH.h"
#include "CustomDataToken.h"

#include "Decoder.h"
#include "ImmediateConstantBufferDeclarationToken.h"
#include "ShaderMessageDeclarationToken.h"

using namespace std;
using namespace SlimShader;

shared_ptr<CustomDataToken> CustomDataToken::Parse(BytecodeReader& reader, uint32_t token0)
{
	auto customDataClass = DecodeValue<CustomDataClass>(token0, 11, 31);
	shared_ptr<CustomDataToken> token;
	switch (customDataClass)
	{
	case CustomDataClass::DclImmediateConstantBuffer:
		token = ImmediateConstantBufferDeclarationToken::Parse(reader);
		break;
	case CustomDataClass::ShaderMessage:
		token = ShaderMessageDeclarationToken::Parse(reader);
		break;
	default:
		throw runtime_error("Unrecognised custom data class");
	}

	token->_customDataClass = customDataClass;
	return token;
}