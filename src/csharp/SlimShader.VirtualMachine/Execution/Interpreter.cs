using System;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Shex.Tokens;

namespace SlimShader.VirtualMachine.Execution
{
	public class Interpreter
	{
		private readonly ExecutionContext[] _executionContexts;
		private readonly InstructionToken[] _instructions;

		public Interpreter(ExecutionContext[] executionContexts, InstructionToken[] instructions)
		{
			_executionContexts = executionContexts;
			_instructions = instructions;
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
			for (int programCounter = 0; programCounter < _instructions.Length; programCounter++)
			{
				InstructionToken token = _instructions[programCounter];
				switch (token.Header.OpcodeType)
				{
					case OpcodeType.Add:
						Execute(t => t.ExecuteAdd(token));
						break;
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
					case OpcodeType.EndSwitch:
						break;
					case OpcodeType.ILt:
						Execute(t => t.ExecuteILt(token));
						break;
					case OpcodeType.EndLoop:
						programCounter += token.LinkedInstructionOffset;
						break;
					case OpcodeType.ItoF:
						Execute(t => t.ExecuteItoF(token));
						break;
					case OpcodeType.FtoI:
						Execute(t => t.ExecuteFtoI(token));
						break;
					case OpcodeType.FtoU:
						Execute(t => t.ExecuteFtoU(token));
						break;
					case OpcodeType.IAdd:
						Execute(t => t.ExecuteIAdd(token));
						break;
					case OpcodeType.IGe:
						Execute(t => t.ExecuteIGe(token));
						break;
					case OpcodeType.Loop:
						break;
					case OpcodeType.Lt:
						Execute(t => t.ExecuteLt(token));
						break;
					case OpcodeType.Mad:
						Execute(t => t.ExecuteMad(token));
						break;
					case OpcodeType.Max:
						Execute(t => t.ExecuteMax(token));
						break;
					case OpcodeType.Mov:
						Execute(t => t.ExecuteMov(token));
						break;
					case OpcodeType.Mul:
						Execute(t => t.ExecuteMul(token));
						break;
					case OpcodeType.Ret:
						break;
					case OpcodeType.Rsq:
						Execute(t => t.ExecuteRsq(token));
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

		private void Execute(Action<ExecutionContext> callback)
		{
			foreach (var thread in _executionContexts)
				callback(thread);
		}
	}
}