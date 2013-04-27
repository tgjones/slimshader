using System.Collections.Generic;
using System.Linq;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Shex.Tokens;
using SlimShader.VirtualMachine.Analysis;
using SlimShader.VirtualMachine.Analysis.ControlFlow;
using SlimShader.VirtualMachine.Analysis.ExecutableInstructions;
using SlimShader.VirtualMachine.Analysis.ExplicitBranching;
using SlimShader.VirtualMachine.Execution;
using SlimShader.VirtualMachine.Registers;
using SlimShader.VirtualMachine.Resources;

namespace SlimShader.VirtualMachine
{
	public class VirtualMachine
	{
		private readonly BytecodeContainer _bytecode;

		private readonly ExecutionContext[] _executionContexts;
		private readonly IShaderExecutor _shaderExecutor;

		private readonly RequiredRegisters _requiredRegisters;

		public int NumPrimitives
		{
			get { return _requiredRegisters.NumPrimitives; }
		}

		public VirtualMachine(BytecodeContainer bytecode, int numContexts)
		{
			_bytecode = bytecode;

			var instructionTokens = bytecode.Shader.Tokens.OfType<InstructionToken>().ToArray();
			var branchingInstructions = ExplicitBranchingRewriter.Rewrite(instructionTokens);
			var controlFlowGraph = ControlFlowGraph.FromInstructions(branchingInstructions);
			var executableInstructions = ExecutableInstructionRewriter.Rewrite(controlFlowGraph);

			_requiredRegisters = RequiredRegisters.FromShader(bytecode.Shader);

			_executionContexts = new ExecutionContext[numContexts];
			for (int i = 0; i < _executionContexts.Length; i++)
				_executionContexts[i] = new ExecutionContext(i, _requiredRegisters);
			_shaderExecutor = new Interpreter(_executionContexts, executableInstructions.ToArray()); 
		}

		public IEnumerable<ExecutionResponse> ExecuteMultiple()
		{
			return _shaderExecutor.Execute();
		}

		public void Execute()
		{
			ExecuteMultiple().ToList();
		}

		public Number4 GetRegister(int contextIndex, OperandType registerType, RegisterIndex registerIndex)
		{
			Number4[] register;
			int index;
			_executionContexts[contextIndex].GetRegister(registerType, registerIndex, out register, out index);
			return register[index];
		}

		public void SetRegister(int contextIndex, OperandType registerType, RegisterIndex registerIndex, Number4 value)
		{
			Number4[] register;
			int index;
			_executionContexts[contextIndex].GetRegister(registerType, registerIndex, out register, out index);
			register[index] = value;
		}

		public void SetTexture(int contextIndex, RegisterIndex registerIndex, ITexture texture)
		{
			_executionContexts[contextIndex].Textures[registerIndex.Index1D] = texture;
		}

		public void SetSampler(int contextIndex, RegisterIndex registerIndex, ISampler sampler)
		{
			_executionContexts[contextIndex].Samplers[registerIndex.Index1D] = sampler;
		}
	}
}