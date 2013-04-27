using SlimShader.Util;

namespace SlimShader.Chunks.Shex.Tokens
{
	/// <summary>
	/// Structured Thread Group Shared Memory Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D11_SB_OPCODE_DCL_THREAD_GROUP_SHARED_MEMORY_STRUCTURED
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 3 operands:
	/// (1) an operand, starting with OperandToken0, defining which
	///     g# register (D3D11_SB_OPERAND_TYPE_THREAD_GROUP_SHARED_MEMORY) is 
	///     being declared.
	/// (2) a DWORD indicating UINT32 struct byte stride
	/// (3) a DWORD indicating UINT32 struct count
	/// </summary>
	public class StructuredThreadGroupSharedMemoryDeclarationToken : DeclarationToken
	{
		public uint StructByteStride { get; private set; }
		public uint StructCount { get; private set; }

		public static StructuredThreadGroupSharedMemoryDeclarationToken Parse(BytecodeReader reader)
		{
			var token0 = reader.ReadUInt32();
			return new StructuredThreadGroupSharedMemoryDeclarationToken
			{
				Operand = Operand.Parse(reader, token0.DecodeValue<OpcodeType>(0, 10)),
				StructByteStride = reader.ReadUInt32(),
				StructCount = reader.ReadUInt32()
			};
		}

		public override string ToString()
		{
			return string.Format("{0} {1}, {2}, {3}", TypeDescription, Operand, StructByteStride, StructCount);
		}
	}
}