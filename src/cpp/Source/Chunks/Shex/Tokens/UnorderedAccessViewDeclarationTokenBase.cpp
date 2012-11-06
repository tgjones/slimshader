#include "PCH.h"
#include "UnorderedAccessViewDeclarationTokenBase.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

UnorderedAccessViewCoherency UnorderedAccessViewDeclarationTokenBase::GetCoherency() const { return _coherency; }

UnorderedAccessViewDeclarationTokenBase::UnorderedAccessViewDeclarationTokenBase(UnorderedAccessViewCoherency coherency, Operand operand)
	: _coherency(coherency), DeclarationToken(operand)
{

}