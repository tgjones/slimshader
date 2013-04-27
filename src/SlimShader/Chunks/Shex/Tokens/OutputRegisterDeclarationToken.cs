using SlimShader.Util;

namespace SlimShader.Chunks.Shex.Tokens
{
	/// <summary>
	/// Output Register Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_OUTPUT
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 1 operand:
	/// (1) Operand, starting with OperandToken0, defining which
	///     o# register (D3D10_SB_OPERAND_TYPE_OUTPUT) is being declared,
	///     including writemask.
	///     (in Pixel Shader, output can also be one of 
	///     D3D10_SB_OPERAND_TYPE_OUTPUT_DEPTH,
	///     D3D11_SB_OPERAND_TYPE_OUTPUT_DEPTH_GREATER_EQUAL, or
	///     D3D11_SB_OPERAND_TYPE_OUTPUT_DEPTH_LESS_EQUAL )
	///
	/// -------
	/// 
	/// Output Register Declaration w/System Interpreted Value
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_OUTPUT_SIV
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 2 operands:
	/// (1) an operand, starting with OperandToken0, defining which
	///     o# register (D3D10_SB_OPERAND_TYPE_OUTPUT) is being declared,
	///     including writemask.
	/// (2) a System Interpreted Name token (NameToken)
	///
	/// -------
	///
	/// Output Register Declaration w/System Generated Value
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_OUTPUT_SGV
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 2 operands:
	/// (1) an operand, starting with OperandToken0, defining which
	///     o# register (D3D10_SB_OPERAND_TYPE_OUTPUT) is being declared,
	///     including writemask.
	/// (2) a System Generated Name token (NameToken)
	/// </summary>
	public class OutputRegisterDeclarationToken : DeclarationToken
	{
		/// <summary>
		/// Only applicable for SGV and SIV declarations.
		/// </summary>
		public SystemValueName SystemValueName { get; internal set; }

		public static OutputRegisterDeclarationToken Parse(BytecodeReader reader)
		{
			uint token0 = reader.ReadUInt32();
			var opcodeType = token0.DecodeValue<OpcodeType>(0, 10);

			var result = new OutputRegisterDeclarationToken
			{
				Operand = Operand.Parse(reader, opcodeType)
			};

			switch (opcodeType)
			{
				case OpcodeType.DclOutputSgv:
				case OpcodeType.DclOutputSiv:
					result.SystemValueName = NameToken.Parse(reader);
					break;
			}

			return result;
		}

		public override string ToString()
		{
			string result = string.Format("{0} {1}", TypeDescription, Operand);

			if (Header.OpcodeType == OpcodeType.DclOutputSgv || Header.OpcodeType == OpcodeType.DclOutputSiv)
				result += ", " + SystemValueName.GetDescription();

			return result;
		}
	}
}