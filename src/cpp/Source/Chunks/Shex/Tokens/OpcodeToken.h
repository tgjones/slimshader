#pragma once

#include "PCH.h"
#include "OpcodeHeader.h"

namespace SlimShader
{
	class OpcodeToken
	{
	public :
		const OpcodeHeader& GetHeader() const;
		void SetHeader(OpcodeHeader header);

		friend std::ostream& operator<<(std::ostream& out, const OpcodeToken& value);

	protected :
		std::string GetTypeDescription() const;
		virtual void Print(std::ostream& out) const;

	private :
		OpcodeHeader _header;
	};
};