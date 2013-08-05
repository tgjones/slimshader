using System.Collections.Generic;
using SlimShader.Chunks.Common;
using SlimShader.Util;

namespace SlimShader.Chunks.Shex.Tokens
{
	/// <summary>
	/// Instruction Token
	///
	/// Normal instructions:
	/// [10:00] OpcodeType
	/// [12:11] ResInfo return type
	/// [13:13] Saturate
	/// [17:14] Ignored, 0
	/// [18:18] Boolean test for conditional instructions such as if (if_z or if_nz)
	/// [22:19] Precise value mask
	//          This is part of the opcode specific control range.
	//          It's 1 bit per-channel of the output, for instructions with multiple
	//          output operands, it applies to that component in each operand. This
	//          uses the components defined in D3D10_SB_4_COMPONENT_NAME.
	/// [23:23] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.
	/// 
	/// OpcodeType == Sync:
	/// [11:11] ThreadsInGroup
	/// [12:12] SharedMemory
	/// [13:13] UavGroup
	/// [14:14] UavGlobal
	///
	/// OpcodeToken0 is followed by 1 or more operands.
	/// </summary>
	public class InstructionToken : OpcodeToken
	{
		/// <summary>
		/// Return type for the resinfo instruction.
 		/// </summary>
		public ResInfoReturnType ResInfoReturnType { get; private set; }

		public bool Saturate { get; private set; }
		public InstructionTestBoolean TestBoolean { get; private set; }
		public ComponentMask PreciseValueMask { get; private set; }
		public SyncFlags SyncFlags { get; private set; }
		public List<InstructionTokenExtendedType> ExtendedTypes { get; private set; }
		public sbyte[] SampleOffsets { get; private set; }
		public ResourceDimension ResourceTarget { get; set; }
		public byte ResourceStride { get; private set; }
		public ResourceReturnType[] ResourceReturnTypes { get; private set; }

		/// <summary>
		/// Not stored in the shader bytecode; derived from a post-parsing pass.
		/// </summary>
		public int LinkedInstructionOffset { get; internal set; }

		/// <summary>
		/// Gets the function index to call in the function table specified for the given interface.
		/// Only relevant for OpcodeType.InterfaceCall instructions.
		/// </summary>
		public uint FunctionIndex { get; private set; }

		public List<Operand> Operands { get; private set; }

		public InstructionToken()
		{
			ExtendedTypes = new List<InstructionTokenExtendedType>();
			SampleOffsets = new sbyte[3];
			ResourceReturnTypes = new ResourceReturnType[4];
			Operands = new List<Operand>();
		}

		public static InstructionToken Parse(BytecodeReader reader, OpcodeHeader header)
		{
			var instructionToken = new InstructionToken();

			// Advance to next token.
			var instructionEnd = reader.CurrentPosition + (header.Length * sizeof(uint));
			var token0 = reader.ReadUInt32();

			if (header.OpcodeType == OpcodeType.Sync)
			{
				instructionToken.SyncFlags = token0.DecodeValue<SyncFlags>(11, 14);
			}
			else
			{
				instructionToken.ResInfoReturnType = token0.DecodeValue<ResInfoReturnType>(11, 12);
				instructionToken.Saturate = (token0.DecodeValue(13, 13) == 1);
				instructionToken.TestBoolean = token0.DecodeValue<InstructionTestBoolean>(18, 18);
				instructionToken.PreciseValueMask = token0.DecodeValue<ComponentMask>(19, 22);
			}

			bool extended = header.IsExtended;
			while (extended)
			{
				uint extendedToken = reader.ReadUInt32();
				var extendedType = extendedToken.DecodeValue<InstructionTokenExtendedType>(0, 5);
				instructionToken.ExtendedTypes.Add(extendedType);
				extended = (extendedToken.DecodeValue(31, 31) == 1);

				switch (extendedType)
				{
					case InstructionTokenExtendedType.SampleControls:
						instructionToken.SampleOffsets[0] = extendedToken.DecodeSigned4BitValue(09, 12);
						instructionToken.SampleOffsets[1] = extendedToken.DecodeSigned4BitValue(13, 16);
						instructionToken.SampleOffsets[2] = extendedToken.DecodeSigned4BitValue(17, 20);
						break;
					case InstructionTokenExtendedType.ResourceDim:
						instructionToken.ResourceTarget = extendedToken.DecodeValue<ResourceDimension>(6, 10);
						instructionToken.ResourceStride = extendedToken.DecodeValue<byte>(11, 15);
						break;
					case InstructionTokenExtendedType.ResourceReturnType:
						instructionToken.ResourceReturnTypes[0] = extendedToken.DecodeValue<ResourceReturnType>(06, 09);
						instructionToken.ResourceReturnTypes[1] = extendedToken.DecodeValue<ResourceReturnType>(10, 13);
						instructionToken.ResourceReturnTypes[2] = extendedToken.DecodeValue<ResourceReturnType>(14, 17);
						instructionToken.ResourceReturnTypes[3] = extendedToken.DecodeValue<ResourceReturnType>(18, 21);
						break;
					default:
						throw new ParseException("Unrecognised extended type: " + extendedType);
				}
			}

			if (header.OpcodeType == OpcodeType.InterfaceCall)
			{
				// Interface call
				//
				// OpcodeToken0:
				//
				// [10:00] D3D10_SB_OPCODE_INTERFACE_CALL
				// [23:11] Ignored, 0
				// [30:24] Instruction length in DWORDs including the opcode token.
				// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
				//         contains extended operand description.  If it is extended, then
				//         it contains the actual instruction length in DWORDs, since
				//         it may not fit into 7 bits if enough types are used.
				//
				// OpcodeToken0 is followed by a DWORD that gives the function index to
				// call in the function table specified for the given interface. 
				// Next is the interface operand.
				instructionToken.FunctionIndex = reader.ReadUInt32();
			}

		    while (reader.CurrentPosition < instructionEnd)
		    {
		        var operand = Operand.Parse(reader, header.OpcodeType);
                if (operand != null)
		            instructionToken.Operands.Add(operand);
		    }

		    return instructionToken;
		}

		public override string ToString()
		{
			string result = TypeDescription;

			if (Header.OpcodeType == OpcodeType.Sync)
				result += SyncFlags.GetDescription();

			if (ExtendedTypes.Contains(InstructionTokenExtendedType.SampleControls))
				result += "_aoffimmi";

			if (ExtendedTypes.Contains(InstructionTokenExtendedType.ResourceDim))
				result += "_indexable";

			if (ExtendedTypes.Contains(InstructionTokenExtendedType.SampleControls))
				result += string.Format("({0},{1},{2})", SampleOffsets[0], SampleOffsets[1], SampleOffsets[2]);

			if (ExtendedTypes.Contains(InstructionTokenExtendedType.ResourceDim))
			{
				result += string.Format("({0}", ResourceTarget.GetDescription());
				if (ResourceStride != 0)
					result += string.Format(", stride={0}", ResourceStride);
				result += ")";
			}

			if (ExtendedTypes.Contains(InstructionTokenExtendedType.ResourceReturnType))
				result += string.Format("({0},{1},{2},{3})",
					ResourceReturnTypes[0].GetDescription(),
					ResourceReturnTypes[1].GetDescription(),
					ResourceReturnTypes[2].GetDescription(),
					ResourceReturnTypes[3].GetDescription());

			if (Header.OpcodeType.IsConditionalInstruction())
				result += "_" + TestBoolean.GetDescription();

			if (Saturate)
				result += "_sat";
			result += " ";

			if (Header.OpcodeType == OpcodeType.InterfaceCall)
			{
				result += string.Format("fp{0}[{1}][{2}]", 
					Operands[0].Indices[0].Value,
					Operands[0].Indices[1],
					FunctionIndex);
			}
			else
			{
				for (int i = 0; i < Operands.Count; i++)
				{
				    var operandString = Operands[i].ToString();
                    if (i > 0 && !string.IsNullOrEmpty(operandString))
                        result += ", ";
                    result += operandString;
				}
			}

			return result;
		}
	}
}