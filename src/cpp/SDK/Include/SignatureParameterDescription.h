#pragma once

#include "PCH.h"
#include "BytecodeReader.h"
#include "ChunkType.h"
#include "ComponentMask.h"
#include "Name.h"
#include "ProgramType.h"
#include "RegisterComponentType.h"
#include "SignatureElementSize.h"

namespace SlimShader
{
	/// <summary>
	/// Describes a shader signature.
	/// Based on D3D11_SIGNATURE_PARAMETER_DESC.
	/// </summary>
	class SignatureParameterDescription
	{
	public :
		static SignatureParameterDescription Parse(const BytecodeReader& reader, BytecodeReader& parameterReader,
			ChunkType chunkType, SignatureElementSize size, ProgramType programType);

		/// <summary>
		/// A per-parameter string that identifies how the data will be used.
		/// </summary>
		const std::string& GetSemanticName() const;

		/// <summary>
		/// Semantic index that modifies the semantic. Used to differentiate different parameters that use the same 
		/// semantic.
		/// </summary>
		uint32_t GetSemanticIndex() const;

		/// <summary>
		/// The register that will contain this variable's data.
		/// </summary>
		uint32_t GetRegister() const;

		/// <summary>
		/// A <see cref="SystemValueName"/>-typed value that identifies a predefined string that determines the 
		/// functionality of certain pipeline stages.
		/// </summary>
		Name GetSystemValueType() const;

		/// <summary>
		/// A <see cref="RegisterComponentType"/>-typed value that identifies the per-component-data type that is 
		/// stored in a register. Each register can store up to four-components of data.
		/// </summary>
		RegisterComponentType GetComponentType() const;

		/// <summary>
		/// Mask which indicates which components of a register are used.
		/// </summary>
		ComponentMask GetMask() const;

		/// <summary>
		/// Mask which indicates whether a given component is never written (if the signature is an output signature) 
		/// or always read (if the signature is an input signature).
		/// </summary>
		ComponentMask GetReadWriteMask() const;

		/// <summary>
		/// Indicates which stream the geometry shader is using for the signature parameter.
		/// </summary>
		uint32_t GetStream() const;

		friend std::ostream& operator<<(std::ostream& out, const SignatureParameterDescription& value);

	private :
		SignatureParameterDescription() { }

		std::string _semanticName;
		uint32_t _semanticIndex;
		uint32_t _register;
		Name _systemValueType;
		RegisterComponentType _componentType;
		ComponentMask _mask;
		ComponentMask _readWriteMask;
		uint32_t _stream;
	};
};