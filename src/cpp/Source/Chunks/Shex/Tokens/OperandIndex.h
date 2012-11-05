#pragma once

#include "PCH.h"

namespace SlimShader
{
	class Operand;

	struct OperandIndex
	{
	public :
		uint64_t Value;
		std::shared_ptr<Operand> Register;

		friend std::ostream& operator<<(std::ostream& out, const OperandIndex& value);
	};
};