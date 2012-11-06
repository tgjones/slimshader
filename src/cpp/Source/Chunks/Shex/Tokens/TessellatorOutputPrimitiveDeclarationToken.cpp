#include "PCH.h"
#include "TessellatorOutputPrimitiveDeclarationToken.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

shared_ptr<TessellatorOutputPrimitiveDeclarationToken> TessellatorOutputPrimitiveDeclarationToken::Parse(BytecodeReader& reader)
{
	auto token0 = reader.ReadUInt32();
	auto result = shared_ptr<TessellatorOutputPrimitiveDeclarationToken>(new TessellatorOutputPrimitiveDeclarationToken());
	result->_outputPrimitive = DecodeValue<TessellatorOutputPrimitive>(token0, 11, 13);
	return result;
};

TessellatorOutputPrimitive TessellatorOutputPrimitiveDeclarationToken::GetOutputPrimitive() const { return _outputPrimitive; }

void TessellatorOutputPrimitiveDeclarationToken::Print(std::ostream& out) const
{
	out << GetTypeDescription() << " " << ToStringShex(_outputPrimitive);
};