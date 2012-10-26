namespace SlimShader.Chunks.Shex
{
	public enum OperandIndexRepresentation
	{
		/// <summary>
		/// Extra DWORD
		/// </summary>
		Immediate32 = 0,

		/// <summary>
		/// 2 Extra DWORDs (HI32:LO32)
		/// </summary>
		Immediate64 = 1,

		/// <summary>
		/// Extra operand
		/// </summary>
		Relative = 2,

		/// <summary>
		/// Extra DWORD followed by extra operand
		/// </summary>
		Immediate32PlusRelative = 3, 
 
		/// <summary>
		/// 2 Extra DWORDS (HI32:LO32) followed by extra operand
		/// </summary>
		Immediate64PlusRelative = 4
	}
}