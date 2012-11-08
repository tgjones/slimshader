#pragma once

#include "PCH.h"
#include "DxbcContainer.h"
#include "InstructionToken.h"
#include "Number.h"

namespace SlimShader
{
	class VirtualMachineThread
	{
	public :
		void Execute(const InstructionToken& token, std::function<Number&(const Number&)> callback);
		void Execute(const InstructionToken& token, std::function<Number&(const Number&, const Number&)> callback);

		void SetInputRegisterCount(size_t count);
		void SetIndexableTempRegisterCount(uint32_t index, size_t count);
		void SetOutputRegisterCount(size_t count);
		void SetTempRegisterCount(size_t count);

		/// <summary>
		/// Gets value for use on LHS of an operation.
		/// </summary>
		Number4& GetRegisterLhs(const Operand& operand);

		/// <summary>
		/// Gets potentially-swizzled value for use on RHS of an operation.
		/// </summary>
		const Number4 GetRegisterRhs(const Operand& operand);

		void ExecuteMov(const InstructionToken& token);
		void ExecuteMul(const InstructionToken& token);

	private :

		std::vector<Number4> _tempRegisters;
		std::vector<std::vector<Number4>> _indexableTempRegisters;
		std::vector<Number4> _inputRegisters;
		std::vector<Number4> _resourceRegisters;
		std::vector<Number4> _samplerRegisters;
		std::vector<Number4> _outputRegisters;
	};

	class VirtualMachine
	{
	public :
		VirtualMachine(DxbcContainer container, size_t numThreads);

		const std::vector<VirtualMachineThread>& GetThreads() const;
		void Execute();

	private:
		void Execute(const std::function<void(VirtualMachineThread&)>& callback);

		DxbcContainer _container;
		std::vector<VirtualMachineThread> _threads;
		std::vector<std::shared_ptr<InstructionToken>> _instructionTokens;
	};
};