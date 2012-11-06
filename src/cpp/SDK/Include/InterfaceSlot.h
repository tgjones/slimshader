#pragma once

#include "PCH.h"
#include "BytecodeReader.h"

namespace SlimShader
{
	class InterfaceSlot
	{
	public :
		static InterfaceSlot Parse(const BytecodeReader& reader, BytecodeReader& interfaceSlotReader);

		uint32_t GetStartSlot() const;
		void SetStartSlot(uint32_t startSlot);

		uint32_t GetSlotSpan() const;

		const std::vector<uint32_t>& GetTypeIDs() const;
		const std::vector<uint32_t>& GetTableIDs() const;

		friend std::ostream& operator<<(std::ostream& out, const InterfaceSlot& value);

	private :
		InterfaceSlot() { }

		uint32_t _startSlot;
		uint32_t _slotSpan;
		std::vector<uint32_t> _typeIDs;
		std::vector<uint32_t> _tableIDs;
	};
};