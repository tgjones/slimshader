using System;

namespace SlimShader.Chunks
{
	/// <summary>
	/// Provided here because Portable Class Libraries don't include
	/// System.ComponentModel.DescriptionAttribute.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public class DescriptionAttribute : Attribute
	{
		private readonly string _description;
		private readonly ChunkType _chunkType;

		public string Description
		{
			get { return _description; }
		}

		public ChunkType ChunkType
		{
			get { return _chunkType; }
		}

		public DescriptionAttribute(string description, ChunkType chunkType = ChunkType.Unknown)
		{
			_description = description;
			_chunkType = chunkType;
		}
	}
}