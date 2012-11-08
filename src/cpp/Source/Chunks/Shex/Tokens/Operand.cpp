#include "PCH.h"
#include "Operand.h"

#include <sstream>
#include "Decoder.h"
#include "ExtendedOperandType.h"
#include "OperandNumComponents.h"

using namespace std;
using namespace SlimShader;

Operand Operand::Parse(BytecodeReader& reader, OpcodeType parentType)
{
	auto token0 = reader.ReadUInt32();

	Operand operand(parentType);

	auto numComponents = DecodeValue<OperandNumComponents>(token0, 0, 1);
	switch (numComponents)
	{
	case OperandNumComponents::Zero:
		{
			operand._numComponents = 0;
			break;
		}
	case OperandNumComponents::One:
		{
			operand._numComponents = 1;
			break;
		}
	case OperandNumComponents::Four:
		{
			operand._numComponents = 4;
			operand._selectionMode = DecodeValue<Operand4ComponentSelectionMode>(token0, 2, 3);
			switch (operand._selectionMode)
			{
			case Operand4ComponentSelectionMode::Mask:
				{
					operand._componentMask = DecodeValue<ComponentMask>(token0, 4, 7);
					break;
				}
			case Operand4ComponentSelectionMode::Swizzle:
				{
					auto swizzle = DecodeValue(token0, 4, 11);
					auto swizzleDecoder = [](uint32_t s, uint8_t i)
					{
						return (Operand4ComponentName) ((s >> (i * 2)) & 3);
					};
					operand._swizzles[0] = swizzleDecoder(swizzle, 0);
					operand._swizzles[1] = swizzleDecoder(swizzle, 1);
					operand._swizzles[2] = swizzleDecoder(swizzle, 2);
					operand._swizzles[3] = swizzleDecoder(swizzle, 3);
					break;
				}
			case Operand4ComponentSelectionMode::Select1:
				{
					auto swizzle = DecodeValue<Operand4ComponentName>(token0, 4, 5);
					operand._swizzles[0] = operand._swizzles[1] = operand._swizzles[2] = operand._swizzles[3] = swizzle;
					break;
				}
			default:
				{
					throw runtime_error("Unrecognized selection method: " + to_string((int) operand._selectionMode));
				}
			}
			break;
		}
	case OperandNumComponents::N:
		{
			throw runtime_error("OperandNumComponents::N is not currently supported.");
		}
	}

	operand._operandType = DecodeValue<OperandType>(token0, 12, 19);
	operand._indexDimension = DecodeValue<OperandIndexDimension>(token0, 20, 21);

	const auto indexRepresentationDecoder = [](uint32_t t, uint8_t i)
	{
		return (OperandIndexRepresentation) DecodeValue(t, (uint8_t) (22 + (i * 3)), (uint8_t) (22 + (i * 3) + 2));
	};
	for (uint8_t i = 0; i < (uint8_t) operand._indexDimension; i++)
		operand._indexRepresentations[i] = indexRepresentationDecoder(token0, i);

	operand._isExtended = DecodeValue(token0, 31, 31) == 1;
	if (operand._isExtended)
		ReadExtendedOperand(operand, reader);

	for (uint8_t i = 0; i < (uint8_t) operand._indexDimension; i++)
	{
		operand._indices[i].Register = nullptr;
		operand._indices[i].Value = 0;
		switch (operand._indexRepresentations[i])
		{
		case OperandIndexRepresentation::Immediate32:
			operand._indices[i].Value = reader.ReadUInt32();
			break;
		case OperandIndexRepresentation::Immediate64:
			operand._indices[i].Value = reader.ReadUInt64();
			goto label_default;
		case OperandIndexRepresentation::Relative:
label_relative :
			operand._indices[i].Register = shared_ptr<Operand>(new Operand(Parse(reader, parentType)));
			break;
		case OperandIndexRepresentation::Immediate32PlusRelative:
			operand._indices[i].Value = reader.ReadUInt32();
			goto label_relative;
		case OperandIndexRepresentation::Immediate64PlusRelative:
			operand._indices[i].Value = reader.ReadUInt64();
			goto label_relative;
		default:
label_default :
			throw runtime_error("Invalid index representation");
		}
	}

	auto numberType = GetNumberType(parentType);
	switch (operand._operandType)
	{
	case OperandType::Immediate32:
		for (auto i = 0; i < operand._numComponents; i++)
			operand._immediateValues.Numbers[i] = Number::Parse(reader, numberType);
		break;
	case OperandType::Immediate64:
		for (auto i = 0; i < operand._numComponents; i++)
			operand._immediateValues.Doubles[i] = reader.ReadDouble();
		break;
	}

	return operand;
}

 Operand::Operand()
	 : _parentType((OpcodeType) -1), _modifier(OperandModifier::None), _componentMask(ComponentMask::None)
 {

 }

string Wrap(OperandModifier modifier, string& valueToWrap)
{
	switch (modifier)
	{
	case OperandModifier::None:
		return valueToWrap;
	case OperandModifier::Neg:
		return "-" + valueToWrap;
	case OperandModifier::Abs:
		return "|" + valueToWrap + "|";
	case OperandModifier::AbsNeg:
		return "-|" + valueToWrap + "|";
	default:
		throw runtime_error("Modifier not recognised");
	}
}

ostream& SlimShader::operator<<(ostream& out, const Operand& value)
{
	switch (value._operandType)
	{
	case OperandType::Immediate32:
	case OperandType::Immediate64:
		{
			out << ((value._operandType == OperandType::Immediate64) ? "d(" : "l(");
			bool addSpaces = value._parentType != OpcodeType::Mov && value._parentType != OpcodeType::MovC;
			for (int i = 0; i < value._numComponents; i++)
			{
				if (value._operandType == OperandType::Immediate64)
					out << value._immediateValues.Doubles[i];
				else
					out << value._immediateValues.Numbers[i];

				if (i < value._numComponents - 1)
				{
					out << ",";
					if (addSpaces)
						out << " ";
				}
			}
			out << ")";
			break;
		}
	case OperandType::Null:
		{
			out << ToString(value._operandType);
			break;
		}
	default:
		{
			stringstream indexStream;
			switch (value._indexDimension)
			{
			case OperandIndexDimension::_0D:
				break;
			case OperandIndexDimension::_1D:
				if (value._indexRepresentations[0] == OperandIndexRepresentation::Relative
					|| value._indexRepresentations[0] == OperandIndexRepresentation::Immediate32PlusRelative
					|| !RequiresRegisterNumberFor1DIndex(value._operandType))
					indexStream << "[" << value._indices[0] << "]";
				else
					indexStream << value._indices[0];
				break;
			case OperandIndexDimension::_2D:
				if (value._indexRepresentations[0] == OperandIndexRepresentation::Relative
					|| value._indexRepresentations[0] == OperandIndexRepresentation::Immediate32PlusRelative
					|| !RequiresRegisterNumberFor2DIndex(value._operandType))
					indexStream << "[" << value._indices[0] << "][" << value._indices[1] << "]";
				else
					indexStream << value._indices[0] << "[" << value._indices[1] << "]";
				break;
			case OperandIndexDimension::_3D:
				break;
			default:
				throw runtime_error("Unknown index dimension");
			}
			auto index = indexStream.str();

			string components;
			if (value._parentType != OpcodeType::DclConstantBuffer)
			{
				switch (value._selectionMode)
				{
				case Operand4ComponentSelectionMode::Mask:
					components = ToStringShex(value._componentMask);
					break;
				case Operand4ComponentSelectionMode::Swizzle:
					components = ToString(value._swizzles[0])
						+ ToString(value._swizzles[1])
						+ ToString(value._swizzles[2])
						+ ToString(value._swizzles[3]);
					break;
				case Operand4ComponentSelectionMode::Select1:
					components = ToString(value._swizzles[0]);
					break;
				default:
					throw runtime_error("Unknown selection mode");
				}
				if (!components.empty())
					components = "." + components;
			}

			out << Wrap(value._modifier, ToString(value._operandType) + index + components);
			break;
		}
	}
	return out;
}

Operand::Operand(OpcodeType parentType) :
	_parentType(parentType),
	_modifier(OperandModifier::None),
	_selectionMode(Operand4ComponentSelectionMode::Mask),
	_componentMask(ComponentMask::None)
{

}

uint8_t Operand::GetNumComponents() const { return _numComponents; }
Operand4ComponentSelectionMode Operand::GetSelectionMode() const { return _selectionMode; }
ComponentMask Operand::GetComponentMask() const { return _componentMask; }
const Operand4ComponentName* Operand::GetSwizzles() const { return _swizzles; }
OperandType Operand::GetOperandType() const { return _operandType; }
OperandIndexDimension Operand::GetIndexDimension() const { return _indexDimension; }
const OperandIndexRepresentation* Operand::GetIndexRepresentations() const { return _indexRepresentations; }
bool Operand::IsExtended() const { return _isExtended; }
OperandModifier Operand::GetModifier() const { return _modifier; }
const OperandIndex* Operand::GetIndices() const { return _indices; }
const Number4& Operand::GetImmediateValues() const { return _immediateValues; }

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
void Operand::ReadExtendedOperand(Operand& operand, BytecodeReader& reader)
{
	auto token1 = reader.ReadUInt32();

	switch (DecodeValue<ExtendedOperandType>(token1, 0, 5))
	{
	case ExtendedOperandType::Modifier:
		operand._modifier = DecodeValue<OperandModifier>(token1, 6, 13);
		break;
	}
}