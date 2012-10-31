using System;
using System.Collections.Generic;
using SlimShader.Chunks.Ifce;
using SlimShader.Chunks.Rdef;
using SlimShader.Chunks.Sdbg;
using SlimShader.Chunks.Sfio;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Stat;
using SlimShader.Chunks.Xsgn;
using SlimShader.Util;

namespace SlimShader.Chunks
{
	public abstract class DxbcChunk
	{
		private static readonly Dictionary<uint, ChunkType> KnownChunkTypes = new Dictionary<uint, ChunkType>
		{
			{ "IFCE".ToFourCc(), ChunkType.Ifce },
			{ "ISGN".ToFourCc(), ChunkType.Isgn },
			{ "OSGN".ToFourCc(), ChunkType.Osgn },
			{ "OSG5".ToFourCc(), ChunkType.Osg5 },
			{ "PCSG".ToFourCc(), ChunkType.Pcsg },
			{ "RDEF".ToFourCc(), ChunkType.Rdef },
			{ "SDBG".ToFourCc(), ChunkType.Sdbg },
			{ "SFI0".ToFourCc(), ChunkType.Sfi0 },
			{ "SHDR".ToFourCc(), ChunkType.Shdr },
			{ "SHEX".ToFourCc(), ChunkType.Shex },
			{ "STAT".ToFourCc(), ChunkType.Stat }
		};

		public uint FourCc { get; internal set; }
		public ChunkType ChunkType { get; internal set; }
		public uint ChunkSize { get; internal set; }

		public static DxbcChunk ParseChunk(BytecodeReader chunkReader, DxbcContainer container)
		{
			// Type of chunk this is.
			uint fourCc = chunkReader.ReadUInt32();

			// Total length of the chunk in bytes.
			uint chunkSize = chunkReader.ReadUInt32();

			ChunkType chunkType;
			if (KnownChunkTypes.ContainsKey(fourCc))
				chunkType = KnownChunkTypes[fourCc];
			else
				throw new NotSupportedException("Chunk type '" + fourCc.ToFourCcString() + "' is not yet supported.");

			var chunkContentReader = chunkReader.CopyAtCurrentPosition((int) chunkSize);
			DxbcChunk chunk;
			switch (chunkType)
			{
				case ChunkType.Ifce :
					chunk = InterfacesChunk.Parse(chunkContentReader, chunkSize);
					break;
				case ChunkType.Isgn :
				case ChunkType.Osgn:
				//case ChunkType.Osg5: // Doesn't seem to be used?
				case ChunkType.Pcsg:
					chunk = InputOutputSignatureChunk.Parse(chunkContentReader, chunkType,
						container.ResourceDefinition.Target.ProgramType);
					break;
				case ChunkType.Rdef:
					chunk = ResourceDefinitionChunk.Parse(chunkContentReader);
					break;
				case ChunkType.Sdbg :
					chunk = DebuggingChunk.Parse(chunkContentReader);
					break;
				case ChunkType.Sfi0:
					chunk = Sfi0Chunk.Parse(chunkContentReader);
					break;
				case ChunkType.Shdr:
				case ChunkType.Shex:
					chunk = ShaderProgramChunk.Parse(chunkContentReader);
					break;
				case ChunkType.Stat:
					chunk = StatisticsChunk.Parse(chunkContentReader, chunkSize);
					break;
				default :
					throw new ArgumentOutOfRangeException();
			}

			chunk.FourCc = fourCc;
			chunk.ChunkSize = chunkSize;
			chunk.ChunkType = chunkType;

			return chunk;
		}
	}
}