#pragma once

#include "PCH.h"
#include "DxbcChunk.h"
#include "ClassInstance.h"
#include "ClassType.h"
#include "InterfaceSlot.h"

namespace SlimShader
{
	class InterfacesChunk : public DxbcChunk
	{
	public :
		static std::shared_ptr<InterfacesChunk> Parse(BytecodeReader& reader, uint32_t sizeInBytes);

		uint32_t GetInterfaceSlotCount() const;
		const std::vector<ClassType>& GetAvailableClassTypes() const;
		const std::vector<ClassInstance>& GetAvailableClassInstances() const;
		const std::vector<InterfaceSlot>& GetInterfaceSlots() const;

		friend std::ostream& operator<<(std::ostream& out, const InterfacesChunk& value);

	private :
		InterfacesChunk() { }

		uint32_t _interfaceSlotCount;
		std::vector<ClassType> _availableClassTypes;
		std::vector<ClassInstance> _availableClassInstances;
		std::vector<InterfaceSlot> _interfaceSlots;
	};
};