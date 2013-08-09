using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Shex.Tokens;
using SlimShader.VirtualMachine.Analysis.ExecutableInstructions;
using SlimShader.VirtualMachine.Registers;
using SlimShader.VirtualMachine.Util;

namespace SlimShader.VirtualMachine.Execution
{
    // NOTE: Gradient instructions can't be used in loops or branches, so all execution contexts must be active (right?).
    // AFAIK, this is enforced by the HLSL compiler. TODO: Check this.
	public class Interpreter : IShaderExecutor
	{
		/// <summary>
		/// http://http.developer.nvidia.com/GPUGems2/gpugems2_chapter34.html
		/// http://people.maths.ox.ac.uk/gilesm/pp10/lec2_2x2.pdf
		/// http://stackoverflow.com/questions/10119796/how-does-cuda-compiler-know-the-divergence-behaviour-of-warps
		/// http://www.istc-cc.cmu.edu/publications/papers/2011/SIMD.pdf
		/// http://hal.archives-ouvertes.fr/docs/00/62/26/54/PDF/collange_sympa2011_en.pdf
		/// </summary>
        public IEnumerable<ExecutionResponse> Execute(
            VirtualMachine virtualMachine, 
            ExecutionContext[] executionContexts, 
            ExecutableInstruction[] instructions)
		{
		    var warp = new Warp(executionContexts.Length);
		    var activeExecutionContexts = GetActiveExecutionContexts(executionContexts, warp.DivergenceStack.Peek());
			while (warp.DivergenceStack.Peek().NextPC < instructions.Length)
			{
				var topOfDivergenceStack = warp.DivergenceStack.Peek();
				int pc = topOfDivergenceStack.NextPC;
				var instruction = instructions[pc];

			    List<BitArray> activeMasks = null;

				switch (instruction.OpcodeType)
				{
				    case ExecutableOpcodeType.Add:
				        foreach (var thread in activeExecutionContexts)
                            Execute(thread, instruction, NumberType.Float, InstructionImplementations.Add);
				        break;
                    case ExecutableOpcodeType.And:
                        foreach (var thread in activeExecutionContexts)
                            Execute(thread, instruction, NumberType.UInt, InstructionImplementations.And);
                        break;
				    case ExecutableOpcodeType.Branch:
				        break;
				    case ExecutableOpcodeType.BranchC:
                        activeMasks = new List<BitArray>
                        {
                            new BitArray(executionContexts.Length),
                            new BitArray(executionContexts.Length)
                        };
				        foreach (var thread in activeExecutionContexts)
				        {
				            var src0 = GetOperandValue(thread, instruction.Operands[0], NumberType.UInt);
				            bool result = TestCondition(ref src0, instruction.TestBoolean);
				            activeMasks[0][thread.Index] = result;
				            activeMasks[1][thread.Index] = !result;
				        }
				        break;
				    case ExecutableOpcodeType.Cut:
				    case ExecutableOpcodeType.CutStream:
				        yield return ExecutionResponse.Cut;
				        break;
                    case ExecutableOpcodeType.Discard:
				        throw new NotImplementedException();
				    case ExecutableOpcodeType.Div:
				        foreach (var thread in activeExecutionContexts)
                            Execute(thread, instruction, NumberType.Float, InstructionImplementations.Div);
				        break;
				    case ExecutableOpcodeType.Dp2:
				        foreach (var thread in activeExecutionContexts)
                            ExecuteScalar(thread, instruction, NumberType.Float, InstructionImplementations.Dp2);
				        break;
				    case ExecutableOpcodeType.Dp3:
				        foreach (var thread in activeExecutionContexts)
                            ExecuteScalar(thread, instruction, NumberType.Float, InstructionImplementations.Dp3);
				        break;
				    case ExecutableOpcodeType.Dp4:
				        foreach (var thread in activeExecutionContexts)
                            ExecuteScalar(thread, instruction, NumberType.Float, InstructionImplementations.Dp4);
				        break;
                    case ExecutableOpcodeType.Emit:
                    case ExecutableOpcodeType.EmitStream:
                        yield return ExecutionResponse.Emit;
                        break;
                    case ExecutableOpcodeType.Eq:
                        foreach (var thread in activeExecutionContexts)
                            Execute(thread, instruction, NumberType.Float, InstructionImplementations.Eq);
                        break;
                    case ExecutableOpcodeType.Exp:
                        foreach (var thread in activeExecutionContexts)
                            Execute(thread, instruction, NumberType.Float, InstructionImplementations.Exp);
                        break;
                    case ExecutableOpcodeType.Frc:
                        foreach (var thread in activeExecutionContexts)
                            Execute(thread, instruction, NumberType.Float, InstructionImplementations.Frc);
                        break;
                    case ExecutableOpcodeType.FtoI:
                        foreach (var thread in activeExecutionContexts)
                            Execute(thread, instruction, NumberType.Float, InstructionImplementations.FtoI);
                        break;
                    case ExecutableOpcodeType.FtoU:
                        foreach (var thread in activeExecutionContexts)
                            Execute(thread, instruction, NumberType.Float, InstructionImplementations.FtoU);
                        break;
                    case ExecutableOpcodeType.Ge:
                        foreach (var thread in activeExecutionContexts)
                            Execute(thread, instruction, NumberType.Float, InstructionImplementations.Ge);
                        break;
                    case ExecutableOpcodeType.IAdd:
                        foreach (var thread in activeExecutionContexts)
                            Execute(thread, instruction, NumberType.Int, InstructionImplementations.IAdd);
                        break;
                    case ExecutableOpcodeType.IEq:
                        foreach (var thread in activeExecutionContexts)
                            Execute(thread, instruction, NumberType.Int, InstructionImplementations.IEq);
                        break;
                    case ExecutableOpcodeType.IGe:
                        foreach (var thread in activeExecutionContexts)
                            Execute(thread, instruction, NumberType.Int, InstructionImplementations.IGe);
                        break;
				    case ExecutableOpcodeType.ILt:
				        foreach (var thread in activeExecutionContexts)
                            Execute(thread, instruction, NumberType.Int, InstructionImplementations.ILt);
				        break;
                    case ExecutableOpcodeType.IMad:
                        foreach (var thread in activeExecutionContexts)
                            Execute(thread, instruction, NumberType.Int, InstructionImplementations.IMad);
                        break;
                    case ExecutableOpcodeType.IMin:
                        foreach (var thread in activeExecutionContexts)
                            Execute(thread, instruction, NumberType.Int, InstructionImplementations.IMin);
                        break;
                    case ExecutableOpcodeType.INe:
                        foreach (var thread in activeExecutionContexts)
                            Execute(thread, instruction, NumberType.Int, InstructionImplementations.INe);
                        break;
                    case ExecutableOpcodeType.INeg:
                        foreach (var thread in activeExecutionContexts)
                            Execute(thread, instruction, NumberType.Int, InstructionImplementations.INeg);
                        break;
                    case ExecutableOpcodeType.IShl:
                        foreach (var thread in activeExecutionContexts)
                            Execute(thread, instruction, NumberType.Int, InstructionImplementations.IShl);
                        break;
                    case ExecutableOpcodeType.IShr:
                        foreach (var thread in activeExecutionContexts)
                            Execute(thread, instruction, NumberType.Int, InstructionImplementations.IShr);
                        break;
                    case ExecutableOpcodeType.ItoF:
				        foreach (var thread in activeExecutionContexts)
				            Execute(thread, instruction, NumberType.Int, InstructionImplementations.ItoF);
				        break;
                    case ExecutableOpcodeType.Log:
                        foreach (var thread in activeExecutionContexts)
                            Execute(thread, instruction, NumberType.Float, InstructionImplementations.Log);
                        break;
				    case ExecutableOpcodeType.Lt:
				        foreach (var thread in activeExecutionContexts)
                            Execute(thread, instruction, NumberType.Float, InstructionImplementations.Lt);
				        break;
				    case ExecutableOpcodeType.Mad:
				        foreach (var thread in activeExecutionContexts)
				            Execute(thread, instruction, NumberType.Float, InstructionImplementations.Mad);
				        break;
				    case ExecutableOpcodeType.Max:
				        foreach (var thread in activeExecutionContexts)
                            Execute(thread, instruction, NumberType.Float, InstructionImplementations.Max);
				        break;
                    case ExecutableOpcodeType.Min:
                        foreach (var thread in activeExecutionContexts)
                            Execute(thread, instruction, NumberType.Float, InstructionImplementations.Min);
                        break;
				    case ExecutableOpcodeType.Mov:
				        foreach (var thread in activeExecutionContexts)
                            Execute(thread, instruction, NumberType.Float, InstructionImplementations.Mov);
				        break;
				    case ExecutableOpcodeType.MovC:
				        foreach (var thread in activeExecutionContexts)
				            Execute(thread, instruction, NumberType.Float, InstructionImplementations.MovC);
				        break;
				    case ExecutableOpcodeType.Mul:
				        foreach (var thread in activeExecutionContexts)
                            Execute(thread, instruction, NumberType.Float, InstructionImplementations.Mul);
				        break;
                    case ExecutableOpcodeType.Ne:
                        foreach (var thread in activeExecutionContexts)
                            Execute(thread, instruction, NumberType.Float, InstructionImplementations.Ne);
                        break;
				    case ExecutableOpcodeType.Ret:
				        yield return ExecutionResponse.Finished;
				        break;
                    case ExecutableOpcodeType.RoundNe:
                        foreach (var thread in activeExecutionContexts)
                            Execute(thread, instruction, NumberType.Float, InstructionImplementations.RoundNe);
                        break;
                    case ExecutableOpcodeType.RoundNi:
                        foreach (var thread in activeExecutionContexts)
                            Execute(thread, instruction, NumberType.Float, InstructionImplementations.RoundNi);
                        break;
                    case ExecutableOpcodeType.RoundPi:
                        foreach (var thread in activeExecutionContexts)
                            Execute(thread, instruction, NumberType.Float, InstructionImplementations.RoundPi);
                        break;
                    case ExecutableOpcodeType.RoundZ:
                        foreach (var thread in activeExecutionContexts)
                            Execute(thread, instruction, NumberType.Float, InstructionImplementations.RoundZ);
                        break;
				    case ExecutableOpcodeType.Rsq:
				        foreach (var thread in activeExecutionContexts)
                            Execute(thread, instruction, NumberType.Float, InstructionImplementations.Rsq);
				        break;
                    case ExecutableOpcodeType.DerivRtx:
				    case ExecutableOpcodeType.RtxCoarse:
				        for (var i = 0; i < executionContexts.Length; i += 4)
				        {
				            var topLeft = GetOperandValue(executionContexts[i + 0], instruction.Operands[1], NumberType.Float);
                            var topRight = GetOperandValue(executionContexts[i + 1], instruction.Operands[1], NumberType.Float);

				            var deltaX = Number4.Subtract(ref topRight, ref topLeft);

				            for (var j = i; j < i + 4; j++)
				                SetRegisterValue(executionContexts[j], instruction.Operands[0], deltaX);
				        }
				        break;
				    case ExecutableOpcodeType.RtxFine:
                        for (var i = 0; i < executionContexts.Length; i += 4)
                        {
                            var topLeft = GetOperandValue(executionContexts[i + 0], instruction.Operands[1], NumberType.Float);
                            var topRight = GetOperandValue(executionContexts[i + 1], instruction.Operands[1], NumberType.Float);
                            var bottomLeft = GetOperandValue(executionContexts[i + 2], instruction.Operands[1], NumberType.Float);
                            var bottomRight = GetOperandValue(executionContexts[i + 3], instruction.Operands[1], NumberType.Float);

                            var topDeltaX = Number4.Subtract(ref topRight, ref topLeft);
                            var bottomDeltaX = Number4.Subtract(ref bottomRight, ref bottomLeft);

                            SetRegisterValue(executionContexts[i + 0], instruction.Operands[0], topDeltaX);
                            SetRegisterValue(executionContexts[i + 1], instruction.Operands[0], topDeltaX);

                            SetRegisterValue(executionContexts[i + 2], instruction.Operands[0], bottomDeltaX);
                            SetRegisterValue(executionContexts[i + 3], instruction.Operands[0], bottomDeltaX);
                        }
				        break;
                    case ExecutableOpcodeType.DerivRty:
                    case ExecutableOpcodeType.RtyCoarse:
                        for (var i = 0; i < executionContexts.Length; i += 4)
                        {
                            var topLeft = GetOperandValue(executionContexts[i + 0], instruction.Operands[1], NumberType.Float);
                            var bottomLeft = GetOperandValue(executionContexts[i + 2], instruction.Operands[1], NumberType.Float);

                            var deltaY = Number4.Subtract(ref bottomLeft, ref topLeft);

                            for (var j = i; j < i + 4; j++)
                                SetRegisterValue(executionContexts[j], instruction.Operands[0], deltaY);
                        }
                        break;
                    case ExecutableOpcodeType.RtyFine:
                        for (var i = 0; i < executionContexts.Length; i += 4)
                        {
                            var topLeft = GetOperandValue(executionContexts[i + 0], instruction.Operands[1], NumberType.Float);
                            var topRight = GetOperandValue(executionContexts[i + 1], instruction.Operands[1], NumberType.Float);
                            var bottomLeft = GetOperandValue(executionContexts[i + 2], instruction.Operands[1], NumberType.Float);
                            var bottomRight = GetOperandValue(executionContexts[i + 3], instruction.Operands[1], NumberType.Float);

                            var leftDeltaY = Number4.Subtract(ref bottomLeft, ref topLeft);
                            var rightDeltaY = Number4.Subtract(ref bottomRight, ref topRight);

                            SetRegisterValue(executionContexts[i + 0], instruction.Operands[0], leftDeltaY);
                            SetRegisterValue(executionContexts[i + 1], instruction.Operands[0], rightDeltaY);

                            SetRegisterValue(executionContexts[i + 2], instruction.Operands[0], leftDeltaY);
                            SetRegisterValue(executionContexts[i + 3], instruction.Operands[0], rightDeltaY);
                        }
                        break;
                    case ExecutableOpcodeType.Sample:
				    {
				        var srcResourceIndex = instruction.Operands[2].Indices[0].Value;
                        var srcResource = virtualMachine.Textures[srcResourceIndex];
				        var srcSampler = virtualMachine.Samplers[instruction.Operands[3].Indices[0].Value];
				        var textureSampler = virtualMachine.TextureSamplers[srcResourceIndex];

				        for (var i = 0; i < executionContexts.Length; i += 4)
				        {
                            var topLeft = GetOperandValue(executionContexts[i + 0], instruction.Operands[1], NumberType.Float);
                            var topRight = GetOperandValue(executionContexts[i + 1], instruction.Operands[1], NumberType.Float);
                            var bottomLeft = GetOperandValue(executionContexts[i + 2], instruction.Operands[1], NumberType.Float);
                            var bottomRight = GetOperandValue(executionContexts[i + 3], instruction.Operands[1], NumberType.Float);

                            var deltaX = Number4.Subtract(ref topRight, ref topLeft);
                            var deltaY = Number4.Subtract(ref bottomLeft, ref topLeft);

				            SetRegisterValue(executionContexts[i + 0], instruction.Operands[0],
                                textureSampler.SampleGrad(srcResource, srcSampler, ref topLeft,
				                    ref deltaX, ref deltaY));
                            SetRegisterValue(executionContexts[i + 1], instruction.Operands[0],
                                textureSampler.SampleGrad(srcResource, srcSampler, ref topRight,
                                    ref deltaX, ref deltaY));
                            SetRegisterValue(executionContexts[i + 2], instruction.Operands[0],
                                textureSampler.SampleGrad(srcResource, srcSampler, ref bottomLeft,
                                    ref deltaX, ref deltaY));
                            SetRegisterValue(executionContexts[i + 3], instruction.Operands[0],
                                textureSampler.SampleGrad(srcResource, srcSampler, ref bottomRight,
                                    ref deltaX, ref deltaY));
				        }
				        break;
				    }
				    case ExecutableOpcodeType.Sqrt:
				        foreach (var thread in activeExecutionContexts)
                            Execute(thread, instruction, NumberType.Float, InstructionImplementations.Sqrt);
				        break;
                    case ExecutableOpcodeType.UtoF:
				        foreach (var thread in activeExecutionContexts)
				            Execute(thread, instruction, NumberType.UInt, InstructionImplementations.UtoF);
				        break;
				    case ExecutableOpcodeType.Xor:
				        foreach (var thread in activeExecutionContexts)
                            Execute(thread, instruction, NumberType.UInt, InstructionImplementations.Xor);
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
				if (instruction.UpdateDivergenceStack(warp.DivergenceStack, activeMasks))
                    activeExecutionContexts = GetActiveExecutionContexts(executionContexts, topOfDivergenceStack);
			}
		}

        private static IList<ExecutionContext> GetActiveExecutionContexts(
            ExecutionContext[] executionContexts, DivergenceStackEntry divergenceStackEntry)
		{
            return executionContexts.Where(x => divergenceStackEntry.ActiveMask[x.Index]).ToList();
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

		private static void GetRegister(ExecutionContext context, Operand operand, out Number4[] register, out int index)
		{
			var registerIndex = GetRegisterIndex(context, operand);
			context.GetRegister(operand.OperandType, registerIndex, out register, out index);
		}

		/// <summary>
		/// Gets potentially-swizzled value for use on RHS of an operation.
		/// </summary>
		private static Number4 GetOperandValue(ExecutionContext context, Operand operand, NumberType numberType)
		{
			switch (operand.OperandType)
			{
				case OperandType.Immediate32:
                case OperandType.Immediate64:
                    return OperandUtility.ApplyOperandModifier(operand.ImmediateValues, numberType, operand.Modifier);
                case OperandType.ConstantBuffer:
				case OperandType.IndexableTemp:
				case OperandType.Input:
				case OperandType.Temp:
					Number4[] register;
					int index;
					GetRegister(context, operand, out register, out index);
					var swizzledNumber = OperandUtility.ApplyOperandSelectionMode(register[index], operand);
					return OperandUtility.ApplyOperandModifier(swizzledNumber, numberType, operand.Modifier);
				default:
					throw new ArgumentException("Unsupported operand type: " + operand.OperandType);
			}
		}

		private static RegisterIndex GetRegisterIndex(ExecutionContext context, Operand operand)
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

		private static ushort EvaluateOperandIndex(ExecutionContext context, OperandIndex index)
		{
			var result = (ushort)index.Value;
			switch (index.Representation)
			{
				case OperandIndexRepresentation.Immediate32PlusRelative:
				case OperandIndexRepresentation.Immediate64PlusRelative:
				case OperandIndexRepresentation.Relative:
					var operandValue = GetOperandValue(context, index.Register, NumberType.UInt);
					result += (ushort)operandValue.GetMaskedNumber(index.Register.ComponentMask).UInt;
					break;
			}
			return result;
		}

		private static void SetRegisterValue(ExecutionContext context, Operand operand, Number4 value)
		{
			Number4[] register;
			int index;
			GetRegister(context, operand, out register, out index);
			register[index].WriteMaskedValue(value, operand.ComponentMask);
		}

        private static void ExecuteScalar(ExecutionContext context, ExecutableInstruction instruction, NumberType numberType, Number4Number4ToNumberCallback callback)
        {
            var src0 = GetOperandValue(context, instruction.Operands[1], numberType);
            var src1 = GetOperandValue(context, instruction.Operands[2], numberType);
            var result = callback(instruction.Saturate, ref src0, ref src1);

            SetRegisterValue(context, instruction.Operands[0], new Number4
            {
                Number0 = result,
                Number1 = result,
                Number2 = result,
                Number3 = result
            });
        }

	    private delegate Number Number4Number4ToNumberCallback(bool saturate, ref Number4 src0, ref Number4 src1);

        private static void Execute(ExecutionContext context, ExecutableInstruction instruction, NumberType numberType, BoolNumber4Number4Number4ToNumber4Callback callback)
        {
            var src0 = GetOperandValue(context, instruction.Operands[1], numberType);
            var src1 = GetOperandValue(context, instruction.Operands[2], numberType);
            var src2 = GetOperandValue(context, instruction.Operands[3], numberType);
            var result = callback(instruction.Saturate, ref src0, ref src1, ref src2);

            SetRegisterValue(context, instruction.Operands[0], result);
        }

        private delegate Number4 BoolNumber4Number4Number4ToNumber4Callback(bool saturate, ref Number4 src0, ref Number4 src1, ref Number4 src2);

        private static void Execute(ExecutionContext context, ExecutableInstruction instruction, NumberType numberType, Number4Number4Number4ToNumber4Callback callback)
        {
            var src0 = GetOperandValue(context, instruction.Operands[1], numberType);
            var src1 = GetOperandValue(context, instruction.Operands[2], numberType);
            var src2 = GetOperandValue(context, instruction.Operands[3], numberType);
            var result = callback(ref src0, ref src1, ref src2);

            SetRegisterValue(context, instruction.Operands[0], result);
        }

        private delegate Number4 Number4Number4Number4ToNumber4Callback(ref Number4 src0, ref Number4 src1, ref Number4 src2);

        private static void Execute(ExecutionContext context, ExecutableInstruction instruction, NumberType numberType, BoolNumber4Number4ToNumber4Callback callback)
        {
            var src0 = GetOperandValue(context, instruction.Operands[1], numberType);
            var src1 = GetOperandValue(context, instruction.Operands[2], numberType);
            var result = callback(instruction.Saturate, ref src0, ref src1);

            SetRegisterValue(context, instruction.Operands[0], result);
        }

        private delegate Number4 BoolNumber4Number4ToNumber4Callback(bool saturate, ref Number4 src0, ref Number4 src1);

        private static void Execute(ExecutionContext context, ExecutableInstruction instruction, NumberType numberType, Number4Number4ToNumber4Callback callback)
        {
            var src0 = GetOperandValue(context, instruction.Operands[1], numberType);
            var src1 = GetOperandValue(context, instruction.Operands[2], numberType);
            var result = callback(ref src0, ref src1);

            SetRegisterValue(context, instruction.Operands[0], result);
        }

        private delegate Number4 Number4Number4ToNumber4Callback(ref Number4 src0, ref Number4 src1);

        private static void Execute(ExecutionContext context, ExecutableInstruction instruction, NumberType numberType, BoolNumber4ToNumber4Callback callback)
        {
            var src0 = GetOperandValue(context, instruction.Operands[1], numberType);
            var result = callback(instruction.Saturate, ref src0);

            SetRegisterValue(context, instruction.Operands[0], result);
        }

        private delegate Number4 BoolNumber4ToNumber4Callback(bool saturate, ref Number4 src0);

        private static void Execute(ExecutionContext context, ExecutableInstruction instruction, NumberType numberType, Number4ToNumber4Callback callback)
        {
            var src0 = GetOperandValue(context, instruction.Operands[1], numberType);
            var result = callback(ref src0);

            SetRegisterValue(context, instruction.Operands[0], result);
        }

        private delegate Number4 Number4ToNumber4Callback(ref Number4 src0);
	}
}