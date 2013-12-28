using System;
using System.Collections.Generic;
using System.Linq;
using SlimShader.Chunks.Common;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Shex.Tokens;
using SlimShader.VirtualMachine.Analysis.ControlFlow;
using SlimShader.VirtualMachine.Analysis.ExecutableInstructions;
using SlimShader.VirtualMachine.Analysis.ExplicitBranching;
using SlimShader.VirtualMachine.Execution;
using SlimShader.VirtualMachine.Registers;
using SlimShader.VirtualMachine.Resources;

namespace SlimShader.VirtualMachine
{
    // For pixel shaders, the virtual machine expects pixels quads to be arranged as follows:
    // 0 = top left
    // 1 = top right
    // 2 = bottom left
    // 3 = bottom right
	public class VirtualMachine
	{
        public static IShaderExecutor ShaderExecutor { get; set; }

        static VirtualMachine()
        {
            ShaderExecutor = new Interpreter();
        }

		private readonly BytecodeContainer _bytecode;

		private readonly ExecutionContext[] _executionContexts;
	    private readonly ExecutableInstruction[] _executableInstructions;

		private readonly RequiredRegisters _requiredRegisters;

	    public BytecodeContainer Bytecode
	    {
            get { return _bytecode; }
	    }

        internal Number4[][] ConstantBuffers { get; private set; }
        internal TextureSampler[] TextureSamplers { get; private set; }
        internal ITexture[] Textures { get; private set; }
        internal SamplerState[] Samplers { get; private set; }

		public int NumPrimitives
		{
			get { return _requiredRegisters.NumPrimitives; }
		}

		public VirtualMachine(BytecodeContainer bytecode, int numContexts)
		{
            if (bytecode.Shader.Version.ProgramType == ProgramType.PixelShader && numContexts % 4 != 0)
                throw new ArgumentOutOfRangeException("numContexts", "numContexts must be a multiple of 4 for pixel shaders.");

			_bytecode = bytecode;

			var instructionTokens = bytecode.Shader.Tokens.OfType<InstructionToken>().ToArray();
			var branchingInstructions = ExplicitBranchingRewriter.Rewrite(instructionTokens);
			var controlFlowGraph = ControlFlowGraph.FromInstructions(branchingInstructions);
            _executableInstructions = ExecutableInstructionRewriter.Rewrite(controlFlowGraph).ToArray();

			_requiredRegisters = RequiredRegisters.FromShader(bytecode.Shader);

			_executionContexts = new ExecutionContext[numContexts];
			for (int i = 0; i < _executionContexts.Length; i++)
				_executionContexts[i] = new ExecutionContext(this, i, _requiredRegisters);

            ConstantBuffers = new Number4[_requiredRegisters.ConstantBuffers.Count][];
            for (int i = 0; i < _requiredRegisters.ConstantBuffers.Count; i++)
                ConstantBuffers[i] = new Number4[_requiredRegisters.ConstantBuffers[i]];

		    TextureSamplers = new TextureSampler[_requiredRegisters.Resources.Count];
			for (int i = 0; i < _requiredRegisters.Resources.Count; i++)
				TextureSamplers[i] = TextureSamplerFactory.Create(_requiredRegisters.Resources[i]);

            Textures = new ITexture[_requiredRegisters.Resources.Count];
            Samplers = new SamplerState[_requiredRegisters.Samplers];
		}

		public IEnumerable<ExecutionResponse> ExecuteMultiple()
		{
            return ShaderExecutor.Execute(this, _executionContexts, _executableInstructions);
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

        public Number4 GetOutputRegisterValue(int contextIndex, int registerIndex)
        {
            return _executionContexts[contextIndex].GetOutputRegisterValue(registerIndex);
        }

		public void SetRegister(int contextIndex, OperandType registerType, RegisterIndex registerIndex, ref Number4 value)
		{
			Number4[] register;
			int index;
			_executionContexts[contextIndex].GetRegister(registerType, registerIndex, out register, out index);
			if (index < register.Length)
				register[index] = value;
		}

        public void SetRegister(int contextIndex, OperandType registerType, RegisterIndex registerIndex, Number4 value)
        {
            SetRegister(contextIndex, registerType, registerIndex, ref value);
        }

        public void SetInputRegisterValue(int contextIndex, int index0, int index1, ref Number4 value)
        {
            if (index0 < _requiredRegisters.NumPrimitives && index1 < _requiredRegisters.Inputs)
                _executionContexts[contextIndex].SetInputRegisterValue(index0, index1, ref value);
        }

		internal void SetInputRegisterValue(int contextIndex, int index0, int index1, Number4 value)
		{
			SetInputRegisterValue(contextIndex, index0, index1, ref value);
		}

        public void SetConstantBufferRegisterValue(int index0, int index1, ref Number4 value)
        {
			var constantBuffer = ConstantBuffers[index0];
			if (index1 < constantBuffer.Length)
				constantBuffer[index1] = value;
        }

		internal void SetConstantBufferRegisterValue(int index0, int index1, Number4 value)
		{
			SetConstantBufferRegisterValue(index0, index1, ref value);
		}

		public void SetTexture(RegisterIndex registerIndex, ITexture texture)
		{
			Textures[registerIndex.Index1D] = texture;
		}

		public void SetSampler(RegisterIndex registerIndex, SamplerState sampler)
		{
			Samplers[registerIndex.Index1D] = sampler;
		}
	}
}