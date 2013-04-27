using System;
using System.Collections;
using System.Collections.Generic;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Shex.Tokens;
using SlimShader.VirtualMachine.Analysis.ExecutableInstructions;
using SlimShader.VirtualMachine.Registers;
using SlimShader.VirtualMachine.Resources;
using SlimShader.VirtualMachine.Util;

namespace SlimShader.VirtualMachine.Execution
{
	public class Interpreter : IShaderExecutor
	{
		private readonly ExecutionContext[] _executionContexts;
		private readonly ExecutableInstruction[] _instructions;
		private readonly BitArray _allOne;

		public Interpreter(ExecutionContext[] executionContexts, ExecutableInstruction[] instructions)
		{
			_executionContexts = executionContexts;
			_instructions = instructions;
			_allOne = BitArrayUtility.CreateAllOne(executionContexts.Length);
		}

		/// <summary>
		/// http://http.developer.nvidia.com/GPUGems2/gpugems2_chapter34.html
		/// http://people.maths.ox.ac.uk/gilesm/pp10/lec2_2x2.pdf
		/// http://stackoverflow.com/questions/10119796/how-does-cuda-compiler-know-the-divergence-behaviour-of-warps
		/// http://www.istc-cc.cmu.edu/publications/papers/2011/SIMD.pdf
		/// http://hal.archives-ouvertes.fr/docs/00/62/26/54/PDF/collange_sympa2011_en.pdf
		/// </summary>
		public IEnumerable<ExecutionResponse> Execute()
		{
			var divergenceStack = new DivergenceStack(_executionContexts.Length);
			divergenceStack.Push(0, _allOne, -1);

			while (divergenceStack.Peek().NextPC < _instructions.Length)
			{
				var topOfDivergenceStack = divergenceStack.Peek();
				int pc = topOfDivergenceStack.NextPC;
				var instruction = _instructions[pc];

				var activeMasks = new List<BitArray>();

				switch (instruction.OpcodeType)
 				{
					case ExecutableOpcodeType.Add:
						Execute(topOfDivergenceStack, t => Execute(t, instruction, (src0, src1) => Number.FromFloat(src0.Float + src1.Float, instruction.Saturate)));
						break;
					case ExecutableOpcodeType.Branch:
 						break;
					case ExecutableOpcodeType.BranchC :
						activeMasks.Add(new BitArray(_executionContexts.Length));
						activeMasks.Add(new BitArray(_executionContexts.Length));
						Execute(topOfDivergenceStack, t =>
						{
							var src0 = GetOperandValue(t, instruction.Operands[0]);
							bool result = TestCondition(ref src0, instruction.TestBoolean);
							activeMasks[0][t.Index] = result;
							activeMasks[1][t.Index] = !result;
						});
						break;
					case ExecutableOpcodeType.Cut :
					case ExecutableOpcodeType.CutStream:
						yield return ExecutionResponse.Cut;
						break;
					case ExecutableOpcodeType.Div:
						Execute(topOfDivergenceStack, t => Execute(t, instruction, (src0, src1) => Number.FromFloat(src0.Float / src1.Float, instruction.Saturate)));
						break;
					case ExecutableOpcodeType.Dp2:
						Execute(topOfDivergenceStack, t => Execute(t, instruction, (src0, src1) => Number.FromFloat(
							src0.Number0.Float * src1.Number0.Float + src0.Number1.Float * src1.Number1.Float,
							instruction.Saturate)));
						break;
					case ExecutableOpcodeType.Dp3:
						Execute(topOfDivergenceStack, t => Execute(t, instruction, (src0, src1) => Number.FromFloat(
							src0.Number0.Float * src1.Number0.Float
								+ src0.Number1.Float * src1.Number1.Float
								+ src0.Number2.Float * src1.Number2.Float,
							instruction.Saturate)));
						break;
					case ExecutableOpcodeType.Dp4:
						Execute(topOfDivergenceStack, t => Execute(t, instruction, (src0, src1) => Number.FromFloat(
							src0.Number0.Float * src1.Number0.Float
								+ src0.Number1.Float * src1.Number1.Float
								+ src0.Number2.Float * src1.Number2.Float
								+ src0.Number3.Float * src1.Number3.Float,
							instruction.Saturate)));
						break;
					case ExecutableOpcodeType.Emit:
					case ExecutableOpcodeType.EmitStream :
						yield return ExecutionResponse.Emit;
						break;
					case ExecutableOpcodeType.ILt:
						Execute(topOfDivergenceStack, t => Execute(t, instruction, (src0, src1) => Number.FromUInt((src0.Int < src1.Int) ? 0xFFFFFFFF : 0x0000000)));
						break;
					case ExecutableOpcodeType.ItoF:
						Execute(topOfDivergenceStack, t => Execute(t, instruction, src => Number.FromFloat(Convert.ToSingle(src.Int), instruction.Saturate)));
						break;
					case ExecutableOpcodeType.FtoI:
						Execute(topOfDivergenceStack, t => Execute(t, instruction, src => Number.FromInt(Convert.ToInt32(src.Float))));
						break;
					case ExecutableOpcodeType.FtoU:
						Execute(topOfDivergenceStack, t => Execute(t, instruction, src => Number.FromUInt(Convert.ToUInt32(src.Float))));
						break;
					case ExecutableOpcodeType.IAdd:
						Execute(topOfDivergenceStack, t => Execute(t, instruction, (src0, src1) => Number.FromInt(src0.Int + src1.Int)));
						break;
					case ExecutableOpcodeType.IGe:
						Execute(topOfDivergenceStack, t => Execute(t, instruction, (src0, src1) => Number.FromUInt((src0.Int >= src1.Int) ? 0xFFFFFFFF : 0x0000000)));
						break;
					case ExecutableOpcodeType.Lt:
						Execute(topOfDivergenceStack, t => Execute(t, instruction, (src0, src1) => Number.FromUInt((src0.Float < src1.Float) ? 0xFFFFFFFF : 0x0000000)));
						break;
					case ExecutableOpcodeType.Mad:
						Execute(topOfDivergenceStack, t => Execute(t, instruction, (src0, src1, src2) => Number.FromFloat((src0.Float * src1.Float) + src2.Float, instruction.Saturate)));
						break;
					case ExecutableOpcodeType.Max:
						Execute(topOfDivergenceStack, t => Execute(t, instruction, (src0, src1) => Number.FromFloat(Math.Max(src0.Float, src1.Float), instruction.Saturate)));
						break;
					case ExecutableOpcodeType.Mov:
						Execute(topOfDivergenceStack, t => Execute(t, instruction, src => src));
						break;
					case ExecutableOpcodeType.MovC:
						Execute(topOfDivergenceStack, t =>
						{
							// If src0, then dest = src1 else dest = src2
							var src0 = GetOperandValue(t, instruction.Operands[1]);
							bool result = TestCondition(ref src0, instruction.TestBoolean);
							SetRegisterValue(t, instruction.Operands[0], result
								? GetOperandValue(t, instruction.Operands[2])
								: GetOperandValue(t, instruction.Operands[3]));
						});
						break;
					case ExecutableOpcodeType.Mul:
						Execute(topOfDivergenceStack, t => Execute(t, instruction, (src0, src1) => Number.FromFloat(src0.Float * src1.Float, instruction.Saturate)));
						break;
					case ExecutableOpcodeType.Ret:
						yield return ExecutionResponse.Finished;
 						break;
					case ExecutableOpcodeType.Rsq:
						Execute(topOfDivergenceStack, t => Execute(t, instruction, src => Number.FromFloat((float) (1.0f / Math.Sqrt(src.Float)), instruction.Saturate)));
						break;
					case ExecutableOpcodeType.Sample:
						Execute(topOfDivergenceStack, t =>
						{
							var srcAddress = GetOperandValue(t, instruction.Operands[1]);
							var srcResource = GetTexture(t, instruction.Operands[2]);
							var srcSampler = GetSampler(t, instruction.Operands[3]);

							//var result = srcResource.Sample(srcSampler, srcAddress);
							var result = new Number4();

							SetRegisterValue(t, instruction.Operands[0], result);
						});
						break;
					case ExecutableOpcodeType.Sqrt:
						Execute(topOfDivergenceStack, t => Execute(t, instruction, src => Number.FromFloat((float) Math.Sqrt(src.Float), instruction.Saturate)));
						break;
					case ExecutableOpcodeType.Utof:
						Execute(topOfDivergenceStack, t => Execute(t, instruction, src => Number.FromFloat(Convert.ToSingle(src.UInt), instruction.Saturate)));
						break;
					case ExecutableOpcodeType.Xor:
						Execute(topOfDivergenceStack, t => Execute(t, instruction, (src0, src1) => Number.FromUInt(src0.UInt | src1.UInt)));
						break;
					default:
						throw new InvalidOperationException(instruction.OpcodeType + " is not yet supported.");
				}

				// Algorithm from "Dynamic Warp Formation: Exploiting Thread Scheduling for Efficient MIMD Control Flow
				// on SIMD Graphics Hardware" by Wilson Wai Lun Fung -
				// https://circle.ubc.ca/bitstream/handle/2429/2268/ubc_2008_fall_fung_wilson_wai_lun.pdf?sequence=1
				// 
				// 3 possible cases:
				// - No Divergence (single next PC)
				//     => Update the next PC ﬁeld of the top of stack (TOS) entry to
				//        the next PC of all active threads in this warp.
				// - Divergence (multiple next PC)
				//     => Modify the next PC ﬁeld of the TOS entry to the reconvergence point. 
				//        For each unique next PC of the warp, push a
				//        new entry onto the stack with next PC ﬁeld being the unique
				//        next PC and the reconv. PC being the reconvergence point.
				//        The active mask of each entry denotes the threads branching
				//        to the next PC value of this entry.
				// - Reconvergence (next PC = reconv. PC of TOS)
				//     => Pop TOS entry from the stack.
				instruction.UpdateDivergenceStack(divergenceStack, activeMasks);
			}
		}

		private void Execute(DivergenceStackEntry divergenceStackEntry, Action<ExecutionContext> callback)
		{
			foreach (var thread in _executionContexts)
				if (divergenceStackEntry.ActiveMask[thread.Index])
					callback(thread);
		}

		private static bool TestCondition(ref Number4 number, InstructionTestBoolean testBoolean)
		{
			switch (testBoolean)
			{
				case InstructionTestBoolean.Zero:
					return number.AllZero;
				case InstructionTestBoolean.NonZero:
					return number.AnyNonZero;
				default:
					throw new ArgumentOutOfRangeException("testBoolean");
			}
		}

		private void GetRegister(ExecutionContext context, Operand operand, out Number4[] register, out int index)
		{
			var registerIndex = GetRegisterIndex(context, operand);
			context.GetRegister(operand.OperandType, registerIndex, out register, out index);
		}

		/// <summary>
		/// Gets potentially-swizzled value for use on RHS of an operation.
		/// </summary>
		private Number4 GetOperandValue(ExecutionContext context, Operand operand)
		{
			switch (operand.OperandType)
			{
				case OperandType.Immediate32:
				case OperandType.Immediate64:
					return OperandUtility.ApplyOperandModifier(operand.ImmediateValues, operand.Modifier);
				case OperandType.ConstantBuffer:
				case OperandType.IndexableTemp:
				case OperandType.Input:
				case OperandType.Temp:
					Number4[] register;
					int index;
					GetRegister(context, operand, out register, out index);
					var swizzledNumber = OperandUtility.ApplyOperandSelectionMode(register[index], operand);
					return OperandUtility.ApplyOperandModifier(swizzledNumber, operand.Modifier);
				default:
					throw new ArgumentException("Unsupported operand type: " + operand.OperandType);
			}
		}

		private ITexture GetTexture(ExecutionContext context, Operand operand)
		{
			switch (operand.OperandType)
			{
				case OperandType.Resource:
					return context.Textures[GetRegisterIndex(context, operand).Index1D];
				default:
					throw new ArgumentException("Unsupported operand type: " + operand.OperandType);
			}
		}

		private ISampler GetSampler(ExecutionContext context, Operand operand)
		{
			switch (operand.OperandType)
			{
				case OperandType.Sampler:
					return context.Samplers[GetRegisterIndex(context, operand).Index1D];
				default:
					throw new ArgumentException("Unsupported operand type: " + operand.OperandType);
			}
		}

		private RegisterIndex GetRegisterIndex(ExecutionContext context, Operand operand)
		{
			var result = new RegisterIndex();
			switch (operand.IndexDimension)
			{
				case OperandIndexDimension._1D:
					result.Index1D = EvaluateOperandIndex(context, operand.Indices[0]);
					break;
				case OperandIndexDimension._2D:
					result.Index2D_0 = EvaluateOperandIndex(context, operand.Indices[0]);
					result.Index2D_1 = EvaluateOperandIndex(context, operand.Indices[1]);
					break;
			}
			return result;
		}

		private ushort EvaluateOperandIndex(ExecutionContext context, OperandIndex index)
		{
			var result = (ushort)index.Value;
			switch (index.Representation)
			{
				case OperandIndexRepresentation.Immediate32PlusRelative:
				case OperandIndexRepresentation.Immediate64PlusRelative:
				case OperandIndexRepresentation.Relative:
					var operandValue = GetOperandValue(context, index.Register);
					result += (ushort)operandValue.GetMaskedNumber(index.Register.ComponentMask).UInt;
					break;
			}
			return result;
		}

		private void SetRegisterValue(ExecutionContext context, Operand operand, Number4 value)
		{
			Number4[] register;
			int index;
			GetRegister(context, operand, out register, out index);
			register[index].WriteMaskedValue(value, operand.ComponentMask);
		}

		private void Execute(ExecutionContext context, ExecutableInstruction instruction, Func<Number, Number> callback)
		{
			var src = GetOperandValue(context, instruction.Operands[1]);

			SetRegisterValue(context, instruction.Operands[0], new Number4
			{
				Number0 = callback(src.Number0),
				Number1 = callback(src.Number1),
				Number2 = callback(src.Number2),
				Number3 = callback(src.Number3)
			});
		}

		private void Execute(ExecutionContext context, ExecutableInstruction instruction, Func<Number, Number, Number> callback)
		{
			var src0 = GetOperandValue(context, instruction.Operands[1]);
			var src1 = GetOperandValue(context, instruction.Operands[2]);

			SetRegisterValue(context, instruction.Operands[0], new Number4
			{
				Number0 = callback(src0.Number0, src1.Number0),
				Number1 = callback(src0.Number1, src1.Number1),
				Number2 = callback(src0.Number2, src1.Number2),
				Number3 = callback(src0.Number3, src1.Number3)
			});
		}

		private void Execute(ExecutionContext context, ExecutableInstruction instruction, Func<Number, Number, Number, Number> callback)
		{
			var src0 = GetOperandValue(context, instruction.Operands[1]);
			var src1 = GetOperandValue(context, instruction.Operands[2]);
			var src2 = GetOperandValue(context, instruction.Operands[3]);

			SetRegisterValue(context, instruction.Operands[0], new Number4
			{
				Number0 = callback(src0.Number0, src1.Number0, src2.Number0),
				Number1 = callback(src0.Number1, src1.Number1, src2.Number1),
				Number2 = callback(src0.Number2, src1.Number2, src2.Number2),
				Number3 = callback(src0.Number3, src1.Number3, src2.Number3)
			});
		}

		private void Execute(ExecutionContext context, ExecutableInstruction instruction, Func<Number4, Number4, Number> callback)
		{
			var src0 = GetOperandValue(context, instruction.Operands[1]);
			var src1 = GetOperandValue(context, instruction.Operands[2]);
			var result = callback(src0, src1);

			SetRegisterValue(context, instruction.Operands[0], new Number4
			{
				Number0 = result,
				Number1 = result,
				Number2 = result,
				Number3 = result
			});
		}
	}
}