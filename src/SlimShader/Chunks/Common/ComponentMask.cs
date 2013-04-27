using System;

namespace SlimShader.Chunks.Common
{
	[Flags]
	public enum ComponentMask
	{
		None = 0,

		X = 1,
		Y = 2,
		Z = 4,
		W = 8,

		R = 1,
		G = 2,
		B = 4,
		A = 8,

		Xy = X | Y,
		Xyz = X | Y | Z,

		All = X | Y | Z | W // 15
	}
}