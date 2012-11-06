#pragma once

#include "PCH.h"
#include "BytecodeReader.h"
#include "ComponentMask.h"
#include "Number.h"
#include "OpcodeType.h"
#include "Operand4ComponentName.h"
#include "Operand4ComponentSelectionMode.h"
#include "OperandIndex.h"
#include "OperandIndexDimension.h"
#include "OperandIndexRepresentation.h"
#include "OperandModifier.h"
#include "OperandType.h"

namespace SlimShader
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
	class Operand
	{
	public :
		static Operand Parse(BytecodeReader& reader, OpcodeType parentType);

		Operand();

		uint8_t GetNumComponents() const;
		Operand4ComponentSelectionMode GetSelectionMode() const;
		ComponentMask GetComponentMask() const;
		const Operand4ComponentName* GetSwizzles() const;
		OperandType GetOperandType() const;
		OperandIndexDimension GetIndexDimension() const;
		const OperandIndexRepresentation* GetIndexRepresentations() const;
		bool IsExtended() const;
		OperandModifier GetModifier() const;
		const OperandIndex* GetIndices() const;
		const Number* GetImmediateValues32() const;
		const double* GetImmediateValues64() const;

		friend std::ostream& operator<<(std::ostream& out, const Operand& value);

	private :
		Operand(OpcodeType parentType);

		static void ReadExtendedOperand(Operand& operand, BytecodeReader& reader);

		const OpcodeType _parentType;
		uint8_t _numComponents;
		Operand4ComponentSelectionMode _selectionMode;
		ComponentMask _componentMask;
		Operand4ComponentName _swizzles[4];
		OperandType _operandType;
		OperandIndexDimension _indexDimension;
		// TODO: Can merge this with Indices?
		OperandIndexRepresentation _indexRepresentations[3];
		bool _isExtended;
		OperandModifier _modifier;
		OperandIndex _indices[3];
		Number _immediateValues32[4];
		double _immediateValues64[4];
	};
};