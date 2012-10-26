using System.Collections.Generic;
using System.Linq;
using SlimShader.Util;

namespace SlimShader.Chunks.Shex.Tokens
{
	/// <summary>
	/// Interface function table Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_FUNCTION_TABLE
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  If it is extended, then
	///         it contains the actual instruction length in DWORDs, since
	///         it may not fit into 7 bits if enough functions are defined.
	///
	/// OpcodeToken0 is followed by a DWORD that represents the function table
	/// identifier and another DWORD (TableLength) that gives the number of
	/// functions in the table.
	///
	/// This is followed by TableLength DWORDs which are function body indices.
	/// </summary>
	public class FunctionTableDeclarationToken : DeclarationToken
	{
		/// <summary>
		/// Function table identifier
		/// </summary>
		public uint Identifier { get; private set; }

		/// <summary>
		/// Function body indices
		/// </summary>
		public List<uint> FunctionBodyIndices { get; private set; }

		public FunctionTableDeclarationToken()
		{
			FunctionBodyIndices = new List<uint>();
		}

		public static FunctionTableDeclarationToken Parse(BytecodeReader reader)
		{
			uint token0 = reader.ReadUInt32();
			var result = new FunctionTableDeclarationToken
			{
				Identifier = reader.ReadUInt32()
			};

			uint tableLength = reader.ReadUInt32();
			for (int i = 0; i < tableLength; i++)
				result.FunctionBodyIndices.Add(reader.ReadUInt32());

			return result;
		}

		public override string ToString()
		{
			return string.Format("{0} ft{1} = {{{2}}}", TypeDescription, Identifier,
				string.Join(", ", FunctionBodyIndices.Select(x => "fb" + x)));
		}
	}
}