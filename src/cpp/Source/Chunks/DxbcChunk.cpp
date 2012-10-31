#include "PCH.h"
#include "DebuggingChunk.h"
#include "DxbcChunk.h"
#include "InterfacesChunk.h"
#include "InputOutputSignatureChunk.h"
#include "ResourceDefinitionChunk.h"
#include "Sfi0Chunk.h"
#include "ShaderProgramChunk.h"
#include "StatisticsChunk.h"

using namespace SlimShader;

DxbcChunk DxbcChunk::Parse(const shared_ptr<BytecodeReader> chunkReader, shared_ptr<DxbcContainer> container)
{
	// Type of chunk this is.
	auto fourCc = chunkReader->ReadUInt32();

	// Total length of the chunk in bytes.
	auto chunkSize = chunkReader->ReadUInt32();

	ChunkType chunkType = static_cast<ChunkType>(fourCc);

	shared_ptr<BytecodeReader> chunkContentReader = make_shared<BytecodeReader>(chunkReader, chunkSize);
	DxbcChunk chunk;
	switch (chunkType)
	{
	case ChunkType::Ifce :
		chunk = InterfacesChunk::Parse(chunkContentReader, chunkSize);
		break;
	case ChunkType::Isgn :
	case ChunkType::Osgn:
	//case ChunkType::Osg5: // Doesn't seem to be used?
	case ChunkType::Pcsg:
		chunk = InputOutputSignatureChunk::Parse(chunkContentReader, chunkType,
			container->GetResourceDefinition()->GetTarget()->GetProgramType());
		break;
	case ChunkType::Rdef:
		chunk = ResourceDefinitionChunk::Parse(chunkContentReader);
		break;
	case ChunkType::Sdbg :
		chunk = DebuggingChunk::Parse(chunkContentReader);
		break;
	case ChunkType::Sfi0:
		chunk = Sfi0Chunk::Parse(chunkContentReader);
		break;
	case ChunkType::Shdr:
	case ChunkType::Shex:
		chunk = ShaderProgramChunk::Parse(chunkContentReader);
		break;
	case ChunkType::Stat:
		chunk = StatisticsChunk::Parse(chunkContentReader, chunkSize);
		break;
	default :
		throw std::runtime_error("Chunk type '" + to_string(fourCc) + "' is not yet supported.");
	}

	chunk._fourCc = fourCc;
	chunk._chunkSize = chunkSize;
	chunk._chunkType = chunkType;

	return chunk;
}