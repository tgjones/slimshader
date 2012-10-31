using SlimShader.Util;

namespace SlimShader.Chunks.Rdef
{
	/// <summary>
	/// Indicates a constant buffer's type.
	/// Based on D3D11_CBUFFER_TYPE.
	/// </summary>
	public enum ConstantBufferType
	{
		/// <summary>
		/// A buffer containing scalar constants.
		/// </summary>
		[Description("cbuffer")]
		ConstantBuffer,

		/// <summary>
		/// A buffer containing texture data.
		/// </summary>
		TextureBuffer,

		/// <summary>
		/// A buffer containing interface pointers.
		/// </summary>
		[Description("interfaces")]
		InterfacePointers,

		/// <summary>
		/// A buffer containing binding information.
		/// </summary>
		[Description("Resource bind info for")]
		ResourceBindInformation
	}
}