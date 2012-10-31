using System;

namespace SlimShader.Chunks.Rdef
{
	/// <summary>
	/// Values that identify information about a shader variable.
	/// Based on D3D10_SHADER_VARIABLE_FLAGS.
	/// </summary>
	[Flags]
	public enum ShaderVariableFlags
	{
		None = 0,

		/// <summary>
		/// Indicates that the registers assigned to this shader variable were explicitly declared in shader code
		/// (instead of automatically assigned by the compiler).
		/// </summary>
		UserPacked = 1,

		/// <summary>
		/// Indicates that this variable is used by this shader. This value confirms that a particular shader variable 
		/// (which can be common to many different shaders) is indeed used by a particular shader.
		/// </summary>
		Used = 2,

		/// <summary>
		/// Indicates that this variable is an interface.
		/// </summary>
		InterfacePointer = 4,

		/// <summary>
		/// Indicates that this variable is a parameter of an interface.
		/// </summary>
		InterfaceParameter = 8
	}
}