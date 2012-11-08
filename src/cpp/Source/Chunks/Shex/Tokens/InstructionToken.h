#pragma once

#include "PCH.h"
#include "BytecodeReader.h"
#include "InstructionTestBoolean.h"
#include "InstructionTokenExtendedType.h"
#include "OpcodeToken.h"
#include "Operand.h"
#include "ResourceDimension.h"
#include "ResourceReturnType.h"
#include "SyncFlags.h"

namespace SlimShader
{
	/// <summary>
	/// Instruction Token
	///
	/// Normal instructions:
	/// [10:00] OpcodeType
	/// [12:11] resinfo_return_type TODO
	/// [13:13] Saturate
	/// [17:14] Ignored, 0
	/// [18:18] InstructionTestBoolean
	/// [22:19] precise_mask TODO
	/// [23:23] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.
	/// 
	/// OpcodeType == Sync:
	/// [11:11] ThreadsInGroup
	/// [12:12] SharedMemory
	/// [13:13] UavGroup
	/// [14:14] UavGlobal
	///
	/// OpcodeToken0 is followed by 1 or more operands.
	/// </summary>
	class InstructionToken : public OpcodeToken
	{
	public :
		static std::shared_ptr<InstructionToken> Parse(BytecodeReader& reader, const OpcodeHeader& header);

		const Operand& GetOperand(uint8_t index) const;

	protected :
		virtual void Print(std::ostream& out) const;

	private :
		InstructionToken();

		bool _saturate;
		InstructionTestBoolean _testBoolean;
		SyncFlags _syncFlags;
		std::vector<InstructionTokenExtendedType> _extendedTypes;
		int8_t _sampleOffsets[3];
		ResourceDimension _resourceTarget;
		uint8_t _resourceStride;
		ResourceReturnType _resourceReturnTypes[4];
		uint32_t _functionIndex;
		std::vector<Operand> _operands;
	};
};