using System;

namespace SlimShader.Chunks.Rdef
{
	/// <summary>
	/// Values that identify the indended use of a constant-data buffer.
	/// Based on D3D_SHADER_CBUFFER_FLAGS.
	/// </summary>
	[Flags]
	public enum ConstantBufferFlags
	{
		None = 0,

		/// <summary>
		/// Bind the constant buffer to an input slot defined in HLSL code (instead of letting the compiler choose the 
		/// input slot).
		/// </summary>
		UserPacked = 1
	}
}