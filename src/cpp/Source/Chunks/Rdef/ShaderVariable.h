#pragma once

#include "PCH.h"
#include "BytecodeReader.h"
#include "ShaderType.h"
#include "ShaderVariableFlags.h"
#include "ShaderVersion.h"

namespace SlimShader
{
	class ShaderVariable
	{
	public :
		static ShaderVariable Parse(
			const BytecodeReader& reader,
			BytecodeReader& variableReader,
			const ShaderVersion& target,
			bool isFirst);

		/// <summary>
		/// The variable name.
		/// </summary>
		const std::string& GetName() const;

		/// <summary>
		/// Offset from the start of the parent structure, to the beginning of the variable.
		/// </summary>
		uint32_t GetStartOffset() const;

		/// <summary>
		/// Get a shader-variable type.
		/// </summary>
		ShaderType GetShaderType() const;

		/// <summary>
		/// Gets the name of the base class.
		/// </summary>
		const std::string& GetBaseType() const;

		/// <summary>
		/// Size of the variable (in bytes).
		/// </summary>
		uint32_t GetSize() const;

		/// <summary>
		/// Flags, which identify shader-variable properties.
		/// </summary>
		ShaderVariableFlags GetFlags() const;

		/// <summary>
		/// The default value for initializing the variable.
		/// </summary>
		//public void* DefaultValue { get; private set; }

		int GetStartTexture() const;
		int GetTextureSize() const;
		int GetStartSampler() const;
		int GetSamplerSize() const;

		friend std::ostream& operator<<(std::ostream& out, const ShaderVariable& value);

	private :
		explicit ShaderVariable(ShaderTypeMember member);

		ShaderTypeMember _member;
		std::string _baseType;
		uint32_t _size;
		ShaderVariableFlags _flags;
		int _startTexture;
		int _textureSize;
		int _startSampler;
		int _samplerSize;
	};
};