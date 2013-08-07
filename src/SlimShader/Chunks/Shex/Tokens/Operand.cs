using System;
using SlimShader.Chunks.Common;
using SlimShader.Util;

namespace SlimShader.Chunks.Shex.Tokens
{
	/// <summary>
	/// Instruction Operand Format (OperandToken0)
	///
	/// [01:00] D3D10_SB_OPERAND_NUM_COMPONENTS
	/// [11:02] Component Selection
	///         if([01:00] == D3D10_SB_OPERAND_0_COMPONENT)
	///              [11:02] = Ignored, 0
	///         else if([01:00] == D3D10_SB_OPERAND_1_COMPONENT
	///              [11:02] = Ignored, 0
	///         else if([01:00] == D3D10_SB_OPERAND_4_COMPONENT
	///         {
	///              [03:02] = D3D10_SB_OPERAND_4_COMPONENT_SELECTION_MODE
	///              if([03:02] == D3D10_SB_OPERAND_4_COMPONENT_MASK_MODE)
	///              {
	///                  [07:04] = D3D10_SB_OPERAND_4_COMPONENT_MASK
	///                  [11:08] = Ignored, 0
	///              }
	///              else if([03:02] == D3D10_SB_OPERAND_4_COMPONENT_SWIZZLE_MODE)
	///              {
	///                  [11:04] = D3D10_SB_4_COMPONENT_SWIZZLE
	///              }
	///              else if([03:02] == D3D10_SB_OPERAND_4_COMPONENT_SELECT_1_MODE)
	///              {
	///                  [05:04] = D3D10_SB_4_COMPONENT_NAME
	///                  [11:06] = Ignored, 0
	///              }
	///         }
	///         else if([01:00] == D3D10_SB_OPERAND_N_COMPONENT)
	///         {
	///              Currently not defined.
	///         }
	/// [19:12] D3D10_SB_OPERAND_TYPE
	/// [21:20] D3D10_SB_OPERAND_INDEX_DIMENSION:
	///            Number of dimensions in the register
	///            file (NOT the # of dimensions in the
	///            individual register or memory
	///            resource being referenced).
	/// [24:22] if( [21:20] >= D3D10_SB_OPERAND_INDEX_1D )
	///             D3D10_SB_OPERAND_INDEX_REPRESENTATION for first operand index
	///         else
	///             Ignored, 0
	/// [27:25] if( [21:20] >= D3D10_SB_OPERAND_INDEX_2D )
	///             D3D10_SB_OPERAND_INDEX_REPRESENTATION for second operand index
	///         else
	///             Ignored, 0
	/// [30:28] if( [21:20] == D3D10_SB_OPERAND_INDEX_3D )
	///             D3D10_SB_OPERAND_INDEX_REPRESENTATION for third operand index
	///         else
	///             Ignored, 0
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.
	/// </summary>
	public class Operand
	{
		private readonly OpcodeType _parentType;

		public byte NumComponents { get; private set; }
		public Operand4ComponentSelectionMode SelectionMode { get; private set; }
		public ComponentMask ComponentMask { get; private set; }
		public Operand4ComponentName[] Swizzles { get; private set; }
		public OperandType OperandType { get; private set; }
		public OperandIndexDimension IndexDimension { get; private set; }
		public bool IsExtended { get; private set; }
		public OperandModifier Modifier { get; private set; }
		public OperandIndex[] Indices { get; private set; }
		public Number4 ImmediateValues { get; private set; }

		public Operand(OpcodeType parentType)
		{
			_parentType = parentType;
			Swizzles = new[]
			{
				Operand4ComponentName.X,
				Operand4ComponentName.Y,
				Operand4ComponentName.Z,
				Operand4ComponentName.W
			};
			Indices = new OperandIndex[3];
		}

		public static Operand Parse(BytecodeReader reader, OpcodeType parentType)
		{
			uint token0 = reader.ReadUInt32();
		    if (token0 == 0)
		        return null;

			var operand = new Operand(parentType);

			var numComponents = token0.DecodeValue<OperandNumComponents>(0, 1);
			switch (numComponents)
			{
				case OperandNumComponents.Zero:
					{
						operand.NumComponents = 0;
						break;
					}
				case OperandNumComponents.One:
					{
						operand.NumComponents = 1;
						break;
					}
				case OperandNumComponents.Four:
					{
						operand.NumComponents = 4;
						operand.SelectionMode = token0.DecodeValue<Operand4ComponentSelectionMode>(2, 3);
						switch (operand.SelectionMode)
						{
							case Operand4ComponentSelectionMode.Mask:
								{
									operand.ComponentMask = token0.DecodeValue<ComponentMask>(4, 7);
									break;
								}
							case Operand4ComponentSelectionMode.Swizzle:
								{
									var swizzle = token0.DecodeValue(4, 11);
									Func<uint, byte, Operand4ComponentName> swizzleDecoder = (s, i) =>
										(Operand4ComponentName) ((s >> (i * 2)) & 3);
									operand.Swizzles[0] = swizzleDecoder(swizzle, 0);
									operand.Swizzles[1] = swizzleDecoder(swizzle, 1);
									operand.Swizzles[2] = swizzleDecoder(swizzle, 2);
									operand.Swizzles[3] = swizzleDecoder(swizzle, 3);
									break;
								}
							case Operand4ComponentSelectionMode.Select1:
								{
									var swizzle = token0.DecodeValue<Operand4ComponentName>(4, 5);
									operand.Swizzles[0] = operand.Swizzles[1] = operand.Swizzles[2] = operand.Swizzles[3] = swizzle;
									break;
								}
							default:
								{
									throw new ParseException("Unrecognized selection method: " + operand.SelectionMode);
								}
						}
						break;
					}
				case OperandNumComponents.N:
					{
						throw new ParseException("OperandNumComponents.N is not currently supported.");
					}
			}

			operand.OperandType = token0.DecodeValue<OperandType>(12, 19);
			operand.IndexDimension = token0.DecodeValue<OperandIndexDimension>(20, 21);

			operand.IsExtended = token0.DecodeValue(31, 31) == 1;
			if (operand.IsExtended)
				ReadExtendedOperand(operand, reader);

			Func<uint, byte, OperandIndexRepresentation> indexRepresentationDecoder = (t, i) =>
				(OperandIndexRepresentation) t.DecodeValue((byte) (22 + (i * 3)), (byte) (22 + (i * 3) + 2));

			for (byte i = 0; i < (byte) operand.IndexDimension; i++)
			{
				operand.Indices[i] = new OperandIndex();

				var indexRepresentation = indexRepresentationDecoder(token0, i);
				operand.Indices[i].Representation = indexRepresentation;

				switch (indexRepresentation)
				{
					case OperandIndexRepresentation.Immediate32:
						operand.Indices[i].Value = reader.ReadUInt32();
						break;
					case OperandIndexRepresentation.Immediate64:
						operand.Indices[i].Value = reader.ReadUInt64();
						goto default;
					case OperandIndexRepresentation.Relative:
						operand.Indices[i].Register = Parse(reader, parentType);
						break;
					case OperandIndexRepresentation.Immediate32PlusRelative:
						operand.Indices[i].Value = reader.ReadUInt32();
						goto case OperandIndexRepresentation.Relative;
					case OperandIndexRepresentation.Immediate64PlusRelative:
						operand.Indices[i].Value = reader.ReadUInt64();
						goto case OperandIndexRepresentation.Relative;
					default:
						throw new ParseException("Unrecognised index representation: " + indexRepresentation);
				}
			}

			var numberType = parentType.GetNumberType();
			switch (operand.OperandType)
			{
				case OperandType.Immediate32:
				{
					var immediateValues = new Number4();
					for (var i = 0; i < operand.NumComponents; i++)
						immediateValues.SetNumber(i, Number.Parse(reader));
					operand.ImmediateValues = immediateValues;
					break;
				}
				case OperandType.Immediate64:
				{
					var immediateValues = new Number4();
					for (var i = 0; i < operand.NumComponents; i++)
						immediateValues.SetDouble(i, reader.ReadDouble());
					operand.ImmediateValues = immediateValues;
					break;
				}
			}

			return operand;
		}

		/// <summary>
		/// Extended Instruction Operand Format (OperandToken1)
		///
		/// If bit31 of an operand token is set, the
		/// operand has additional data in a second DWORD
		/// directly following OperandToken0.  Other tokens
		/// expected for the operand, such as immmediate
		/// values or relative address operands (full
		/// operands in themselves) always follow
		/// OperandToken0 AND OperandToken1..n (extended
		/// operand tokens, if present).
		///
		/// [05:00] D3D10_SB_EXTENDED_OPERAND_TYPE
		/// [30:06] if([05:00] == D3D10_SB_EXTENDED_OPERAND_MODIFIER)
		///         {
		///              [13:06] D3D10_SB_OPERAND_MODIFIER
		///              [30:14] Ignored, 0.
		///         }
		///         else
		///         {
		///              [30:06] Ignored, 0.
		///         }
		/// [31]    0 normally. 1 if second order extended operand definition,
		///         meaning next DWORD contains yet ANOTHER extended operand
		///         description. Currently no second order extensions defined.
		///         This would be useful if a particular extended operand does
		///         not have enough space to store the required information in
		///         a single token and so is extended further.
		/// </summary>
		private static void ReadExtendedOperand(Operand operand, BytecodeReader reader)
		{
			uint token1 = reader.ReadUInt32();

			switch (token1.DecodeValue<ExtendedOperandType>(0, 5))
			{
				case ExtendedOperandType.Modifier:
					operand.Modifier = token1.DecodeValue<OperandModifier>(6, 13);
					break;
			}
		}

		public override string ToString()
		{
			switch (OperandType)
			{
				case OperandType.Immediate32:
				case OperandType.Immediate64:
					{
						string result = (OperandType == OperandType.Immediate64) ? "d(" : "l(";
						bool addSpaces = _parentType != OpcodeType.Mov && _parentType != OpcodeType.MovC && _parentType != OpcodeType.StoreStructured;
						for (int i = 0; i < NumComponents; i++)
						{
							result += (OperandType == OperandType.Immediate64)
								? ImmediateValues.GetDouble(i).ToString()
								: ImmediateValues.GetNumber(i).ToString(_parentType.GetNumberType());

							if (i < NumComponents - 1)
							{
								result += ",";
								if (addSpaces)
									result += " ";
							}
						}
						result += ")";
						return result;
					}
				case OperandType.Null:
					{
						return OperandType.GetDescription();
					}
				default:
					{
						string index = string.Empty;
						switch (IndexDimension)
						{
							case OperandIndexDimension._0D:
								break;
							case OperandIndexDimension._1D:
								index = (Indices[0].Representation == OperandIndexRepresentation.Relative
									|| Indices[0].Representation == OperandIndexRepresentation.Immediate32PlusRelative
									|| !OperandType.RequiresRegisterNumberFor1DIndex())
									? string.Format("[{0}]", Indices[0])
									: Indices[0].ToString();
								break;
							case OperandIndexDimension._2D:
								index = (Indices[0].Representation == OperandIndexRepresentation.Relative
									|| Indices[0].Representation == OperandIndexRepresentation.Immediate32PlusRelative
									|| !OperandType.RequiresRegisterNumberFor2DIndex())
									? string.Format("[{0}][{1}]", Indices[0], Indices[1])
									: string.Format("{0}[{1}]", Indices[0], Indices[1]);
								break;
							case OperandIndexDimension._3D:
								break;
						}

						string components = string.Empty;
						if (_parentType != OpcodeType.DclConstantBuffer)
						{
							switch (SelectionMode)
							{
								case Operand4ComponentSelectionMode.Mask:
									components = ComponentMask.GetDescription();
									break;
								case Operand4ComponentSelectionMode.Swizzle:
									components = Swizzles[0].GetDescription()
										+ Swizzles[1].GetDescription()
										+ Swizzles[2].GetDescription()
										+ Swizzles[3].GetDescription();
									break;
								case Operand4ComponentSelectionMode.Select1:
									components = Swizzles[0].GetDescription();
									break;
								default:
									throw new InvalidOperationException("Unrecognised selection mode: " + SelectionMode);
							}
							if (!string.IsNullOrEmpty(components))
								components = "." + components;
						}

						return Modifier.Wrap(string.Format("{0}{1}{2}", OperandType.GetDescription(), index, components));
					}
			}
		}
	}
}