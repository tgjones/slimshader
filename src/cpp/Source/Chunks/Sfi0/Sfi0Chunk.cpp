#include "PCH.h"
#include "Sfi0Chunk.h"

using namespace std;
using namespace SlimShader;

shared_ptr<Sfi0Chunk> Sfi0Chunk::Parse(BytecodeReader& reader)
{
	assert(reader.ReadInt32() == 2); // TODO: Unknown
	return shared_ptr<Sfi0Chunk>(new Sfi0Chunk());
}