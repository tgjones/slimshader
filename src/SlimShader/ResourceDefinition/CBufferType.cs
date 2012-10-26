using SlimShader.Util;

namespace SlimShader.ResourceDefinition
{
	/// <summary>
	/// Indicates a constant buffer's type.
	/// Based on D3D11_CBUFFER_TYPE.
	/// </summary>
	public enum CBufferType
	{
		/// <summary>
		/// A buffer containing scalar constants.
		/// </summary>
		[Description("cbuffer")]
		CBuffer,

		/// <summary>
		/// A buffer containing texture data.
		/// </summary>
		TBuffer,

		/// <summary>
		/// A buffer containing interface pointers.
		/// </summary>
		[Description("interfaces")]
		InterfacePointers,

		/// <summary>
		/// A buffer containing binding information.
		/// </summary>
		[Description("Resource bind info for")]
		ResourceBindInfo
	}
}