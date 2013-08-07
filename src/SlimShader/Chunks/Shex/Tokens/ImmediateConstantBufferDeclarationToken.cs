using System;
using SlimShader.Util;

namespace SlimShader.Chunks.Shex.Tokens
{
	/// <summary>
	/// Immediate Constant Buffer Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_CUSTOMDATA
	/// [31:11] == D3D10_SB_CUSTOMDATA_DCL_IMMEDIATE_CONSTANT_BUFFER
	///
	/// OpcodeToken1 is followed by:
	/// (1) DWORD indicating length of declaration, including OpcodeToken0 and 1.
	///     This length must = 2(for OpcodeToken0 and 1) + a multiple of 4 
	///                                                    (# of immediate constants)
	/// (2) Sequence of 4-tuples of DWORDs defining the Immediate Constant Buffer.
	///     The number of 4-tuples is (length above - 1) / 4
	/// </summary>
	public class ImmediateConstantBufferDeclarationToken : ImmediateDeclarationToken
	{
		public Number[] Data { get; private set; }

		public static ImmediateConstantBufferDeclarationToken Parse(BytecodeReader reader)
		{
			var token0 = reader.ReadUInt32();
			var length = reader.ReadUInt32() - 2;

			var result = new ImmediateConstantBufferDeclarationToken
			{
				DeclarationLength = length,
				Data = new Number[length]
			};

			for (int i = 0; i < length; i++)
				result.Data[i] = Number.Parse(reader);

			return result;
		}

		public override string ToString()
		{
			string result = "dcl_immediateConstantBuffer { ";

			for (int i = 0; i < Data.Length; i += 4)
			{
				if (i > 0)
					result += "," + Environment.NewLine + new string(' ', 30);
				result += string.Format("{{ {0}, {1}, {2}, {3}}}",
					Data[i], Data[i + 1], Data[i + 2], Data[i + 3]);
			}
			
			result += " }";
			return result;
		}
	}
}