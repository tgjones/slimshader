#include "PCH.h"
#include "TessellatorPartitioningDeclarationToken.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

shared_ptr<TessellatorPartitioningDeclarationToken> TessellatorPartitioningDeclarationToken::Parse(BytecodeReader& reader)
{
	auto token0 = reader.ReadUInt32();
	auto result = shared_ptr<TessellatorPartitioningDeclarationToken>(new TessellatorPartitioningDeclarationToken());
	result->_partitioning = DecodeValue<TessellatorPartitioning>(token0, 11, 13);
	return result;
};

TessellatorPartitioning TessellatorPartitioningDeclarationToken::GetPartitioning() const { return _partitioning; }

void TessellatorPartitioningDeclarationToken::Print(std::ostream& out) const
{
	out << GetTypeDescription() << " " << ToStringShex(_partitioning);
};