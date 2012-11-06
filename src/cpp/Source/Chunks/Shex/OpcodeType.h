#pragma once

#include "PCH.h"
#include "NumberType.h"

namespace SlimShader
{
	enum class OpcodeType
	{
		Add,
		And,
		Break,
		BreakC,
		Call,
		CallC,
		Case,
		Continue,
		ContinueC,
		Cut,
		Default,
		DerivRtx,
		DerivRty,
		Discard,
		Div,
		Dp2,
		Dp3,
		Dp4,
		Else,
		Emit,
		EmitThenCut,
		EndIf,
		EndLoop,
		EndSwitch,
		Eq,
		Exp,
		Frc,
		FtoI,
		FtoU,
		Ge,
		IAdd,
		If,
		IEq,
		IGe,
		ILt,
		IMad,
		IMax,
		IMin,
		IMul,
		INe,
		INeg,
		IShl,
		IShr,
		IToF,
		Label,
		Ld,
		LdMs,
		Log,
		Loop,
		Lt,
		Mad,
		Min,
		Max,
		CustomData,
		Mov,
		MovC,
		Mul,
		Ne,
		Nop,
		Not,
		Or,
		Resinfo,
		Ret,
		RetC,
		RoundNe,
		RoundNi,
		RoundPi,
		RoundZ,
		Rsq,
		Sample,
		SampleC,
		SampleCLz,
		SampleL,
		SampleD,
		SampleB,
		Sqrt,
		Switch,
		Sincos,
		UDiv,
		ULt,
		UGe,
		UMul,
		UMad,
		UMax,
		UMin,
		UShr,
		UTof,
		Xor,
		DclResource,
		DclConstantBuffer,
		DclSampler,
		DclIndexRange,
		DclGsOutputPrimitiveTopology,
		DclGsInputPrimitive,
		DclMaxOutputVertexCount,
		DclInput,
		DclInputSgv,
		DclInputSiv,
		DclInputPs,
		DclInputPsSgv,
		DclInputPsSiv,
		DclOutput,
		DclOutputSgv,
		DclOutputSiv,
		DclTemps,
		DclIndexableTemp,
		DclGlobalFlags,

		/// <summary>
		/// This marks the end of D3D10.0 opcodes
		/// </summary>
		D3D10Count,

		// DX 10.1 op codes

		Lod,
		Gather4,
		SamplePos,
		SampleInfo,

		/// <summary>
		/// This marks the end of D3D10.1 opcodes
		/// </summary>
		D3D10_1Count,

		// DX 11 op codes

		/// <summary>
		/// Token marks beginning of HS sub-shader
		/// </summary>
		HsDecls,

		/// <summary>
		/// Token marks beginning of HS sub-shader
		/// </summary>
		HsControlPointPhase,

		/// <summary>
		/// Token marks beginning of HS sub-shader
		/// </summary>
		HsForkPhase,

		/// <summary>
		/// Token marks beginning of HS sub-shader
		/// </summary>
		HsJoinPhase,

		EmitStream,
		CutStream,

		// TODO: Finish descriptions.
		EmitThenCutStream,
		InterfaceCall,
		Bufinfo,
		RtxCoarse,
		RtxFine,
		RtyCoarse,
		RtyFine,
		Gather4C,
		Gather4Po,
		Gather4PoC,
		Rcp,
		F32ToF16,
		F16ToF32,
		UAddC,
		USubB,
		CountBits,
		FirstBitHi,
		FirstBitLo,
		FirstBitSHi,
		UBfe,
		IBfe,
		Bfi,
		BfRev,
		SwapC,
		DclStream,
		DclFunctionBody,
		DclFunctionTable,
		DclInterface,
		DclInputControlPointCount,
		DclOutputControlPointCount,
		DclTessDomain,
		DclTessPartitioning,
		DclTessOutputPrimitive,
		DclHsMaxTessFactor,
		DclHsForkPhaseInstanceCount,
		DclHsJoinPhaseInstanceCount,
		DclThreadGroup,
		DclUnorderedAccessViewTyped,
		DclUnorderedAccessViewRaw,
		DclUnorderedAccessViewStructured,
		DclThreadGroupSharedMemoryRaw,
		DclThreadGroupSharedMemoryStructured,
		DclResourceRaw,
		DclResourceStructured,
		LdUavTyped,
		StoreUavTyped,
		LdRaw,
		StoreRaw,
		LdStructured,
		StoreStructured,
		AtomicAnd,
		AtomicOr,
		AtomicXor,
		AtomicCmpStore,
		AtomicIAdd,
		AtomicIMax,
		AtomicIMin,
		AtomicUMax,
		AtomicUMin,
		ImmAtomicAlloc,
		ImmAtomicConsume,
		ImmAtomicIAdd,
		ImmAtomicAnd,
		ImmAtomicOr,
		ImmAtomicXor,
		ImmAtomicExch,
		ImmAtomicCmpExch,
		ImmAtomicIMax,
		ImmAtomicIMin,
		ImmAtomicUMax,
		ImmAtomicUMin,
		Sync,
		DAdd,
		DMax,
		DMin,
		DMul,
		DEq,
		DGe,
		DLt,
		DNe,
		DMov,
		DMovC,
		DToD,
		FToD,
		EvalSnapped,
		EvalSampleIndex,
		EvalCentroid,
		DclGsInstanceCount,
		Abort,
		DebugBreak,

		/// <summary>
		/// This marks the end of D3D11.0 opcodes
		/// </summary>
		D3D11_0Count,

		Ddiv,
		Dfma,
		Drcp,

		Msad,

		Dtoi,
		Dtou,
		Itod,
		Utod
	};

	NumberType GetNumberType(OpcodeType value);
	std::string ToString(OpcodeType value);
	bool IsConditionalInstruction(OpcodeType type);
	bool IsDeclaration(OpcodeType type);
};