#include "PCH.h"
#include "ShaderMessageDeclarationToken.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

shared_ptr<ShaderMessageDeclarationToken> ShaderMessageDeclarationToken::Parse(BytecodeReader& reader)
{
	auto token0 = reader.ReadUInt32();
	auto length = reader.ReadUInt32() - 2;

	auto result = shared_ptr<ShaderMessageDeclarationToken>(new ShaderMessageDeclarationToken());
	result->_declarationLength = length;
	result->_infoQueueMessageID = reader.ReadUInt32();
	result->_messageFormat = (ShaderMessageFormat) reader.ReadUInt32();
	result->_numCharacters = reader.ReadUInt32();
	result->_numOperands = reader.ReadUInt32();
	result->_operandsLength = reader.ReadUInt32();

	// TODO: Read encoded operands and format string.
	for (uint32_t i = 0; i < length - 5; i++)
		reader.ReadUInt32();

	return result;
}

uint32_t ShaderMessageDeclarationToken::GetInfoQueueMessageID() const { return _infoQueueMessageID; }
ShaderMessageFormat ShaderMessageDeclarationToken::GetMessageFormat() const { return _messageFormat; }
uint32_t ShaderMessageDeclarationToken::GetNumCharacters() const { return _numCharacters; }
uint32_t ShaderMessageDeclarationToken::GetNumOperands() const { return _numOperands; }
uint32_t ShaderMessageDeclarationToken::GetOperandsLength() const { return _operandsLength; }
void* ShaderMessageDeclarationToken::GetEncodedOperands() const { return _encodedOperands; }
string ShaderMessageDeclarationToken::GetFormat() const { return _format; }

void ShaderMessageDeclarationToken::Print(std::ostream& out) const
{
	
}