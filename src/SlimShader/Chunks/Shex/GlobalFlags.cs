using System;

namespace SlimShader.Chunks.Shex
{
	[Flags]
	public enum GlobalFlags
	{
		/// <summary>
		/// Refactoring allowed if bit set.
		/// </summary>
		[Description("refactoringAllowed")]
		RefactoringAllowed = 1,

		/// <summary>
		/// Enable double precision float ops.
		/// </summary>
		[Description("enableDoublePrecisionFloatOps")]
		EnableDoublePrecisionFloatOps = 2,

		/// <summary>
		/// Force early depth-stencil test.
		/// </summary>
		ForceEarlyDepthStencilTest = 4,

		/// <summary>
		/// Enable RAW and structured buffers in non-CS 4.x shaders.
		/// </summary>
		[Description("enableRawAndStructuredBuffers")]
		EnableRawAndStructuredBuffersInNonCsShaders = 8,

		/// <summary>
		/// Skip optimizations of shader IL when translating to native code
		/// </summary>
		SkipOptimizationsOfShaderIl = 16,

		/// <summary>
		/// Enable minimum-precision data types
		/// </summary>
		EnableMinimumPrecisionDataTypes = 32,

		/// <summary>
		/// Enable 11.1 double-precision floating-point instruction extensions
		/// </summary>
		Enable11_1DoublePrecisionInstructionExtensions = 64,

		/// <summary>
		/// Enable 11.1 non-double instruction extensions
		/// </summary>
		Enable11_1NonDoubleInstructionExtensions = 128,
	}
}