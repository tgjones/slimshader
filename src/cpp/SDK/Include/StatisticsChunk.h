#pragma once

#include "PCH.h"
#include "DxbcChunk.h"
#include "Primitive.h"
#include "PrimitiveTopology.h"
#include "TessellatorDomain.h"
#include "TessellatorOutputPrimitive.h"
#include "TessellatorPartitioning.h"

namespace SlimShader
{
	/// <summary>
	/// Statistics chunk. Thanks to Wine for the hard work in decoding this.
	/// http://source.winehq.org/source/dlls/d3dcompiler_43/reflection.c#L1061
	/// Based on D3D11_SHADER_DESC.
	/// </summary>
	class StatisticsChunk : public DxbcChunk
	{
	public :
		static std::shared_ptr<StatisticsChunk> Parse(BytecodeReader& reader, const uint32_t chunkSize);

		/// <summary>
		/// The number of intermediate-language instructions in the compiled shader.
		/// </summary>
		uint32_t GetInstructionCount() const;

		/// <summary>
		/// The number of temporary registers in the compiled shader.
		/// </summary>
		uint32_t GetTempRegisterCount() const;

		/// <summary>
		/// Number of temporary arrays used.
		/// </summary>
		uint32_t GetTempArrayCount() const;

		/// <summary>
		/// Number of constant defines.
		/// </summary>
		uint32_t GetDefineCount() const;

		/// <summary>
		/// Number of declarations (input + output).
		/// </summary>
		uint32_t GetDeclarationCount() const;

		/// <summary>
		/// Number of non-categorized texture instructions.
		/// </summary>
		uint32_t GetTextureNormalInstructions() const;

		/// <summary>
		/// Number of texture load instructions
		/// </summary>
		uint32_t GetTextureLoadInstructions() const;

		/// <summary>
		/// Number of texture comparison instructions
		/// </summary>
		uint32_t GetTextureCompInstructions() const;

		/// <summary>
		/// Number of texture bias instructions
		/// </summary>
		uint32_t GetTextureBiasInstructions() const;

		/// <summary>
		/// Number of texture gradient instructions.
		/// </summary>
		uint32_t GetTextureGradientInstructions() const;

		/// <summary>
		/// Number of floating point arithmetic instructions used.
		/// </summary>
		uint32_t GetFloatInstructionCount() const;

		/// <summary>
		/// Number of signed integer arithmetic instructions used.
		/// </summary>
		uint32_t GetIntInstructionCount() const;

		/// <summary>
		/// Number of unsigned integer arithmetic instructions used.
		/// </summary>
		uint32_t GetUIntInstructionCount() const;

		/// <summary>
		/// Number of static flow control instructions used.
		/// </summary>
		uint32_t GetStaticFlowControlCount() const;

		/// <summary>
		/// Number of dynamic flow control instructions used.
		/// </summary>
		uint32_t GetDynamicFlowControlCount() const;

		/// <summary>
		/// Number of macro instructions used.
		/// </summary>
		uint32_t GetMacroInstructionCount() const;

		/// <summary>
		/// Number of array instructions used.
		/// </summary>
		uint32_t GetArrayInstructionCount() const;

		/// <summary>
		/// Number of cut instructions used.
		/// </summary>
		uint32_t GetCutInstructionCount() const;

		/// <summary>
		/// Number of emit instructions used.
		/// </summary>
		uint32_t GetEmitInstructionCount() const;

		/// <summary>
		/// The <see cref="PrimitiveTopology" />-typed value that represents the geometry shader output topology.
		/// </summary>
		PrimitiveTopology GetGeometryShaderOutputTopology() const;

		/// <summary>
		/// Geometry shader maximum output vertex count.
		/// </summary>
		uint32_t GetGeometryShaderMaxOutputVertexCount() const;

		/// <summary>
		/// The <see cref="Primitive"/>-typed value that represents the input primitive for a geometry shader or hull shader.
		/// </summary>
		Primitive GetInputPrimitive() const;

		/// <summary>
		/// Number of geometry shader instances.
		/// </summary>
		uint32_t GetGeometryShaderInstanceCount() const;

		/// <summary>
		/// Number of control points in the hull shader and domain shader.
		/// </summary>
		uint32_t GetControlPoints() const;

		/// <summary>
		/// The <see cref="TessellatorOutputPrimitive"/>-typed value that represents the tessellator output-primitive type.
		/// </summary>
		TessellatorOutputPrimitive GetHullShaderOutputPrimitive() const;

		/// <summary>
		/// The <see cref="TessellatorPartitioning" />-typed value that represents the tessellator partitioning mode.
		/// </summary>
		TessellatorPartitioning GetHullShaderPartitioning() const;

		/// <summary>
		/// The <see cref="TessellatorDomain"/>-typed value that represents the tessellator domain.
		/// </summary>
		TessellatorDomain GetTessellatorDomain() const;

		/// <summary>
		/// Number of barrier instructions in a compute shader.
		/// </summary>
		uint32_t GetBarrierInstructions() const;

		/// <summary>
		/// Number of interlocked instructions in a compute shader.
		/// </summary>
		uint32_t GetInterlockedInstructions() const;

		/// <summary>
		/// Number of texture writes in a compute shader.
		/// </summary>
		uint32_t GetTextureStoreInstructions() const;

		uint32_t GetMovInstructionCount() const;
		uint32_t GetMovCInstructionCount() const;
		uint32_t GetUnknown3() const;
		uint32_t GetConversionInstructionCount() const;
		uint32_t GetUnknown4() const;
		uint32_t GetUnknown5() const;
		uint32_t GetUnknown6() const;
		uint32_t GetUnknown7() const;

		friend std::ostream& operator<<(std::ostream& out, const StatisticsChunk& chunk);

	private :
		StatisticsChunk();

		uint32_t _instructionCount;
		uint32_t _tempRegisterCount;
		uint32_t _tempArrayCount;
		uint32_t _defineCount;
		uint32_t _declarationCount;
		uint32_t _textureNormalInstructions;
		uint32_t _textureLoadInstructions;
		uint32_t _textureCompInstructions;
		uint32_t _textureBiasInstructions;
		uint32_t _textureGradientInstructions;
		uint32_t _floatInstructionCount;
		uint32_t _intInstructionCount;
		uint32_t _uintInstructionCount;
		uint32_t _staticFlowControlCount;
		uint32_t _dynamicFlowControlCount;
		uint32_t _macroInstructionCount;
		uint32_t _arrayInstructionCount;
		uint32_t _cutInstructionCount;
		uint32_t _emitInstructionCount;
		PrimitiveTopology _geometryShaderOutputTopology;
		uint32_t _geometryShaderMaxOutputVertexCount;
		Primitive _inputPrimitive;
		uint32_t _geometryShaderInstanceCount;
		uint32_t _controlPoints;
		TessellatorOutputPrimitive _hullShaderOutputPrimitive;
		TessellatorPartitioning _hullShaderPartitioning;
		TessellatorDomain _tessellatorDomain;
		uint32_t _barrierInstructions;
		uint32_t _interlockedInstructions;
		uint32_t _textureStoreInstructions;
		uint32_t _movInstructionCount;
		uint32_t _movCInstructionCount;
		uint32_t _unknown3;
		uint32_t _conversionInstructionCount;
		uint32_t _unknown4;
		uint32_t _unknown5;
		uint32_t _unknown6;
		uint32_t _unknown7;
	};
};