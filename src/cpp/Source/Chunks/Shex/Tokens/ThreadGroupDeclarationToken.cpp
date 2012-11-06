#include "PCH.h"
#include "ThreadGroupDeclarationToken.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

shared_ptr<ThreadGroupDeclarationToken> ThreadGroupDeclarationToken::Parse(BytecodeReader& reader)
{
	auto token0 = reader.ReadUInt32();
	auto result = shared_ptr<ThreadGroupDeclarationToken>(new ThreadGroupDeclarationToken());
	result->_dimensions[0] = reader.ReadUInt32();
	result->_dimensions[1] = reader.ReadUInt32();
	result->_dimensions[2] = reader.ReadUInt32();
	return result;
};

const uint32_t* ThreadGroupDeclarationToken::GetDimensions() const { return _dimensions; }

void ThreadGroupDeclarationToken::Print(std::ostream& out) const
{
	out << GetTypeDescription() << " " << _dimensions[0] << ", " << _dimensions[1] << ", " << _dimensions[2];
};