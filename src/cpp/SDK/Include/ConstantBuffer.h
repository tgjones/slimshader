#pragma once

#include "PCH.h"
#include "BytecodeReader.h"
#include "ConstantBufferFlags.h"
#include "ConstantBufferType.h"
#include "ShaderVersion.h"
#include "ShaderVariable.h"

namespace SlimShader
{
	/// <summary>
	/// Describes a shader constant-buffer.
	/// Based on D3D11_SHADER_BUFFER_DESC.
	/// </summary>
	class ConstantBuffer
	{
	public :
		static ConstantBuffer Parse(const BytecodeReader& reader, BytecodeReader& constantBufferReader,
			const ShaderVersion& target);

		/// <summary>
		/// The name of the buffer.
		/// </summary>
		const std::string& GetName() const;

		/// <summary>
		/// A <see cref="ConstantBufferType" />-typed value that indicates the intended use of the constant data.
		/// </summary>
		ConstantBufferType GetBufferType() const;

		const std::vector<ShaderVariable>& GetVariables() const;

		/// <summary>
		/// Buffer size (in bytes).
		/// </summary>
		uint32_t GetSize() const;

		/// <summary>
		/// A combination of <see cref="ConstantBufferFlags" />-typed values that are combined by using a bitwise OR 
		/// operation. The resulting value specifies properties for the shader constant-buffer.
		/// </summary>
		ConstantBufferFlags GetFlags() const;

		friend std::ostream& operator<<(std::ostream& out, const ConstantBuffer& value);

	private :
		ConstantBuffer() { }

		std::string _name;
		ConstantBufferType _bufferType;
		std::vector<ShaderVariable> _variables;
		uint32_t _size;
		ConstantBufferFlags _flags;
	};
};