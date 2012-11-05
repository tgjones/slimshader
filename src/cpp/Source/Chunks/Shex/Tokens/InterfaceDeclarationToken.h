#pragma once

#include "PCH.h"
#include "DeclarationToken.h"

namespace SlimShader
{
	/// <summary>
	/// Interface Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_INTERFACE
	/// [11]    1 if the interface is indexed dynamically, 0 otherwise.
	/// [23:12] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  If it is extended, then
	///         it contains the actual instruction length in DWORDs, since
	///         it may not fit into 7 bits if enough types are used.
	///
	/// OpcodeToken0 is followed by a DWORD that represents the interface
	/// identifier. Next is a DWORD that gives the expected function table
	/// length. Then another DWORD (OpcodeToken3) with the following layout:
	///
	/// [15:00] TableLength, the number of types that implement this interface
	/// [31:16] ArrayLength, the number of interfaces that are defined in this array.
	///
	/// This is followed by TableLength DWORDs which are function table
	/// identifiers, representing possible tables for a given interface.
	/// </summary>
	class InterfaceDeclarationToken : public DeclarationToken
	{
	public :
		static std::shared_ptr<InterfaceDeclarationToken> Parse(BytecodeReader& reader);

		/// <summary>
		/// Returns true if the interface is indexed dynamically, otherwise false.
		/// </summary>
		bool IsDynamicallyIndexed() const;

		/// <summary>
		/// Gets the interface identifier.
		/// </summary>
		uint32_t GetIdentifier() const;

		/// <summary>
		/// Gets the expected function table length.
		/// </summary>
		uint32_t GetExpectedFunctionTableLength() const;

		/// <summary>
		/// Gets the number of types that implement this interface.
		/// </summary>
		uint16_t GetTableLength() const;

		/// <summary>
		/// Gets the number of interfaces that are defined in this array.
		/// </summary>
		uint16_t GetArrayLength() const;

		/// <summary>
		/// Gets the possible tables for a given interface.
		/// </summary>
		const std::vector<uint32_t>& GetFunctionTableIdentifiers() const;

	protected :
		virtual void Print(std::ostream& out) const;

	private :
		bool _isDynamicallyIndexed;
		uint32_t _identifier;
		uint32_t _expectedFunctionTableLength;
		uint16_t _tableLength;
		uint16_t _arrayLength;
		std::vector<uint32_t> _functionTableIdentifiers;
	};
};