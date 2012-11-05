#pragma once

#include "PCH.h"
#include "ImmediateDeclarationToken.h"
#include "ShaderMessageFormat.h"

namespace SlimShader
{
	/// <summary>
	/// Shader Message Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_CUSTOMDATA
	/// [31:11] == D3D11_SB_CUSTOMDATA_SHADER_MESSAGE
	///
	/// OpcodeToken0 is followed by:
	/// (1) DWORD indicating length of declaration, including OpcodeToken0.
	/// (2) DWORD indicating the info queue message ID.
	/// (3) D3D11_SB_SHADER_MESSAGE_FORMAT indicating the convention for formatting the message.
	/// (4) DWORD indicating the number of characters in the string without the terminator.
	/// (5) DWORD indicating the number of operands.
	/// (6) DWORD indicating length of operands.
	/// (7) Encoded operands.
	/// (8) String with trailing zero, padded to a multiple of DWORDs.
	///     The string is in the given format and the operands given should
	///     be used for argument substitutions when formatting.
	/// </summary>
	class ShaderMessageDeclarationToken : public ImmediateDeclarationToken
	{
	public :
		static std::shared_ptr<ShaderMessageDeclarationToken> Parse(BytecodeReader& reader);

		/// <summary>
		/// Indicates the info queue message ID.
		/// </summary>
		uint32_t GetInfoQueueMessageID() const;

		/// <summary>
		/// Indicates the convention for formatting the message.
		/// </summary>
		ShaderMessageFormat GetMessageFormat() const;

		/// <summary>
		/// DWORD indicating the number of characters in the string without the terminator.
		/// </summary>
		uint32_t GetNumCharacters() const;

		/// <summary>
		/// DWORD indicating the number of operands.
		/// </summary>
		uint32_t GetNumOperands() const;

		/// <summary>
		/// DWORD indicating length of operands.
		/// </summary>
		uint32_t GetOperandsLength() const;

		// Not sure what format this is.
		void* GetEncodedOperands() const;

		/// <summary>
		/// String with trailing zero, padded to a multiple of DWORDs.
		/// The string is in the given format and the operands given should
		/// be used for argument substitutions when formatting.
		/// </summary>
		std::string GetFormat() const;

	protected :
		virtual void Print(std::ostream& out) const;

	private :
		uint32_t _infoQueueMessageID;
		ShaderMessageFormat _messageFormat;
		uint32_t _numCharacters;
		uint32_t _operandsLength;
		void* _encodedOperands;
		std::string _format;
	};
};