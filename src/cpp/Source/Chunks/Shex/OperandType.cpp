#include "PCH.h"
#include "OperandType.h"

using namespace std;
using namespace SlimShader;

string SlimShader::ToString(OperandType value)
{
	switch (value)
	{
	case OperandType::Temp :
		return "r";
	case OperandType::Input :
		return "v";
	case OperandType::Output :
		return "o";
	case OperandType::IndexableTemp :
		return "x";
	case OperandType::Sampler :
		return "s";
	case OperandType::Resource :
		return "t";
	case OperandType::ConstantBuffer :
		return "cb";
	case OperandType::ImmediateConstantBuffer :
		return "icb";
	case OperandType::Null :
		return "null";
	case OperandType::FunctionBody :
		return "fb";
	case OperandType::InputForkInstanceID :
		return "vForkInstanceID";
	case OperandType::InputControlPoint :
		return "vicp";
	case OperandType::InputDomainPoint :
		return "vDomain";
	case OperandType::ThisPointer :
		return "this";
	case OperandType::UnorderedAccessView :
		return "u";
	case OperandType::ThreadGroupSharedMemory :
		return "g";
	case OperandType::InputThreadID :
		return "vThreadID";
	case OperandType::InputThreadGroupID :
		return "vThreadGroupID";
	case OperandType::InputThreadIDInGroup :
		return "vThreadIDInGroup";
	case OperandType::InputThreadIDInGroupFlattened :
		return "vThreadIDInGroupFlattened";
	case OperandType::OutputDepthGreaterEqual :
		return "oDepthGE";
	case OperandType::OutputDepthLessEqual :
		return "oDepthLE";
	default :
		throw runtime_error("Unsupported value: " + to_string((int) value));
	}
}

bool SlimShader::RequiresRegisterNumberFor1DIndex(OperandType type)
{
	switch (type)
	{
	case OperandType::ImmediateConstantBuffer:
	case OperandType::ThisPointer:
		return false;
	default:
		return true;
	}
}

bool SlimShader::RequiresRegisterNumberFor2DIndex(OperandType type)
{
	switch (type)
	{
	case OperandType::InputControlPoint:
	case OperandType::Input:
		return false;
	default:
		return true;
	}
}