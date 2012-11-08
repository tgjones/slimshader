#include "PCH.h"
#include "VirtualMachine.h"

#include "Decoder.h"
#include "IndexableTempRegisterDeclarationToken.h"
#include "InputRegisterDeclarationToken.h"
#include "InputSignatureChunk.h"
#include "OutputSignatureChunk.h"
#include "ShaderProgramChunk.h"
#include "TempRegisterDeclarationToken.h"

using namespace std;
using namespace SlimShader;

VirtualMachine::VirtualMachine(DxbcContainer container, size_t numThreads)
	: _container(container), _threads(numThreads)
{
	// Prepare threads with correct number of registers.
	for (auto thread : _threads)
	{
		thread.SetInputRegisterCount(container.GetInputSignature()->GetParameters().size());
		thread.SetOutputRegisterCount(container.GetOutputSignature()->GetParameters().size());
	}

	for (auto token : container.GetShader()->GetTokens())
	{
		auto opcodeType = token->GetHeader().OpcodeType;
		if (IsDeclaration(opcodeType))
		{
			switch (opcodeType)
			{
			case OpcodeType::DclIndexableTemp :
				{
					auto typedToken = static_pointer_cast<IndexableTempRegisterDeclarationToken>(token);
					for (auto thread : _threads)
					{
						thread.SetIndexableTempRegisterCount(typedToken->GetRegisterIndex(), typedToken->GetRegisterCount());
					}
				}
				break;
			case OpcodeType::DclTemps :
				for (auto thread : _threads)
				{
					thread.SetTempRegisterCount(static_pointer_cast<TempRegisterDeclarationToken>(token)->GetTempCount());
				}
				break;
			}
		}
		else
		{
			_instructionTokens.push_back(static_pointer_cast<InstructionToken>(token));
		}
	}
}

void VirtualMachine::Execute()
{
	// LHS only has masks.
	// RHS only has swizzles.
	for (auto token : _instructionTokens)
	{
		switch (token->GetHeader().OpcodeType)
		{
		case OpcodeType::Mov :
			Execute([token](VirtualMachineThread& thread) { thread.ExecuteMov(*token); });
			break;
		case OpcodeType::Mul :
			Execute([token](VirtualMachineThread& thread) { thread.ExecuteMul(*token); });
			break;
		default :
			throw runtime_error("Unsupported opcode type: " + ToString(token->GetHeader().OpcodeType));
		}
	}
}

// TODO: Support branching. Implement a flag attached to each thread which allows non-executing threads to be masked off.

void VirtualMachineThread::Execute(const InstructionToken& token,
								   std::function<Number&(const Number&)> callback)
{
	auto destOperand = token.GetOperand(0);
	auto dest = GetRegisterLhs(destOperand);
	auto src0 = GetRegisterRhs(token.GetOperand(1));
	for (auto i = 0; i < 4; i++)
	{
		if (HasFlag(static_cast<ComponentMask>(i), destOperand.GetComponentMask()))
			dest.Numbers[i] = callback(src0.Numbers[i]);
	}
}

void VirtualMachineThread::Execute(const InstructionToken& token,
								   std::function<Number&(const Number&, const Number&)> callback)
{
	auto destOperand = token.GetOperand(0);
	auto dest = GetRegisterLhs(destOperand);
	auto src0 = GetRegisterRhs(token.GetOperand(1));
	auto src1 = GetRegisterRhs(token.GetOperand(2));
	for (auto i = 0; i < 4; i++)
	{
		if (HasFlag(static_cast<ComponentMask>(i), destOperand.GetComponentMask()))
			dest.Numbers[i] = callback(src0.Numbers[i], src1.Numbers[i]);
	}
}

void VirtualMachineThread::ExecuteMov(const InstructionToken& token)
{
	Execute(token, [](const Number& src0) { return src0; });
}

void VirtualMachineThread::ExecuteMul(const InstructionToken& token)
{
	Execute(token, [](const Number& src0, const Number& src1) { return src0.AsFloat() * src1.AsFloat(); });
}

Number4& VirtualMachineThread::GetRegisterLhs(const Operand& operand)
{
	// TODO: Indices might be relative
	auto* indices = operand.GetIndices();
	switch (operand.GetOperandType())
	{
	case OperandType::IndexableTemp :
		return _indexableTempRegisters[indices[0].Value][indices[1].Value];
	case OperandType::Input :
		return _inputRegisters[indices[0].Value];
	case OperandType::Temp :
		return _tempRegisters[indices[0].Value];
	default :
		throw runtime_error("Unsupported operand type");
	}
}

const Number4 VirtualMachineThread::GetRegisterRhs(const Operand& operand)
{
	auto deswizzle = [operand](const Number4& n) -> Number4
	{
		if (operand.GetSelectionMode() != Operand4ComponentSelectionMode::Swizzle)
			return n;
		Number4 result;
		result.Numbers[0] = n.Numbers[static_cast<uint32_t>(operand.GetSwizzles()[0])];
		result.Numbers[1] = n.Numbers[static_cast<uint32_t>(operand.GetSwizzles()[1])];
		result.Numbers[2] = n.Numbers[static_cast<uint32_t>(operand.GetSwizzles()[2])];
		result.Numbers[3] = n.Numbers[static_cast<uint32_t>(operand.GetSwizzles()[3])];
		return result;
	};

	switch (operand.GetOperandType())
	{
	case OperandType::Immediate32 :
	case OperandType::Immediate64 :
		return operand.GetImmediateValues();
	case OperandType::IndexableTemp :
	case OperandType::Input :
	case OperandType::Temp :
		return deswizzle(GetRegisterLhs(operand));
	default :
		throw runtime_error("Unsupported operand type");
	}
}

void VirtualMachine::Execute(const function<void(VirtualMachineThread&)>& callback)
{
	for (auto& thread : _threads)
		callback(thread);
}