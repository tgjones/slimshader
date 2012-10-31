namespace SlimShader.Chunks.Common
{
	public enum ResourceReturnType
	{
		[Description("NA")]
		NotApplicable = 0,

		/// <summary>
		/// Return type is an unsigned integer value normalized to a value between 0 and 1.
		/// </summary>
		[Description("unorm")]
		UNorm = 1,

		/// <summary>
		/// Return type is a signed integer value normalized to a value between -1 and 1.
		/// </summary>
		[Description("snorm")]
		SNorm = 2,

		/// <summary>
		/// Return type is a signed integer.
		/// </summary>
		[Description("sint")]
		SInt = 3,

		/// <summary>
		/// Return type is an unsigned integer.
		/// </summary>
		[Description("uint")]
		UInt = 4,

		[Description("float")]
		Float = 5,

		/// <summary>
		/// Return type is unknown.
		/// </summary>
		[Description("mixed")]
		Mixed = 6,

		/// <summary>
		/// Return type is a double-precision value.
		/// </summary>
		[Description("double")]
		Double = 7,

		/// <summary>
		/// Return type is a multiple-dword type, such as a double or uint64, and the component is continued from the 
		/// previous component that was declared. The first component represents the lower bits.
		/// </summary>
		Continued = 8
	}
}