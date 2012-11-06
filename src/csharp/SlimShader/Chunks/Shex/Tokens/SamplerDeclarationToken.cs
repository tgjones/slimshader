using SlimShader.Util;

namespace SlimShader.Chunks.Shex.Tokens
{
	/// <summary>
	/// Sampler Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_SAMPLER
	/// [14:11] D3D10_SB_SAMPLER_MODE
	/// [23:15] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 1 operand:
	/// (1) Operand starting with OperandToken0, defining which sampler
	///     (D3D10_SB_OPERAND_TYPE_SAMPLER) register # is being declared.
	/// </summary>
	public class SamplerDeclarationToken : DeclarationToken
	{
		public SamplerMode SamplerMode { get; internal set; }

		public static SamplerDeclarationToken Parse(BytecodeReader reader)
		{
			var token0 = reader.ReadUInt32();
			var operand = Operand.Parse(reader, token0.DecodeValue<OpcodeType>(0, 10));
			return new SamplerDeclarationToken
			{
				SamplerMode = token0.DecodeValue<SamplerMode>(11, 14),
				Operand = operand
			};
		}

		public override string ToString()
		{
			return string.Format("{0} {1}, {2}", TypeDescription, Operand,
				SamplerMode.GetDescription());
		}
	}
}