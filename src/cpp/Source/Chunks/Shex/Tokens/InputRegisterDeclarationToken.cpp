#include "PCH.h"
#include "InputRegisterDeclarationToken.h"

#include "Decoder.h"
#include "NameToken.h"
#include "PixelShaderInputRegisterDeclarationToken.h"

using namespace std;
using namespace SlimShader;

shared_ptr<InputRegisterDeclarationToken> InputRegisterDeclarationToken::Parse(BytecodeReader& reader)
{
	auto token0 = reader.ReadUInt32();
	auto opcodeType = DecodeValue<OpcodeType>(token0, 0, 10);

	shared_ptr<InputRegisterDeclarationToken> result;
	switch (opcodeType)
	{
	case OpcodeType::DclInput:
	case OpcodeType::DclInputSgv:
	case OpcodeType::DclInputSiv:
		{
			auto operand = Operand::Parse(reader, opcodeType);
			result = shared_ptr<InputRegisterDeclarationToken>(new InputRegisterDeclarationToken(operand));
			break;
		}
	case OpcodeType::DclInputPs:
	case OpcodeType::DclInputPsSgv:
	case OpcodeType::DclInputPsSiv:
		{
			auto interpolationMode = DecodeValue<InterpolationMode>(token0, 11, 14);
			auto operand = Operand::Parse(reader, opcodeType);
			result = shared_ptr<PixelShaderInputRegisterDeclarationToken>(new PixelShaderInputRegisterDeclarationToken(interpolationMode, operand));
			break;
		}
	default:
		throw runtime_error("Unreognised opcode type");
	}

	switch (opcodeType)
	{
	case OpcodeType::DclInputSgv:
	case OpcodeType::DclInputSiv:
	case OpcodeType::DclInputPsSgv:
	case OpcodeType::DclInputPsSiv:
		result->_systemValueName = NameToken::Parse(reader);
		break;
	}

	return result;
}

SystemValueName InputRegisterDeclarationToken::GetSystemValueName() const { return _systemValueName; }

void InputRegisterDeclarationToken::Print(ostream& out) const
{
	out << GetTypeDescription() << " " << GetOperand();

	if (GetHeader().OpcodeType == OpcodeType::DclInputSgv || GetHeader().OpcodeType == OpcodeType::DclInputSiv)
		out << ", " + ToString(_systemValueName);
}

InputRegisterDeclarationToken::InputRegisterDeclarationToken(Operand operand)
	: DeclarationToken(operand)
{

}