﻿namespace SlimShader.VirtualMachine.Execution
{
	public enum ExecutableOpcodeType
	{
		Add,
		And,
		Branch,
		BranchC,
		Call,
		CallC,
		Cut,
		DerivRtx,
		DerivRty,
		Discard,
		Div,
		Dp2,
		Dp3,
		Dp4,
		Emit,
		EmitThenCut,
		Eq,
		Exp,
		Frc,
		FtoI,
		FtoU,
		Ge,
		IAdd,
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
		ItoF,
		Label,
		Ld,
		LdMs,
		Log,
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
		UtoF,
		Xor,

		Lod,
		Gather4,
		SamplePos,
		SampleInfo,

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

		Ddiv,
		Dfma,
		Drcp,

		Msad,

		Dtoi,
		Dtou,
		Itod,
		Utod
	}
}