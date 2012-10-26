using System;

namespace SlimShader.Chunks.Shex
{
	[Flags]
	public enum SyncFlags
	{
		None,

		ThreadsInGroup = 1,
		SharedMemory = 2,
		UnorderedAccessViewGroup = 4,
		UnorderedAccessViewGlobal = 8
	}
}