namespace SlimShader.Chunks.Shex
{
	public enum OperandType
	{
		/// <summary>
		/// Temporary Register File
		/// </summary>
		[Description("r")]
		Temp = 0,

		/// <summary>
		/// General Input Register File
		/// </summary>
		[Description("v")]
		Input = 1,

		/// <summary>
		/// General Output Register File
		/// </summary>
		[Description("o")]
		Output = 2,

		/// <summary>
		/// Temporary Register File (indexable)
		/// </summary>
		[Description("x")]
		IndexableTemp = 3,

		/// <summary>
		/// 32bit/component immediate value(s)
		/// // If for example, operand token bits
		/// [01:00]==OPERAND_4_COMPONENT,
		/// this means that the operand type:
		/// OPERAND_TYPE_IMMEDIATE32
		/// results in 4 additional 32bit
		/// DWORDS present for the operand.
		/// </summary>
		Immediate32 = 4,
		
		/// <summary>
		/// // 64bit/comp.imm.val(s)HI:LO
		/// </summary>
		Immediate64 = 5,

		/// <summary>
		/// Reference to sampler state
		/// </summary>
		[Description("s")]
		Sampler = 6,

		/// <summary>
		/// Reference to memory resource (e.g. texture)
		/// </summary>
		[Description("t")]
		Resource = 7,

		/// <summary>
		/// Reference to constant buffer
		/// </summary>
		[Description("cb")]
		ConstantBuffer = 8,

		/// <summary>
		/// Reference to immediate constant buffer
		/// </summary>
		[Description("icb")]
		ImmediateConstantBuffer = 9,

		/// <summary>
		/// Label
		/// </summary>
		Label = 10,

		/// <summary>
		/// Input primitive ID
		/// </summary>
		[Description("vPrim")]
		InputPrimitiveID = 11,

		/// <summary>
		/// Output Depth
		/// </summary>
		OutputDepth = 12,

		/// <summary>
		/// Null register, used to discard results of operations
		/// </summary>
		[Description("null")]
		Null = 13,

		// Below are operands new in DX 10.1
		
		/// <summary>
		/// DX10.1 Rasterizer register, used to denote the depth/stencil and render target resources
		/// </summary>
		[Description("rasterizer")]
		Rasterizer = 14,

		/// <summary>
		/// DX10.1 PS output MSAA coverage mask (scalar)
		/// </summary>
		OutputCoverageMask = 15,

		// Below Are operands new in DX 11

		/// <summary>
		/// Reference to GS stream output resource
		/// </summary>
		[Description("m")]
		Stream = 16,

		/// <summary>
		/// Reference to a function definition
		/// </summary>
		[Description("fb")]
		FunctionBody = 17,

		/// <summary>
		/// Reference to a set of functions used by a class
		/// </summary>
		FunctionTable = 18,

		/// <summary>
		/// Reference to an interface
		/// </summary>
		Interface = 19,

		/// <summary>
		/// Reference to an input parameter to a function
		/// </summary>
		FunctionInput = 20,

		/// <summary>
		/// Reference to an output parameter to a function
		/// </summary>
		FunctionOutput = 21,

		/// <summary>
		/// HS Control Point phase input saying which output control point ID this is
		/// </summary>
		[Description("vOutputControlPointID")]
		OutputControlPointID = 22,

		/// <summary>
		/// HS Fork Phase input instance ID
		/// </summary>
		[Description("vForkInstanceID")]
		InputForkInstanceID = 23,

		/// <summary>
		/// HS Join Phase input instance ID
		/// </summary>
		InputJoinInstanceID = 24,

		/// <summary>
		/// HS Fork+Join, DS phase input control points (array of them)
		/// </summary>
		[Description("vicp")]
		InputControlPoint = 25,

		/// <summary>
		/// HS Fork+Join phase output control points (array of them)
		/// </summary>
		OutputControlPoint = 26,

		/// <summary>
		/// DS+HSJoin Input Patch Constants (array of them)
		/// </summary>
		[Description("vpc")]
		InputPatchConstant = 27,

		/// <summary>
		/// DS Input Domain point
		/// </summary>
		[Description("vDomain")]
		InputDomainPoint = 28,

		/// <summary>
		/// Reference to an interface this pointer
		/// </summary>
		[Description("this")]
		ThisPointer = 29,

		/// <summary>
		/// Reference to UAV u#
		/// </summary>
		[Description("u")]
		UnorderedAccessView = 30,

		/// <summary>
		/// Reference to Thread Group Shared Memory g#
		/// </summary>
		[Description("g")]
		ThreadGroupSharedMemory = 31,

		/// <summary>
		/// Compute Shader Thread ID
		/// </summary>
		[Description("vThreadID")]
		InputThreadID = 32,

		/// <summary>
		/// Compute Shader Thread Group ID
		/// </summary>
		[Description("vThreadGroupID")]
		InputThreadGroupID = 33,

		/// <summary>
		/// Compute Shader Thread ID In Thread Group
		/// </summary>
		[Description("vThreadIDInGroup")]
		InputThreadIDInGroup = 34,

		/// <summary>
		/// Pixel shader coverage mask input
		/// </summary>
		[Description("vCoverage")]
		InputCoverageMask = 35,

		/// <summary>
		/// Compute Shader Thread ID In Group Flattened to a 1D value.
		/// </summary>
		[Description("vThreadIDInGroupFlattened")]
		InputThreadIDInGroupFlattened = 36,

		/// <summary>
		/// Input GS instance ID
		/// </summary>
		InputGSInstanceID = 37,

		/// <summary>
		/// Output Depth, forced to be greater than or equal than current depth
		/// </summary>
		[Description("oDepthGE")]
		OutputDepthGreaterEqual = 38,

		/// <summary>
		/// Output Depth, forced to be less than or equal to current depth
		/// </summary>
		[Description("oDepthLE")]
		OutputDepthLessEqual = 39,

		/// <summary>
		/// Cycle counter
		/// </summary>
		CycleCounter = 40
	}
}