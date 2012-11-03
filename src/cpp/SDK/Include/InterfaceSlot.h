#pragma once

#include "PCH.h"
#include "BytecodeReader.h"

namespace SlimShader
{
	class InterfaceSlot
	{
	public :
		static InterfaceSlot Parse(const BytecodeReader& reader, BytecodeReader& interfaceSlotReader);

		const uint32_t GetID() const;
		void SetID(uint32_t id);

		const std::vector<uint32_t>& GetTypeIDs() const;
		const std::vector<uint32_t>& GetTableIDs() const;

		friend std::ostream& operator<<(std::ostream& out, const InterfaceSlot& value);

	private :
		InterfaceSlot() { }

		uint32_t _id;
		std::vector<uint32_t> _typeIDs;
		std::vector<uint32_t> _tableIDs;
	};
};