namespace SlimShader.Chunks.Common
{
	public enum Primitive
	{
		Undefined = 0,

		[Description("point", ChunkType.Shex)]
		Point = 1,

		[Description("line", ChunkType.Shex)]
		Line = 2,

		[Description("triangle", ChunkType.Shex)]
		Triangle = 3,

		// Adjacency values should be equal to (0x4 & non-adjacency):
		LineAdj = 6,
		TriangleAdj = 7,

		_1ControlPointPatch = 8,
		_2ControlPointPatch = 9,
		_3ControlPointPatch = 10,
		_4ControlPointPatch = 11,
		_5ControlPointPatch = 12,
		_6ControlPointPatch = 13,
		_7ControlPointPatch = 14,
		_8ControlPointPatch = 15,
		_9ControlPointPatch = 16,
		_10ControlPointPatch = 17,
		_11ControlPointPatch = 18,
		_12ControlPointPatch = 19,
		_13ControlPointPatch = 20,
		_14ControlPointPatch = 21,
		_15ControlPointPatch = 22,
		_16ControlPointPatch = 23,
		_17ControlPointPatch = 24,
		_18ControlPointPatch = 25,
		_19ControlPointPatch = 26,
		_20ControlPointPatch = 27,
		_21ControlPointPatch = 28,
		_22ControlPointPatch = 29,
		_23ControlPointPatch = 30,
		_24ControlPointPatch = 31,
		_25ControlPointPatch = 32,
		_26ControlPointPatch = 33,
		_27ControlPointPatch = 34,
		_28ControlPointPatch = 35,
		_29ControlPointPatch = 36,
		_30ControlPointPatch = 37,
		_31ControlPointPatch = 38,
		_32ControlPointPatch = 39,
	}
}