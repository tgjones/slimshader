using SlimShader.Util;

namespace SlimShader.Chunks.Rdef
{
	/// <summary>
	/// These flags identify the shader-variable class.
	/// Based on D3D10_SHADER_VARIABLE_CLASS.
	/// </summary>
	public enum ShaderVariableClass
	{
		/// <summary>
		/// The shader variable is a scalar.
		/// </summary>
		Scalar,

		/// <summary>
		/// The shader variable is a vector.
		/// </summary>
		[Description("")]
		Vector,

		/// <summary>
		/// The shader variable is a row-major matrix.
		/// </summary>
		[Description("row_major ")]
		MatrixRows,

		/// <summary>
		/// The shader variable is a column-major matrix.
		/// </summary>
		[Description("")]
		MatrixColumns,

		/// <summary>
		/// The shader variable is an object.
		/// </summary>
		Object,

		/// <summary>
		/// The shader variable is a structure.
		/// </summary>
		Struct,

		/// <summary>
		/// The shader variable is a class.
		/// </summary>
		InterfaceClass,

		/// <summary>
		/// The shader variable is an interface.
		/// </summary>
		[Description("interface ")]
		InterfacePointer
	}
}