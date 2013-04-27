namespace SlimShader.Chunks.Shex
{
	public enum OpcodeType
	{
		[NumberType(NumberType.Float)]
		[Description("add")]
		Add,

		[NumberType(NumberType.UInt)]
		[Description("and")]
		And,

		[Description("break")]
		Break,

		[Description("breakc")]
		BreakC,

		[Description("call")]
		Call,

		[Description("callc")]
		CallC,

		[Description("case")]
		Case,

		[Description("continue")]
		Continue,

		[Description("continuec")]
		ContinueC,

		[Description("cut")]
		Cut,

		[Description("default")]
		Default,

		[Description("deriv_rtx")]
		DerivRtx,

		[Description("deriv_rty")]
		DerivRty,

		[Description("discard")]
		Discard,

		[NumberType(NumberType.Float)]
		[Description("div")]
		Div,

		[NumberType(NumberType.Float)]
		[Description("dp2")]
		Dp2,

		[NumberType(NumberType.Float)]
		[Description("dp3")]
		Dp3,

		[NumberType(NumberType.Float)]
		[Description("dp4")]
		Dp4,

		[Description("else")]
		Else,

		[Description("emit")]
		Emit,

		[Description("emitThenCut")]
		EmitThenCut,

		[Description("endif")]
		EndIf,

		[Description("endloop")]
		EndLoop,

		[Description("endswitch")]
		EndSwitch,

		[NumberType(NumberType.Float)]
		[Description("eq")]
		Eq,

		[NumberType(NumberType.Float)]
		[Description("exp")]
		Exp,

		[NumberType(NumberType.Float)]
		[Description("frc")]
		Frc,

		[Description("ftoi")]
		FtoI,

		[Description("ftou")]
		FtoU,

		[NumberType(NumberType.Float)]
		[Description("ge")]
		Ge,

		[NumberType(NumberType.Int)]
		[Description("iadd")]
		IAdd,

		[Description("if")]
		If,

		[NumberType(NumberType.Int)]
		[Description("ieq")]
		IEq,

		[NumberType(NumberType.Int)]
		[Description("ige")]
		IGe,

		[NumberType(NumberType.Int)]
		[Description("ilt")]
		ILt,

		[NumberType(NumberType.Int)]
		[Description("imad")]
		IMad,

		[NumberType(NumberType.Int)]
		[Description("imax")]
		IMax,

		[NumberType(NumberType.Int)]
		[Description("imin")]
		IMin,

		[NumberType(NumberType.Int)]
		[Description("imul")]
		IMul,

		[NumberType(NumberType.Int)]
		[Description("ine")]
		INe,

		[NumberType(NumberType.Int)]
		[Description("ineg")]
		INeg,

		[NumberType(NumberType.Int)]
		[Description("ishl")]
		IShl,

		[NumberType(NumberType.Int)]
		[Description("ishr")]
		IShr,

		[Description("itof")]
		ItoF,

		[Description("label")]
		Label,

		[NumberType(NumberType.UInt)]
		[Description("ld")]
		Ld,

		[NumberType(NumberType.UInt)]
		[Description("ldms")]
		LdMs,

		[NumberType(NumberType.Float)]
		[Description("log")]
		Log,

		[Description("loop")]
		Loop,

		[NumberType(NumberType.Float)]
		[Description("lt")]
		Lt,

		[NumberType(NumberType.Float)]
		[Description("mad")]
		Mad,

		[NumberType(NumberType.Float)]
		[Description("min")]
		Min,

		[NumberType(NumberType.Float)]
		[Description("max")]
		Max,

		CustomData,

		[Description("mov")]
		Mov,

		[Description("movc")]
		MovC,

		[NumberType(NumberType.Float)]
		[Description("mul")]
		Mul,

		[NumberType(NumberType.Float)]
		[Description("ne")]
		Ne,

		[Description("nop")]
		Nop,

		[Description("not")]
		Not,

		[NumberType(NumberType.UInt)]
		[Description("or")]
		Or,

		[Description("resinfo")]
		Resinfo,

		[Description("ret")]
		Ret,

		[Description("retc")]
		RetC,

		[NumberType(NumberType.Float)]
		[Description("round_ne")]
		RoundNe,

		[NumberType(NumberType.Float)]
		[Description("round_ni")]
		RoundNi,

		[NumberType(NumberType.Float)]
		[Description("round_pi")]
		RoundPi,

		[NumberType(NumberType.Float)]
		[Description("round_z")]
		RoundZ,

		[NumberType(NumberType.Float)]
		[Description("rsq")]
		Rsq,

		[NumberType(NumberType.Float)]
		[Description("sample")]
		Sample,

		[NumberType(NumberType.Float)]
		[Description("sample_c")]
		SampleC,

		[NumberType(NumberType.Float)]
		[Description("sample_c_lz")]
		SampleCLz,

		[NumberType(NumberType.Float)]
		[Description("sample_l")]
		SampleL,

		[NumberType(NumberType.Float)]
		[Description("sample_d")]
		SampleD,

		[NumberType(NumberType.Float)]
		[Description("sample_b")]
		SampleB,

		[NumberType(NumberType.Float)]
		[Description("sqrt")]
		Sqrt,

		[NumberType(NumberType.Float)]
		[Description("switch")]
		Switch,

		[NumberType(NumberType.Float)]
		[Description("sincos")]
		Sincos,

		[NumberType(NumberType.UInt)]
		[Description("udiv")]
		UDiv,

		[NumberType(NumberType.UInt)]
		[Description("ult")]
		ULt,

		[NumberType(NumberType.UInt)]
		[Description("uge")]
		UGe,

		[NumberType(NumberType.UInt)]
		[Description("umul")]
		UMul,

		[NumberType(NumberType.UInt)]
		[Description("umad")]
		UMad,

		[NumberType(NumberType.UInt)]
		[Description("umax")]
		UMax,

		[NumberType(NumberType.UInt)]
		[Description("umin")]
		UMin,

		[NumberType(NumberType.UInt)]
		[Description("ushr")]
		UShr,

		[Description("utof")]
		Utof,

		[NumberType(NumberType.UInt)]
		[Description("xor")]
		Xor,

		[Description("dcl_resource")]
		DclResource,

		[Description("dcl_constantbuffer")]
		DclConstantBuffer,

		[Description("dcl_sampler")]
		DclSampler,

		[Description("dcl_indexrange")]
		DclIndexRange,

		[Description("dcl_outputtopology")]
		DclGsOutputPrimitiveTopology,

		[Description("dcl_inputprimitive")]
		DclGsInputPrimitive,

		[Description("dcl_maxout")]
		DclMaxOutputVertexCount,

		[Description("dcl_input")]
		DclInput,

		[Description("dcl_input_sgv")]
		DclInputSgv,

		[Description("dcl_input_siv")]
		DclInputSiv,

		[Description("dcl_input_ps")]
		DclInputPs,

		[Description("dcl_input_ps_sgv")]
		DclInputPsSgv,

		[Description("dcl_input_ps_siv")]
		DclInputPsSiv,

		[Description("dcl_output")]
		DclOutput,

		[Description("dcl_output_sgv")]
		DclOutputSgv,

		[Description("dcl_output_siv")]
		DclOutputSiv,

		[Description("dcl_temps")]
		DclTemps,

		[Description("dcl_indexableTemp")]
		DclIndexableTemp,

		[Description("dcl_globalFlags")]
		DclGlobalFlags,

		/// <summary>
		/// This marks the end of D3D10.0 opcodes
		/// </summary>
		D3D10Count,

		// DX 10.1 op codes

		[Description("lod")]
		Lod,

		[Description("gather4")]
		Gather4,

		[Description("samplepos")]
		SamplePos,

		[Description("sampleinfo")]
		SampleInfo,

		/// <summary>
		/// This marks the end of D3D10.1 opcodes
		/// </summary>
		D3D10_1Count,

		// DX 11 op codes

		/// <summary>
		/// Token marks beginning of HS sub-shader
		/// </summary>
		[Description("hs_decls")]
		HsDecls,

		/// <summary>
		/// Token marks beginning of HS sub-shader
		/// </summary>
		[Description("hs_control_point_phase")]
		HsControlPointPhase,

		/// <summary>
		/// Token marks beginning of HS sub-shader
		/// </summary>
		[Description("hs_fork_phase")]
		HsForkPhase,

		/// <summary>
		/// Token marks beginning of HS sub-shader
		/// </summary>
		[Description("hs_join_phase")]
		HsJoinPhase,

		[Description("emit_stream")]
		EmitStream,

		[Description("cut_stream")]
		CutStream,

		// TODO: Finish descriptions.
		EmitThenCutStream,

		[Description("fcall")]
		InterfaceCall,

		Bufinfo,

		[Description("deriv_rtx_coarse")]
		RtxCoarse,

		[Description("deriv_rtx_fine")]
		RtxFine,

		[Description("deriv_rty_coarse")]
		RtyCoarse,

		[Description("deriv_rty_fine")]
		RtyFine,

		[Description("gather4_c")]
		Gather4C,

		[Description("gather4_po")]
		Gather4Po,

		[Description("gather4_po_c")]
		Gather4PoC,

		[Description("rcp")]
		Rcp,

		[NumberType(NumberType.Float)]
		F32ToF16,

		[NumberType(NumberType.Float)]
		F16ToF32,

		[NumberType(NumberType.UInt)]
		UAddC,

		[NumberType(NumberType.UInt)]
		USubB,

		[NumberType(NumberType.UInt)]
		[Description("countbits")]
		CountBits,

		[NumberType(NumberType.UInt)]
		FirstBitHi,

		[NumberType(NumberType.UInt)]
		FirstBitLo,

		[NumberType(NumberType.UInt)]
		FirstBitSHi,

		[NumberType(NumberType.UInt)]
		[Description("ubfe")]
		UBfe,

		[NumberType(NumberType.Int)]
		[Description("ibfe")]
		IBfe,

		[NumberType(NumberType.UInt)]
		[Description("bfi")]
		Bfi,

		[Description("bfrev")]
		BfRev,

		[Description("swapc")]
		SwapC,

		[Description("dcl_stream")]
		DclStream,

		[Description("dcl_function_body")]
		DclFunctionBody,

		[Description("dcl_function_table")]
		DclFunctionTable,

		[Description("dcl_interface")]
		DclInterface,

		[Description("dcl_input_control_point_count")]
		DclInputControlPointCount,

		[Description("dcl_output_control_point_count")]
		DclOutputControlPointCount,

		[Description("dcl_tessellator_domain")]
		DclTessDomain,

		[Description("dcl_tessellator_partitioning")]
		DclTessPartitioning,

		[Description("dcl_tessellator_output_primitive")]
		DclTessOutputPrimitive,

		[Description("dcl_hs_max_tessfactor")]
		DclHsMaxTessFactor,

		[Description("dcl_hs_fork_phase_instance_count")]
		DclHsForkPhaseInstanceCount,

		[Description("dcl_hs_join_phase_instance_count")]
		DclHsJoinPhaseInstanceCount,

		[Description("dcl_thread_group")]
		DclThreadGroup,

		[Description("dcl_uav_typed")]
		DclUnorderedAccessViewTyped,

		[Description("dcl_uav_raw")]
		DclUnorderedAccessViewRaw,

		[Description("dcl_uav_structured")]
		DclUnorderedAccessViewStructured,

		[Description("dcl_tgsm_raw")]
		DclThreadGroupSharedMemoryRaw,

		[Description("dcl_tgsm_structured")]
		DclThreadGroupSharedMemoryStructured,

		[Description("dcl_resource_raw")]
		DclResourceRaw,

		[Description("dcl_resource_structured")]
		DclResourceStructured,

		[Description("ld_uav_typed")]
		LdUavTyped,

		[Description("store_uav_typed")]
		StoreUavTyped,

		[Description("ld_raw")]
		LdRaw,

		[Description("store_raw")]
		StoreRaw,

		[Description("ld_structured")]
		LdStructured,

		[Description("store_structured")]
		StoreStructured,

		[NumberType(NumberType.UInt)]
		AtomicAnd,

		[NumberType(NumberType.UInt)]
		AtomicOr,

		[NumberType(NumberType.UInt)]
		AtomicXor,

		AtomicCmpStore,

		[NumberType(NumberType.Int)]
		AtomicIAdd,

		[NumberType(NumberType.Int)]
		AtomicIMax,

		[NumberType(NumberType.Int)]
		AtomicIMin,

		[NumberType(NumberType.UInt)]
		AtomicUMax,

		[NumberType(NumberType.UInt)]
		AtomicUMin,

		ImmAtomicAlloc,
		ImmAtomicConsume,

		[NumberType(NumberType.Int)]
		ImmAtomicIAdd,

		[NumberType(NumberType.UInt)]
		ImmAtomicAnd,

		[NumberType(NumberType.UInt)]
		ImmAtomicOr,

		[NumberType(NumberType.UInt)]
		ImmAtomicXor,

		ImmAtomicExch,
		ImmAtomicCmpExch,

		[NumberType(NumberType.Int)]
		ImmAtomicIMax,

		[NumberType(NumberType.Int)]
		ImmAtomicIMin,

		[NumberType(NumberType.UInt)]
		ImmAtomicUMax,

		[NumberType(NumberType.UInt)]
		ImmAtomicUMin,

		[Description("sync")]
		Sync,

		[NumberType(NumberType.Double)]
		[Description("dadd")]
		DAdd,

		[NumberType(NumberType.Double)]
		DMax,

		[NumberType(NumberType.Double)]
		DMin,

		[NumberType(NumberType.Double)]
		DMul,

		[NumberType(NumberType.Double)]
		DEq,

		[NumberType(NumberType.Double)]
		DGe,

		[NumberType(NumberType.Double)]
		DLt,

		[NumberType(NumberType.Double)]
		DNe,

		[NumberType(NumberType.Double)]
		DMov,

		[NumberType(NumberType.Double)]
		DMovC,

		DToD,
		FToD,
		EvalSnapped,
		EvalSampleIndex,
		EvalCentroid,
		DclGsInstanceCount,

		[Description("abort")]
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
	}
}