#include "PCH.h"
#include "DebuggingChunk.h"

using namespace std;
using namespace SlimShader;

shared_ptr<DebuggingChunk> DebuggingChunk::Parse(BytecodeReader& reader)
{
	return shared_ptr<DebuggingChunk>(new DebuggingChunk());
}