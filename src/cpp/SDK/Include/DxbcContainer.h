#pragma once

#include "PCH.h"
#include "BytecodeReader.h"
#include "DxbcContainerHeader.h"

namespace SlimShader
{
	class DxbcChunk;
	class ResourceDefinitionChunk;
	class PatchConstantSignatureChunk;
	class InputSignatureChunk;
	class OutputSignatureChunk;
	class ShaderProgramChunk;
	class StatisticsChunk;
	class InterfacesChunk;

	class DxbcContainer
	{
	public :
		static DxbcContainer Parse(const std::vector<char> bytes);
		static DxbcContainer Parse(BytecodeReader& reader);

		const std::shared_ptr<ResourceDefinitionChunk> GetResourceDefinition() const;
		const std::shared_ptr<PatchConstantSignatureChunk> GetPatchConstantSignature() const;
		const std::shared_ptr<InputSignatureChunk> GetInputSignature() const;
		const std::shared_ptr<OutputSignatureChunk> GetOutputSignature() const;
		const std::shared_ptr<ShaderProgramChunk> GetShader() const;
		const std::shared_ptr<StatisticsChunk> GetStatistics() const;
		const std::shared_ptr<InterfacesChunk> GetInterfaces() const;

		friend std::ostream& operator<<(std::ostream &out, const DxbcContainer &container);

	private :
		DxbcContainerHeader _header;
		std::vector<std::shared_ptr<DxbcChunk>> _chunks;
	};
};