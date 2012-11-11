using System;
using System.Collections.Generic;
using System.Linq;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Shex.Tokens;
using SlimShader.VirtualMachine.Resources;

namespace SlimShader.VirtualMachine
{
	public class VirtualMachine
	{
		private readonly DxbcContainer _bytecode;
		private readonly DeclarationToken[] _declarations;
		private readonly InstructionToken[] _instructions;

		private readonly GlobalMemory _globalMemory;

		private readonly VirtualMachineThread[] _threads;
		private readonly UnorderedAccessView[] _unorderedAccessViews;

		private readonly Stack<int> _divergenceStack;

		private int _programCounter;

		public GlobalMemory GlobalMemory
		{
			get { return _globalMemory; }
		}

		public VirtualMachine(DxbcContainer bytecode, int numContexts)
		{
			_bytecode = bytecode;

			_declarations = bytecode.Shader.Tokens.OfType<DeclarationToken>().ToArray();
			_instructions = bytecode.Shader.Tokens.OfType<InstructionToken>().ToArray();

			_globalMemory = new GlobalMemory(bytecode.Shader.RegisterCounts, numContexts,
				bytecode.InputSignature.Parameters.Count,
				bytecode.OutputSignature.Parameters.Count);

			_threads = new VirtualMachineThread[numContexts];

			_unorderedAccessViews = new UnorderedAccessView[64];

			for (int i = 0; i < _threads.Length; i++)
				_threads[i] = new VirtualMachineThread(i, _globalMemory, bytecode.Shader.RegisterCounts);
		}

		public void SetUnorderedAccessViews(int startSlot, UnorderedAccessView[] unorderedAccessViews)
		{
			for (int i = 0; i < unorderedAccessViews.Length; i++)
				_unorderedAccessViews[startSlot + i] = unorderedAccessViews[i];
		}

		/// <summary>
		/// http://http.developer.nvidia.com/GPUGems2/gpugems2_chapter34.html
		/// http://people.maths.ox.ac.uk/gilesm/pp10/lec2_2x2.pdf
		/// http://stackoverflow.com/questions/10119796/how-does-cuda-compiler-know-the-divergence-behaviour-of-warps
		/// http://www.istc-cc.cmu.edu/publications/papers/2011/SIMD.pdf
		/// http://hal.archives-ouvertes.fr/docs/00/62/26/54/PDF/collange_sympa2011_en.pdf
		/// </summary>
		public void Execute()
		{
			for (_programCounter = 0; _programCounter < _instructions.Length; _programCounter++)
			{
				InstructionToken token = _instructions[_programCounter];
				switch (token.Header.OpcodeType)
				{
					case OpcodeType.Add:
						Execute(t => t.ExecuteAdd(token));
						break;
					case OpcodeType.BreakC:
					{
						// If all threads have "breaked", we can continue executing after the loop / switch.
						bool allBroke = false;
						Execute(t => allBroke = allBroke && t.ExecuteBreakC(token));
						if (allBroke)
							_programCounter += token.LinkedInstructionOffset;
						break;
					}
					case OpcodeType.Div:
						Execute(t => t.ExecuteDiv(token));
						break;
					case OpcodeType.Dp2:
						Execute(t => t.ExecuteDp2(token));
						break;
					case OpcodeType.Dp3:
						Execute(t => t.ExecuteDp3(token));
						break;
					case OpcodeType.Dp4:
						Execute(t => t.ExecuteDp4(token));
						break;
					case OpcodeType.EndSwitch :
						break;
					case OpcodeType.ILt:
						Execute(t => t.ExecuteILt(token));
						break;
					case OpcodeType.EndLoop :
						_programCounter += token.LinkedInstructionOffset;
						break;
					case OpcodeType.If:
					{
						// If all threads have followed the same branch, we can continue executing after the loop / switch.
						bool allBroke = false;
						Execute(t => allBroke = allBroke && t.ExecuteBreakC(token));
						if (allBroke)
							_programCounter += token.LinkedInstructionOffset;
						break;
					}
					case OpcodeType.ItoF:
						Execute(t => t.ExecuteItoF(token));
						break;
					case OpcodeType.FtoI:
						Execute(t => t.ExecuteFtoI(token));
						break;
					case OpcodeType.FtoU :
						Execute(t => t.ExecuteFtoU(token));
						break;
					case OpcodeType.Loop:
						break;
					case OpcodeType.Lt:
						Execute(t => t.ExecuteLt(token));
						break;
					case OpcodeType.Mov:
						Execute(t => t.ExecuteMov(token));
						break;
					case OpcodeType.Mul:
						Execute(t => t.ExecuteMul(token));
						break;
					case OpcodeType.Ret :
						break;
					case OpcodeType.Sqrt:
						Execute(t => t.ExecuteSqrt(token));
						break;
					case OpcodeType.Utof:
						Execute(t => t.ExecuteUtoF(token));
						break;
					case OpcodeType.Xor:
						Execute(t => t.ExecuteXor(token));
						break;
					default:
						throw new InvalidOperationException(token.Header.OpcodeType + " is not yet supported.");
				}
			}
		}

		private void Execute(Action<VirtualMachineThread> callback)
		{
			foreach (var thread in _threads)
			{
				if (thread.PerThreadProgramCounter == _programCounter)
					callback(thread);
			}
		}
	}
}