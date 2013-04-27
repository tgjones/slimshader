using SlimShader.Util;

namespace SlimShader.Chunks.Rdef
{
	/// <summary>
	/// These flags identify various data, texture, and buffer types that can be assigned to a shader variable.
	/// Based on D3D10_SHADER_VARIABLE_TYPE .
	/// </summary>
	public enum ShaderVariableType
	{
		Void = 0,

		[Description("bool")]
		Bool = 1,

		[Description("int")]
		Int = 2,

		[Description("float")]
		Float = 3,

		String = 4,
		Texture = 5,
		Texture1D = 6,
		Texture2D = 7,
		Texture3D = 8,
		TextureCube = 9,
		Sampler = 10,
		PixelShader = 15,
		VertexShader = 16,

		[Description("uint")]
		UInt = 19,

		UInt8 = 20,
		GeometryShader = 21,
		Rasterizer = 22,
		DepthStencil = 23,
		Blend = 24,
		Buffer = 25,
		CBuffer = 26,
		TBuffer = 27,
		Texture1DArray = 28,
		Texture2DArray = 29,
		RenderTargetView = 30,
		DepthStencilView = 31,
		Texture2DMultiSampled = 32,
		Texture2DMultiSampledArray = 33,
		TextureCubeArray = 34,

		// The following are new in D3D11.

		HullShader = 35,
		DomainShader = 36,

		[Description("interface")]
		InterfacePointer = 37,

		ComputeShader = 38,

		[Description("double")]
		Double = 39,

		ReadWriteTexture1D,
		ReadWriteTexture1DArray,
		ReadWriteTexture2D,
		ReadWriteTexture2DArray,
		ReadWriteTexture3D,
		ReadWriteBuffer,
		ByteAddressBuffer,
		ReadWriteByteAddressBuffer,
		StructuredBuffer,
		ReadWriteStructuredBuffer,
		AppendStructuredBuffer,
		ConsumeStructuredBuffer,
	}
}