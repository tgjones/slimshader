using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimShader.IO;
using SlimShader.Shader.Tokens;
using SlimShader.Util;

namespace SlimShader.Shader
{
	public class ShaderProgramChunk : DxbcChunk
	{
		public ShaderVersion Version { get; private set; }
		public uint Length { get; private set; }
		public List<OpcodeToken> Tokens { get; private set; }

		public ShaderProgramChunk()
		{
			Tokens = new List<OpcodeToken>();
		}

		public static ShaderProgramChunk Parse(BytecodeReader reader)
		{
			var program = new ShaderProgramChunk
			{
				Version = ShaderVersion.Parse(reader),

				// Length Token (LenTok)
				// Always follows VerTok
				// [31:00] Unsigned integer count of number of DWORDs in program code, including version and length tokens.
				// So the minimum value is 0x00000002 (if an empty program is ever valid).
				Length = reader.ReadUInt32()
			};

			while (!reader.EndOfBuffer)
			{
				// Opcode Format (OpcodeToken0)
				//
				// [10:00] D3D10_SB_OPCODE_TYPE
				// if( [10:00] == D3D10_SB_OPCODE_CUSTOMDATA )
				// {
				//    Token starts a custom-data block.  See "Custom-Data Block Format".
				// }
				// else // standard opcode token
				// {
				//    [23:11] Opcode-Specific Controls
				//    [30:24] Instruction length in DWORDs including the opcode token.
				//    [31]    0 normally. 1 if extended operand definition, meaning next DWORD
				//            contains extended opcode token.
				// }
				var opcodeHeaderReader = reader.CopyAtCurrentPosition();
				var opcodeToken0 = opcodeHeaderReader.ReadUInt32();
				var opcodeHeader = new OpcodeHeader
				{
					OpcodeType = opcodeToken0.DecodeValue<OpcodeType>(0, 10),
					Length = opcodeToken0.DecodeValue(24, 30),
					IsExtended = (opcodeToken0.DecodeValue(31, 31) == 1)
				};

				OpcodeToken opcodeToken;
				if (opcodeHeader.OpcodeType == OpcodeType.CustomData)
				{
					opcodeToken = CustomDataToken.Parse(reader, opcodeToken0);
				}
				else if (opcodeHeader.OpcodeType.IsDeclaration())
				{
					opcodeToken = DeclarationToken.Parse(reader, opcodeHeader.OpcodeType);
				}
				else // Not custom data or declaration, so must be instruction.
				{
					opcodeToken = InstructionToken.Parse(reader, opcodeHeader);
				}

				opcodeToken.Header = opcodeHeader;
				program.Tokens.Add(opcodeToken);
			}

			return program;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.AppendLine(string.Format("{0}_{1}_{2}",
				Version.ProgramType.GetDescription(),
				Version.MajorVersion,
				Version.MinorVersion));

			int indent = 0;
			foreach (var token in Tokens)
			{
				if (token.Header.OpcodeType.IsNestedSectionEnd())
					indent -= 2;
				sb.AppendLine(string.Join(string.Empty, Enumerable.Repeat(" ", indent)) + token);
				if (token.Header.OpcodeType.IsNestedSectionStart())
					indent += 2;
			}

			return sb.ToString();
		}
	}
}