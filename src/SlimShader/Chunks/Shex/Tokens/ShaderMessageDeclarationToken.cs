using System;
using System.Collections.Generic;
using System.Text;
using SlimShader.Util;

namespace SlimShader.Chunks.Shex.Tokens
{
	/// <summary>
	/// Shader Message Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_CUSTOMDATA
	/// [31:11] == D3D11_SB_CUSTOMDATA_SHADER_MESSAGE
	///
	/// OpcodeToken0 is followed by:
	/// (1) DWORD indicating length of declaration, including OpcodeToken0.
	/// (2) DWORD indicating the info queue message ID.
	/// (3) D3D11_SB_SHADER_MESSAGE_FORMAT indicating the convention for formatting the message.
	/// (4) DWORD indicating the number of characters in the string without the terminator.
	/// (5) DWORD indicating the number of operands.
	/// (6) DWORD indicating length of operands.
	/// (7) Encoded operands.
	/// (8) String with trailing zero, padded to a multiple of DWORDs.
	///     The string is in the given format and the operands given should
	///     be used for argument substitutions when formatting.
	/// </summary>
	public class ShaderMessageDeclarationToken : ImmediateDeclarationToken
	{
		/// <summary>
		/// Indicates the info queue message ID.
		/// </summary>
		public uint InfoQueueMessageID { get; private set; }

		/// <summary>
		/// Indicates the convention for formatting the message.
		/// </summary>
		public ShaderMessageFormat MessageFormat { get; private set; }

		/// <summary>
		/// DWORD indicating the number of characters in the string without the terminator.
		/// </summary>
		public uint NumCharacters { get; private set; }

		/// <summary>
		/// DWORD indicating the number of operands.
		/// </summary>
		public uint NumOperands { get; private set; }

		/// <summary>
		/// DWORD indicating length of operands.
		/// </summary>
		public uint OperandsLength { get; private set; }

		/// <summary>
		/// Operands, used as arguments to the format string.
		/// </summary>
		public List<Operand> Operands { get; private set; }

		/// <summary>
		/// String with trailing zero, padded to a multiple of DWORDs.
		/// The string is in the given format and the operands given should
		/// be used for argument substitutions when formatting.
		/// </summary>
		public string Format { get; private set; }

		public ShaderMessageDeclarationToken()
		{
			Operands = new List<Operand>();
		}

		public static ShaderMessageDeclarationToken Parse(BytecodeReader reader)
		{
			var token0 = reader.ReadUInt32();
			var length = reader.ReadUInt32() - 2;

			var result = new ShaderMessageDeclarationToken
			{
				DeclarationLength = length,
				InfoQueueMessageID = reader.ReadUInt32(),
				MessageFormat = (ShaderMessageFormat) reader.ReadUInt32(),
				NumCharacters = reader.ReadUInt32(),
				NumOperands = reader.ReadUInt32(),
				OperandsLength = reader.ReadUInt32()
			};

			for (int i = 0; i < result.NumOperands; i++)
				result.Operands.Add(Operand.Parse(reader, OpcodeType.CustomData));

			result.Format = reader.ReadString();

			// String is padded to a multiple of DWORDs.
			uint remainingBytes = (4 - ((result.NumCharacters + 1) % 4)) % 4;
			reader.ReadBytes((int) remainingBytes);

			return result;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			string command;
			switch (InfoQueueMessageID)
			{
				case 2097410 :
					command = "printf";
					break;
				case 2097411 :
					command = "errorf";
					break;
				default :
					throw new NotSupportedException("Unknown InfoQueueMessageID: " + InfoQueueMessageID);
			}
			sb.AppendFormat("{0} \"{1}\"", command, Format);
			foreach (var operand in Operands)
				sb.AppendFormat(", {0}", operand);

			return sb.ToString();
		}
	}
}