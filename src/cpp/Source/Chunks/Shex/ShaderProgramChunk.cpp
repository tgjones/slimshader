#include "PCH.h"
#include "ShaderProgramChunk.h"

#include "CustomDataToken.h"
#include "DeclarationToken.h"
#include "Decoder.h"
#include "InstructionToken.h"

using namespace std;
using namespace SlimShader;

shared_ptr<ShaderProgramChunk> ShaderProgramChunk::Parse(BytecodeReader& reader)
{
	auto version = ShaderVersion::ParseShex(reader);
	auto program = shared_ptr<ShaderProgramChunk>(new ShaderProgramChunk(version));
	
	// Length Token (LenTok)
	// Always follows VerTok
	// [31:00] Unsigned integer count of number of DWORDs in program code, including version and length tokens.
	// So the minimum value is 0x00000002 (if an empty program is ever valid).
	program->_length = reader.ReadUInt32();

	while (!reader.IsEndOfBuffer())
	{
		// Opcode Format (OpcodeToken0)
		//
		// [10:00] D3D10_SB_OPCODE_TYPE
		// if( [10:00] == D3D10_SB_OPCODE_CUSTOMDATA )
		// {
		//    Token starts a custom-data block.  See "Custom-Data Block Format".
		// }
		// else // standard opcode token
		// {
		//    [23:11] Opcode-Specific Controls
		//    [30:24] Instruction length in DWORDs including the opcode token.
		//    [31]    0 normally. 1 if extended operand definition, meaning next DWORD
		//            contains extended opcode token.
		// }
		BytecodeReader opcodeHeaderReader(reader);
		auto opcodeToken0 = opcodeHeaderReader.ReadUInt32();

		OpcodeHeader opcodeHeader;
		opcodeHeader.OpcodeType = DecodeValue<OpcodeType>(opcodeToken0, 0, 10);
		opcodeHeader.Length = DecodeValue(opcodeToken0, 24, 30);
		opcodeHeader.IsExtended = (DecodeValue(opcodeToken0, 31, 31) == 1);

		shared_ptr<OpcodeToken> opcodeToken;
		if (opcodeHeader.OpcodeType == OpcodeType::CustomData)
		{
			opcodeToken = CustomDataToken::Parse(reader, opcodeToken0);
		}
		else if (IsDeclaration(opcodeHeader.OpcodeType))
		{
			opcodeToken = DeclarationToken::Parse(reader, opcodeHeader.OpcodeType);
		}
		else // Not custom data or declaration, so must be instruction.
		{
			opcodeToken = InstructionToken::Parse(reader, opcodeHeader);
		}

		opcodeToken->SetHeader(opcodeHeader);
		program->_tokens.push_back(opcodeToken);
	}

	return program;
}

bool IsNestedSectionStart(OpcodeType type)
{
	switch (type)
	{
	case OpcodeType::Loop :
	case OpcodeType::If :
	case OpcodeType::Else :
	case OpcodeType::Switch :
		return true;
	default :
		return false;
	}
}

bool IsNestedSectionEnd(OpcodeType type)
{
	switch (type)
	{
		case OpcodeType::EndLoop:
		case OpcodeType::EndIf:
		case OpcodeType::Else:
		case OpcodeType::EndSwitch:
			return true;
		default:
			return false;
	}
}

ostream& SlimShader::operator<<(ostream& out, const ShaderProgramChunk& value)
{
	out << ToString(value._version.GetProgramType()) << "_"
		<< (uint32_t) value._version.GetMajorVersion() << "_"
		<< (uint32_t) value._version.GetMinorVersion()
		<< endl;

	int indent = 0;
	for (shared_ptr<OpcodeToken> token : value._tokens)
	{
		if (IsNestedSectionEnd(token->GetHeader().OpcodeType))
			indent -= 2;
		out << string(indent, ' ') << *token << endl;
		if (IsNestedSectionStart(token->GetHeader().OpcodeType))
			indent += 2;
	}

	return out;
}

ShaderProgramChunk::ShaderProgramChunk(ShaderVersion version)
	: _version(version)
{

}