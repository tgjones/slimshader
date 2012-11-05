#include "PCH.h"
#include "SyncFlags.h"

#include "Decoder.h"

using namespace std;
using namespace SlimShader;

string SlimShader::ToString(SyncFlags value)
{
	string result;

	if (HasFlag(value, SyncFlags::UnorderedAccessViewGlobal))
		result += "_uglobal";
	if (HasFlag(value, SyncFlags::UnorderedAccessViewGroup))
		result += "_ugroup";
	if (HasFlag(value, SyncFlags::SharedMemory))
		result += "_g";
	if (HasFlag(value, SyncFlags::ThreadsInGroup))
		result += "_t";

	return result;
}