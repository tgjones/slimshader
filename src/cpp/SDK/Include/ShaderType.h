#pragma once

#include "PCH.h"
#include "ShaderVariableClass.h"
#include "ShaderVariableType.h"
#include "ShaderTypeMember.h"

namespace SlimShader
{
	/// <summary>
	/// Describes a shader-variable type.
	/// Based on D3D11_SHADER_TYPE_DESC.
	/// </summary>
	class ShaderType
	{
	public :
		static std::shared_ptr<ShaderType> Parse(
			const BytecodeReader& reader, 
			BytecodeReader& typeReader,
			const ShaderVersion& target,
			int indent,
			bool isFirst,
			uint32_t parentOffset);

		/// <summary>
		/// Identifies the variable class as one of scalar, vector, matrix or object.
		/// </summary>
		ShaderVariableClass GetVariableClass() const;

		/// <summary>
		/// The variable type.
		/// </summary>
		ShaderVariableType GetVariableType() const;

		/// <summary>
		/// Number of rows in a matrix. Otherwise a numeric type returns 1, any other type returns 0.
		/// </summary>
		uint16_t GetRows() const;

		/// <summary>
		/// Number of columns in a matrix. Otherwise a numeric type returns 1, any other type returns 0.
		/// </summary>
		uint16_t GetColumns() const;

		/// <summary>
		/// Number of elements in an array; otherwise 0.
		/// </summary>
		uint16_t GetElementCount() const;

		const std::vector<ShaderTypeMember>& GetMembers() const;

		const std::string& GetBaseTypeName() const;

		friend std::ostream& operator<<(std::ostream& out, const ShaderType& value);

	private :
		ShaderType(int indent, bool isFirst);

		const int _indent;
		const bool _isFirst;
		ShaderVariableClass _variableClass;
		ShaderVariableType _variableType;
		uint16_t _rows;
		uint16_t _columns;
		uint16_t _elementCount;
		std::vector<ShaderTypeMember> _members;
		std::string _baseTypeName;
	};
};