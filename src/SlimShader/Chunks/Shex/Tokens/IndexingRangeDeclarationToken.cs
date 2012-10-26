using SlimShader.Util;

namespace SlimShader.Chunks.Shex.Tokens
{
	/// <summary>
	/// Input or Output Register Indexing Range Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_INDEX_RANGE
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 2 operands:
	/// (1) an operand, starting with OperandToken0, defining which
	///     input (v#) or output (o#) register is having its array indexing range
	///     declared, including writemask.  For Geometry Shader inputs, 
	///     it is assumed that the vertex axis is always fully indexable,
	///     and 0 must be specified as the vertex# in this declaration, so that 
	///     only the a range of attributes are having their index range defined.
	///     
	/// (2) a DWORD representing the count of registers starting from the one
	///     indicated in (1).
	/// </summary>
	public class IndexingRangeDeclarationToken : DeclarationToken
	{
		/// <summary>
		/// Represents the count of registers starting the from the one indicated in Operand.
		/// </summary>
		public uint RegisterCount { get; internal set; }

		public static IndexingRangeDeclarationToken Parse(BytecodeReader reader)
		{
			var token0 = reader.ReadUInt32();
			var operand = Operand.Parse(reader, token0.DecodeValue<OpcodeType>(0, 10));
			var registerCount = reader.ReadUInt32();
			return new IndexingRangeDeclarationToken
			{
				Operand = operand,
				RegisterCount = registerCount
			};
		}

		public override string ToString()
		{
			return string.Format("{0} {1} {2}", TypeDescription, Operand, RegisterCount);
		}
	}
}