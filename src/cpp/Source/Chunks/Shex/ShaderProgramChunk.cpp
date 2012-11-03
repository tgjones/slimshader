#include "PCH.h"
#include "ShaderProgramChunk.h"

using namespace std;
using namespace SlimShader;

shared_ptr<ShaderProgramChunk> ShaderProgramChunk::Parse(BytecodeReader& reader)
{
	return shared_ptr<ShaderProgramChunk>(new ShaderProgramChunk());
}