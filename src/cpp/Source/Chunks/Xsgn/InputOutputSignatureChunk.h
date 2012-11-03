#pragma once

#include "PCH.h"
#include "DxbcChunk.h"
#include "ProgramType.h"
#include "SignatureParameterDescription.h"

namespace SlimShader
{
	class InputOutputSignatureChunk : public DxbcChunk
	{
	public :
		static std::shared_ptr<InputOutputSignatureChunk> Parse(BytecodeReader& reader, const ChunkType chunkType,
			const ProgramType programType);

		virtual ~InputOutputSignatureChunk() = 0; // Force InputOutputSignatureChunk to be abstract.

		const std::vector<SignatureParameterDescription>& GetParameters() const;
 
	protected :
		InputOutputSignatureChunk() {}

	private :
		std::vector<SignatureParameterDescription> _parameters;
	};

	inline InputOutputSignatureChunk::~InputOutputSignatureChunk() { }
};