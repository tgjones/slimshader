#pragma once

#include "PCH.h"
#include "BytecodeReader.h"
#include "ShaderVersion.h"

namespace SlimShader
{
	class ShaderType;

	class ShaderTypeMember
	{
	public :
		static ShaderTypeMember Parse(
			const BytecodeReader& reader,
			BytecodeReader& memberReader,
			const ShaderVersion& target,
			int indent,
			bool isFirst,
			uint32_t parentOffset);

		ShaderTypeMember(
			uint32_t parentOffset,
			std::string name,
			uint32_t offset,
			std::shared_ptr<ShaderType> type);

		/// <summary>
		/// The variable name.
		/// </summary>
		const std::string& GetName() const;

		/// <summary>
		/// Offset from the start of the parent structure, to the beginning of the variable.
		/// </summary>
		uint32_t GetOffset() const;

		/// <summary>
		/// Get a shader-variable type.
		/// </summary>
		const ShaderType& GetType() const;

		friend std::ostream& operator<<(std::ostream& out, const ShaderTypeMember& value);

	private :
		const uint32_t _parentOffset;
		std::string _name;
		uint32_t _offset;
		std::shared_ptr<ShaderType> _type; // Need to use pointer to avoid circular class declaration
	};
};