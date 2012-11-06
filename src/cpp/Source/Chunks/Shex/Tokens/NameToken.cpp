#include "PCH.h"
#include "NameToken.h"

#include "Decoder.h"

using namespace SlimShader;

SystemValueName NameToken::Parse(BytecodeReader& reader)
{
	auto token = reader.ReadUInt32();
	return DecodeValue<SystemValueName>(token, 0, 15);
}