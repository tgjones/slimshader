namespace SlimShader.Chunks.Shex
{
	public enum ResourceDimension
	{
		/// <summary>
		/// The type is unknown.
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// The resource is a buffer.
		/// </summary>
		[Description("buffer")]
		Buffer = 1,

		/// <summary>
		/// The resource is a 1D texture.
		/// </summary>
		[Description("texture1d")]
		Texture1D = 2,

		/// <summary>
		/// The resource is a 2D texture.
		/// </summary>
		[Description("texture2d")]
		Texture2D = 3,

		/// <summary>
		/// The resource is a multisampling 2D texture.
		/// </summary>
		[Description("texture2dms")]
		Texture2DMultiSampled = 4,

		/// <summary>
		/// The resource is a 3D texture.
		/// </summary>
		[Description("texture3d")]
		Texture3D = 5,

		/// <summary>
		/// The resource is a cube texture.
		/// </summary>
		[Description("texturecube")]
		TextureCube = 6,

		/// <summary>
		/// The resource is an array of 1D textures.
		/// </summary>
		[Description("texture1darray")]
		Texture1DArray = 7,

		/// <summary>
		/// The resource is an array of 2D textures.
		/// </summary>
		[Description("texture2darray")]
		Texture2DArray = 8,

		/// <summary>
		/// The resource is an array of multisampling 2D textures.
		/// </summary>
		[Description("texture2dmsarray")]
		Texture2DMultiSampledArray = 9,

		/// <summary>
		/// The resource is an array of cube textures.
		/// </summary>
		[Description("texturecubearray")]
		TextureCubeArray = 10,

		/// <summary>
		/// The resource is a raw buffer.
		/// </summary>
		[Description("raw_buffer")]
		RawBuffer = 11,

		/// <summary>
		/// The resource is a structured buffer.
		/// </summary>
		[Description("structured_buffer")]
		StructuredBuffer = 12,
	}
}