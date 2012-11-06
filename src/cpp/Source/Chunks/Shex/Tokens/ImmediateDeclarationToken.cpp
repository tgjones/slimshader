#include "PCH.h"
#include "ImmediateDeclarationToken.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

uint32_t ImmediateDeclarationToken::GetDeclarationLength() const { return _declarationLength; }