using System;

namespace SlimShader.Chunks.Shex
{
	[Flags]
	public enum ComponentMask
	{
		None = 0, // TODO: Might not need this if Operand is refactored to include only relevant properties.

		X = 1,
		Y = 2,
		Z = 4,
		W = 8,

		R = 1,
		G = 2,
		B = 4,
		A = 8,

		All = X | Y | Z | W // 15
	}
}