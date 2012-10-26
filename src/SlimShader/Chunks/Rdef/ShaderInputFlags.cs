using System;

namespace SlimShader.Chunks.Rdef
{
	[Flags]
	public enum ShaderInputFlags
	{
		None,

		/// <summary>
		/// Assign a shader input to a register based on the register assignment in the HLSL code (instead of letting 
		/// the compiler choose the register).
		/// </summary>
		UserPacked = 0x1,

		/// <summary>
		/// Use a comparison sampler, which uses the SampleCmp (DirectX HLSL Texture Object) and SampleCmpLevelZero 
		/// (DirectX HLSL Texture Object) sampling functions.
		/// </summary>
		ComparisonSampler = 0x2,

		/// <summary>
		/// A 2-bit value for encoding texture components.
		/// </summary>
		TextureComponent0 = 0x4,

		/// <summary>
		/// A 2-bit value for encoding texture components.
		/// </summary>
		TextureComponent1 = 0x8,

		/// <summary>
		/// A 2-bit value for encoding texture components.
		/// </summary>
		TextureComponents = 0xc,

		/// <summary>
		/// This value is reserved.
		/// </summary>
		Unused = 0x10
	}
}