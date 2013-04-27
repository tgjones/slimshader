using System.Collections.Generic;
using System.Linq;
using SlimShader.Util;

namespace SlimShader.Chunks.Shex.Tokens
{
	/// <summary>
	/// Interface Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_INTERFACE
	/// [11]    1 if the interface is indexed dynamically, 0 otherwise.
	/// [23:12] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  If it is extended, then
	///         it contains the actual instruction length in DWORDs, since
	///         it may not fit into 7 bits if enough types are used.
	///
	/// OpcodeToken0 is followed by a DWORD that represents the interface
	/// identifier. Next is a DWORD that gives the expected function table
	/// length. Then another DWORD (OpcodeToken3) with the following layout:
	///
	/// [15:00] TableLength, the number of types that implement this interface
	/// [31:16] ArrayLength, the number of interfaces that are defined in this array.
	///
	/// This is followed by TableLength DWORDs which are function table
	/// identifiers, representing possible tables for a given interface.
	/// </summary>
	public class InterfaceDeclarationToken : DeclarationToken
	{
		/// <summary>
		/// Returns true if the interface is indexed dynamically, otherwise false.
		/// </summary>
		public bool DynamicallyIndexed { get; private set; }

		/// <summary>
		/// Gets the interface identifier.
		/// </summary>
		public uint Identifier { get; private set; }

		/// <summary>
		/// Gets the expected function table length.
		/// </summary>
		public uint ExpectedFunctionTableLength { get; private set; }

		/// <summary>
		/// Gets the number of types that implement this interface.
		/// </summary>
		public ushort TableLength { get; private set; }

		/// <summary>
		/// Gets the number of interfaces that are defined in this array.
		/// </summary>
		public ushort ArrayLength { get; private set; }

		/// <summary>
		/// Gets the possible tables for a given interface.
		/// </summary>
		public List<uint> FunctionTableIdentifiers { get; private set; }

		public InterfaceDeclarationToken()
		{
			FunctionTableIdentifiers = new List<uint>();
		}

		public static InterfaceDeclarationToken Parse(BytecodeReader reader)
		{
			uint token0 = reader.ReadUInt32();
			var result = new InterfaceDeclarationToken
			{
				DynamicallyIndexed = (token0.DecodeValue(11, 11) == 1),
				Identifier = reader.ReadUInt32(),
				ExpectedFunctionTableLength = reader.ReadUInt32()
			};

			uint token3 = reader.ReadUInt32();
			result.TableLength = token3.DecodeValue<ushort>(00, 15);
			result.ArrayLength = token3.DecodeValue<ushort>(16, 31);

			for (int i = 0; i < result.TableLength; i++)
				result.FunctionTableIdentifiers.Add(reader.ReadUInt32());

			return result;
		}

		public override string ToString()
		{
			return string.Format("{0}{1} fp{2}[{3}][{4}] = {{{5}}}",
				TypeDescription,
				(DynamicallyIndexed) ? "_dynamicindexed" : string.Empty,
				Identifier, ArrayLength,
				ExpectedFunctionTableLength, string.Join(", ", FunctionTableIdentifiers.Select(x => "ft" + x)));
		}
	}
}