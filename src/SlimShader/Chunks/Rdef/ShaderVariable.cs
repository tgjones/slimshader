using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimShader.Chunks.Common;
using SlimShader.Chunks.Shex;
using SlimShader.Util;

namespace SlimShader.Chunks.Rdef
{
	/// <summary>
	/// Describes a shader variable.
	/// Based on D3D11_SHADER_VARIABLE_DESC.
	/// </summary>
	public class ShaderVariable
	{
		private ShaderTypeMember Member { get; set; }

		/// <summary>
		/// The variable name.
		/// </summary>
		public string Name
		{
			get { return Member.Name; }
		}

		/// <summary>
		/// Offset from the start of the parent structure, to the beginning of the variable.
		/// </summary>
		public uint StartOffset
		{
			get { return Member.Offset; }
		}

		/// <summary>
		/// Get a shader-variable type.
		/// </summary>
		public ShaderType ShaderType
		{
			get { return Member.Type; }
		}

		/// <summary>
		/// Gets the name of the base class.
		/// </summary>
		public string BaseType { get; private set; }

		/// <summary>
		/// Size of the variable (in bytes).
		/// </summary>
		public uint Size { get; set; }

		/// <summary>
		/// Flags, which identify shader-variable properties.
		/// </summary>
		public ShaderVariableFlags Flags { get; private set; }

		/// <summary>
		/// The default value for initializing the variable.
		/// </summary>
		public List<Number> DefaultValue { get; private set; }

		/// <summary>
		/// First texture index (or -1 if no textures used).
		/// </summary>
		public int StartTexture { get; private set; }

		/// <summary>
		/// Number of texture slots possibly used.
		/// </summary>
		public int TextureSize { get; private set; }
		
		/// <summary>
		/// First sampler index (or -1 if no textures used)
		/// </summary>
		public int StartSampler { get; private set; }

		/// <summary>
		/// Number of sampler slots possibly used.
		/// </summary>
		public int SamplerSize { get; private set; }

		///// <summary>
		///// Gets the corresponding interface slot for a variable that represents an interface pointer.
		///// </summary>
		//public List<uint> InterfaceSlots { get; private set; }

		public static ShaderVariable Parse(BytecodeReader reader,
			BytecodeReader variableReader, ShaderVersion target,
			bool isFirst)
		{
			uint nameOffset = variableReader.ReadUInt32();
			var nameReader = reader.CopyAtOffset((int) nameOffset);

			var startOffset = variableReader.ReadUInt32();
			uint size = variableReader.ReadUInt32();
			var flags = (ShaderVariableFlags) variableReader.ReadUInt32();

			var typeOffset = variableReader.ReadUInt32();
			var typeReader = reader.CopyAtOffset((int) typeOffset);
			var shaderType = ShaderType.Parse(reader, typeReader, target, 2, isFirst, startOffset);

			var defaultValueOffset = variableReader.ReadUInt32();
			List<Number> defaultValue = null;
			if (defaultValueOffset != 0)
			{
				defaultValue = new List<Number>();
				var defaultValueReader = reader.CopyAtOffset((int) defaultValueOffset);
				if (size % 4 != 0)
					throw new ParseException("Can only deal with 4-byte default values at the moment.");
				for (int i = 0; i < size; i += 4)
					defaultValue.Add(new Number(defaultValueReader.ReadBytes(4)));
			}

			var name = nameReader.ReadString();
			var result = new ShaderVariable
			{
				DefaultValue = defaultValue,
				Member = new ShaderTypeMember(0)
				{
					Name = name,
					Offset = startOffset,
					Type = shaderType
				},
				BaseType = name,
				Size = size,
				Flags = flags
			};

			if (target.MajorVersion >= 5)
			{
				result.StartTexture = variableReader.ReadInt32();
				result.TextureSize = variableReader.ReadInt32();
				result.StartSampler = variableReader.ReadInt32();
				result.SamplerSize = variableReader.ReadInt32();
			}

			return result;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.AppendFormat("{0} Size: {1,5}", Member, Size);

			if (!Flags.HasFlag(ShaderVariableFlags.Used))
				sb.Append(" [unused]");

			sb.AppendLine();

			if (DefaultValue != null)
			{
				sb.Append("//      = ");
				foreach (Number number in DefaultValue)
					sb.AppendFormat("0x{0:x8} ", number.UInt);
				sb.AppendLine();
			}

			return sb.ToString();
		}
	}
}