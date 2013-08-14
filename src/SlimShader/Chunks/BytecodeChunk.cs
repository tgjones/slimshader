using System.Collections.Generic;
using SlimShader.Chunks.Ifce;
using SlimShader.Chunks.Rdef;
using SlimShader.Chunks.Sfi0;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Spdb;
using SlimShader.Chunks.Stat;
using SlimShader.Chunks.Xsgn;
using SlimShader.Util;

namespace SlimShader.Chunks
{
	public abstract class BytecodeChunk
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
			{ "SPDB".ToFourCc(), ChunkType.Spdb },
			{ "STAT".ToFourCc(), ChunkType.Stat }
		};

		public BytecodeContainer Container { get; private set; }
		public uint FourCc { get; private set; }
		public ChunkType ChunkType { get; private set; }
		public uint ChunkSize { get; private set; }

		public static BytecodeChunk ParseChunk(BytecodeReader chunkReader, BytecodeContainer container)
		{
			// Type of chunk this is.
			uint fourCc = chunkReader.ReadUInt32();

			// Total length of the chunk in bytes.
			uint chunkSize = chunkReader.ReadUInt32();

			ChunkType chunkType;
		    if (KnownChunkTypes.ContainsKey(fourCc))
		    {
		        chunkType = KnownChunkTypes[fourCc];
		    }
		    else
		    {
		        System.Diagnostics.Debug.WriteLine("Chunk type '" + fourCc.ToFourCcString() + "' is not yet supported.");
		        return null;
		    }

		    var chunkContentReader = chunkReader.CopyAtCurrentPosition((int) chunkSize);
			BytecodeChunk chunk;
			switch (chunkType)
			{
				case ChunkType.Ifce :
					chunk = InterfacesChunk.Parse(chunkContentReader, chunkSize);
					break;
				case ChunkType.Isgn :
				case ChunkType.Osgn:
				case ChunkType.Osg5:
				case ChunkType.Pcsg:
					chunk = InputOutputSignatureChunk.Parse(chunkContentReader, chunkType,
						container.ResourceDefinition.Target.ProgramType);
					break;
				case ChunkType.Rdef:
					chunk = ResourceDefinitionChunk.Parse(chunkContentReader);
					break;
				case ChunkType.Sdbg :
				case ChunkType.Spdb :
					chunk = DebuggingChunk.Parse(chunkContentReader, chunkType, (int) chunkSize);
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
					throw new ParseException("Invalid chunk type: " + chunkType);
			}

			chunk.Container = container;
			chunk.FourCc = fourCc;
			chunk.ChunkSize = chunkSize;
			chunk.ChunkType = chunkType;

			return chunk;
		}
	}
}