namespace SlimShader.Chunks.Rdef
{
	/// <summary>
	/// Very similar to Shader.ResourceDimension, but this one is used within the RDEF (Resource Definition) chunk.
	/// The multi-sampled values are different from ResourceDimension.
	/// </summary>
	public enum ShaderResourceViewDimension
	{
		/// <summary>
		/// The type is unknown.
		/// </summary>
		[Description("NA")]
		Unknown = 0,

		/// <summary>
		/// The resource is a buffer.
		/// </summary>
		[Description("buf")]
		Buffer = 1,

		/// <summary>
		/// The resource is a 1D texture.
		/// </summary>
		[Description("1d")]
		Texture1D = 2,

		/// <summary>
		/// The resource is an array of 1D textures.
		/// </summary>
		[Description("1darray")]
		Texture1DArray = 3,

		/// <summary>
		/// The resource is a 2D texture.
		/// </summary>
		[Description("2d")]
		Texture2D = 4,

		/// <summary>
		/// The resource is an array of 2D textures.
		/// </summary>
		[Description("2darray")]
		Texture2DArray = 5,

		/// <summary>
		/// The resource is a multisampling 2D texture.
		/// </summary>
		[Description("2dMS")]
		Texture2DMultiSampled = 6,

		/// <summary>
		/// The resource is an array of multisampling 2D textures.
		/// </summary>
		[Description("2dMSarray")]
		Texture2DMultiSampledArray = 7,

		/// <summary>
		/// The resource is a 3D texture.
		/// </summary>
		[Description("3d")]
		Texture3D = 8,

		/// <summary>
		/// The resource is a cube texture.
		/// </summary>
		[Description("cube")]
		TextureCube = 9,

		/// <summary>
		/// The resource is an array of cube textures.
		/// </summary>
		[Description("cubearray")]
		TextureCubeArray = 10,

		/// <summary>
		/// The resource is a raw buffer.
		/// </summary>
		ExtendedBuffer = 11
	}
}