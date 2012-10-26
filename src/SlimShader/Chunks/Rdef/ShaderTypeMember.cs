using System;
using SlimShader.Chunks.Common;
using SlimShader.Util;

namespace SlimShader.Chunks.Rdef
{
	public class ShaderTypeMember
	{
		private readonly uint _parentOffset;

		/// <summary>
		/// The variable name.
		/// </summary>
		public string Name { get; internal set; }

		/// <summary>
		/// Offset from the start of the parent structure, to the beginning of the variable.
		/// </summary>
		public uint Offset { get; internal set; }

		/// <summary>
		/// Get a shader-variable type.
		/// </summary>
		public ShaderType Type { get; internal set; }

		public ShaderTypeMember(uint parentOffset)
		{
			_parentOffset = parentOffset;
		}

		public static ShaderTypeMember Parse(BytecodeReader reader, BytecodeReader memberReader, ShaderVersion target,
			int indent, bool isFirst, uint parentOffset)
		{
			var nameOffset = memberReader.ReadUInt32();
			var nameReader = reader.CopyAtOffset((int) nameOffset);
			var name = nameReader.ReadString();

			var memberTypeOffset = memberReader.ReadUInt32();

			var offset = memberReader.ReadUInt32();

			var memberTypeReader = reader.CopyAtOffset((int) memberTypeOffset);
			var memberType = ShaderType.Parse(reader, memberTypeReader, target, indent, isFirst, parentOffset + offset);

			return new ShaderTypeMember(parentOffset)
			{
				Name = name,
				Type = memberType,
				Offset = offset
			};
		}

		public override string ToString()
		{
			string declaration = Type + " " + Name;
			if (Type.ElementCount > 0)
				declaration += string.Format("[{0}]", Type.ElementCount);
			declaration += ";";

			// Split declaration into separate lines, so that we can put the "// Offset" comment at the right place.
			var declarationLines = declaration.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
			declarationLines[declarationLines.Length - 1] = string.Format("{0,-40}// Offset: {1,4}",
				declarationLines[declarationLines.Length - 1], _parentOffset + Offset);

			return string.Join(Environment.NewLine, declarationLines);
		}
	}
}