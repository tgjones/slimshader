using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SlimShader.Chunks.Common;
using SlimShader.Chunks.Shex.Tokens;
using SlimShader.Util;

namespace SlimShader.Chunks.Shex
{
	public class ShaderProgramChunk : BytecodeChunk
	{
		public ShaderVersion Version { get; private set; }
		public uint Length { get; private set; }
		public List<OpcodeToken> Tokens { get; private set; }

		public IEnumerable<DeclarationToken> DeclarationTokens
		{
			get { return Tokens.OfType<DeclarationToken>(); }
		}

		public IEnumerable<InstructionToken> InstructionTokens
		{
			get { return Tokens.OfType<InstructionToken>(); }
		}

		public RegisterCounts RegisterCounts
		{
			get { return new RegisterCounts(DeclarationTokens); }
		}

		public ShaderProgramChunk()
		{
			Tokens = new List<OpcodeToken>();
		}

		public static ShaderProgramChunk Parse(BytecodeReader reader)
		{
			var program = new ShaderProgramChunk
			{
				Version = ShaderVersion.ParseShex(reader),

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

			program.LinkControlFlowInstructions();

			return program;
		}

		private void LinkControlFlowInstructions()
		{
			var stack = new Stack<int>();
			var breakTokenIndices = new List<int>();
			for (int instructionIndex = 0; instructionIndex < Tokens.Count; instructionIndex++)
			{
				var token = Tokens[instructionIndex] as InstructionToken;
				if (token == null)
					continue;

				switch (token.Header.OpcodeType)
				{
					case OpcodeType.If:
					case OpcodeType.Switch:
					case OpcodeType.Loop:
						stack.Push(instructionIndex);
						break;
					case OpcodeType.Break :
					case OpcodeType.BreakC :
						breakTokenIndices.Add(instructionIndex);
						break;
					case OpcodeType.EndSwitch:
					case OpcodeType.EndIf:
					case OpcodeType.EndLoop:
						int v = stack.Pop();

						// Link End[If/Switch/Loop] to [If/Switch/Loop]
						token.LinkedInstructionOffset = v - instructionIndex;

						// Link [If/Switch/Loop] to End[If/Switch/Loop]
						((InstructionToken) Tokens[v]).LinkedInstructionOffset = instructionIndex - v;

						if (token.Header.OpcodeType != OpcodeType.EndIf)
						{
							// Link [Break/BreakC] to End[Switch/Loop]
							foreach (var breakTokenIndex in breakTokenIndices)
								((InstructionToken) Tokens[breakTokenIndex]).LinkedInstructionOffset = instructionIndex - breakTokenIndex;
							breakTokenIndices.Clear();
						}
						break;
				}
			}
			Debug.Assert(stack.Count == 0);
		}

		public void GetThreadGroupSize(out uint sizeX, out uint sizeY, out uint sizeZ)
		{
			var threadGroupDeclarationToken = Tokens.OfType<ThreadGroupDeclarationToken>().FirstOrDefault();
			if (threadGroupDeclarationToken == null)
			{
				sizeX = sizeY = sizeZ = 0;
				return;
			}
			sizeX = threadGroupDeclarationToken.Dimensions[0];
			sizeY = threadGroupDeclarationToken.Dimensions[1];
			sizeZ = threadGroupDeclarationToken.Dimensions[2];
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