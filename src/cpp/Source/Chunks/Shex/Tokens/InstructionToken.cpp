#include "PCH.h"
#include "InstructionToken.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

shared_ptr<InstructionToken> InstructionToken::Parse(BytecodeReader& reader, const OpcodeHeader& header)
{
	auto instructionToken = shared_ptr<InstructionToken>(new InstructionToken());

	// Advance to next token.
	auto instructionEnd = reader.GetCurrentPosition() + (header.Length * sizeof(uint32_t));
	auto token0 = reader.ReadUInt32();

	if (header.OpcodeType == OpcodeType::Sync)
	{
		instructionToken->_syncFlags = DecodeValue<SyncFlags>(token0, 11, 14);
	}
	else
	{
		instructionToken->_saturate = (DecodeValue(token0, 13, 13) == 1);
		instructionToken->_testBoolean = DecodeValue<InstructionTestBoolean>(token0, 18, 18);
	}

	bool extended = header.IsExtended;
	while (extended)
	{
		auto extendedToken = reader.ReadUInt32();
		auto extendedType = DecodeValue<InstructionTokenExtendedType>(extendedToken, 0, 5);
		instructionToken->_extendedTypes.push_back(extendedType);
		extended = (DecodeValue(extendedToken, 31, 31) == 1);

		switch (extendedType)
		{
		case InstructionTokenExtendedType::SampleControls:
			instructionToken->_sampleOffsets[0] = DecodeSigned4BitValue(extendedToken, 9, 12);
			instructionToken->_sampleOffsets[1] = DecodeSigned4BitValue(extendedToken, 13, 16);
			instructionToken->_sampleOffsets[2] = DecodeSigned4BitValue(extendedToken, 17, 20);
			break;
		case InstructionTokenExtendedType::ResourceDim:
			instructionToken->_resourceTarget = DecodeValue<ResourceDimension>(extendedToken, 6, 10);
			instructionToken->_resourceStride = DecodeValue<uint8_t>(extendedToken, 11, 15);
			break;
		case InstructionTokenExtendedType::ResourceReturnType:
			instructionToken->_resourceReturnTypes[0] = DecodeValue<ResourceReturnType>(extendedToken, 6, 9);
			instructionToken->_resourceReturnTypes[1] = DecodeValue<ResourceReturnType>(extendedToken, 10, 13);
			instructionToken->_resourceReturnTypes[2] = DecodeValue<ResourceReturnType>(extendedToken, 14, 17);
			instructionToken->_resourceReturnTypes[3] = DecodeValue<ResourceReturnType>(extendedToken, 18, 21); 
			break;
		default:
			throw new runtime_error("Unrecognised extended type");
		}
	}

	if (header.OpcodeType == OpcodeType::InterfaceCall)
	{
		// Interface call
		//
		// OpcodeToken0:
		//
		// [10:00] D3D10_SB_OPCODE_INTERFACE_CALL
		// [23:11] Ignored, 0
		// [30:24] Instruction length in DWORDs including the opcode token.
		// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
		//         contains extended operand description.  If it is extended, then
		//         it contains the actual instruction length in DWORDs, since
		//         it may not fit into 7 bits if enough types are used.
		//
		// OpcodeToken0 is followed by a DWORD that gives the function index to
		// call in the function table specified for the given interface. 
		// Next is the interface operand.
		instructionToken->_functionIndex = reader.ReadUInt32();
	}

	while (reader.GetCurrentPosition() < instructionEnd)
		instructionToken->_operands.push_back(Operand::Parse(reader, header.OpcodeType));

	return instructionToken;
}

template <typename T>
bool Contains(vector<T> vector, T value)
{
	return find(vector.begin(), vector.end(), value) != vector.end();
}

void InstructionToken::Print(ostream& out) const
{
	out << GetTypeDescription();

	if (GetHeader().OpcodeType == OpcodeType::Sync)
		out << ToString(_syncFlags);

	if (Contains(_extendedTypes, InstructionTokenExtendedType::ResourceDim))
		out << "_indexable";

	if (Contains(_extendedTypes, InstructionTokenExtendedType::SampleControls))
		out << boost::format("(%i,%i,%i)") 
			% (int32_t) _sampleOffsets[0] 
			% (int32_t) _sampleOffsets[1]
			% (int32_t) _sampleOffsets[2];

	if (Contains(_extendedTypes, InstructionTokenExtendedType::ResourceDim))
	{
		out << "(" << ToString(_resourceTarget);
		if (_resourceStride != 0)
			out << ", stride=" << (uint32_t) _resourceStride;
		out << ")";
	}

	if (Contains(_extendedTypes, InstructionTokenExtendedType::ResourceReturnType))
		out << boost::format("(%s,%s,%s,%s)")
			% ToString(_resourceReturnTypes[0])
			% ToString(_resourceReturnTypes[1])
			% ToString(_resourceReturnTypes[2])
			% ToString(_resourceReturnTypes[3]);

	if (IsConditionalInstruction(GetHeader().OpcodeType))
		out << "_" + ToString(_testBoolean);

	if (_saturate)
		out << "_sat";
	out << " ";

	if (GetHeader().OpcodeType == OpcodeType::InterfaceCall)
	{
		out << "fp" << _operands[0].GetIndices()[0].Value
			<< "[" << _operands[0].GetIndices()[1] << "]"
			<< "[" << _functionIndex << "]";
	}
	else
	{
		for (size_t i = 0; i < _operands.size(); i++)
		{
			out << _operands[i];
			if (i < _operands.size() - 1)
				out << ", ";
		}
	}
}

InstructionToken::InstructionToken() :
	_saturate(false),
	_testBoolean(InstructionTestBoolean::Zero),
	_syncFlags(SyncFlags::None)
{

}