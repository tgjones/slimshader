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

		const std::vector<SignatureParameterDescription>& GetParameters() const;

		friend std::ostream& operator<<(std::ostream& out, const InputOutputSignatureChunk& value);
 
	protected :
		InputOutputSignatureChunk() { }

		virtual std::string GetOutputDescription() const = 0;

	private :
		std::vector<SignatureParameterDescription> _parameters;
	};
};