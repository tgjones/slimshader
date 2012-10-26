using System.Collections.Generic;
using System.Text;
using SlimShader.Chunks.Common;
using SlimShader.Chunks.Shex;
using SlimShader.Util;

namespace SlimShader.Chunks.Rdef
{
	/// <summary>
	/// Describes a shader constant-buffer.
	/// Based on D3D11_SHADER_BUFFER_DESC.
	/// </summary>
	public class ConstantBuffer
	{
		/// <summary>
		/// The name of the buffer.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// A <see cref="ConstantBufferType" />-typed value that indicates the intended use of the constant data.
		/// </summary>
		public ConstantBufferType BufferType { get; private set; }

		public List<ShaderVariable> Variables { get; private set; }

		/// <summary>
		/// Buffer size (in bytes).
		/// </summary>
		public uint Size { get; private set; }

		/// <summary>
		/// A combination of <see cref="ConstantBufferFlags" />-typed values that are combined by using a bitwise OR 
		/// operation. The resulting value specifies properties for the shader constant-buffer.
		/// </summary>
		public ConstantBufferFlags Flags { get; private set; }

		public ConstantBuffer()
		{
			Variables = new List<ShaderVariable>();
		}

		public static ConstantBuffer Parse(
			BytecodeReader reader, BytecodeReader constantBufferReader,
			ShaderVersion target)
		{
			uint nameOffset = constantBufferReader.ReadUInt32();
			var nameReader = reader.CopyAtOffset((int) nameOffset);

			uint variableCount = constantBufferReader.ReadUInt32();
			uint variableOffset = constantBufferReader.ReadUInt32();

			var result = new ConstantBuffer
			{
				Name = nameReader.ReadString()
			};

			var variableReader = reader.CopyAtOffset((int) variableOffset);
			for (int i = 0; i < variableCount; i++)
				result.Variables.Add(ShaderVariable.Parse(reader, variableReader, target, i == 0));

			result.Size = constantBufferReader.ReadUInt32();
			result.Flags = (ConstantBufferFlags) constantBufferReader.ReadUInt32();
			result.BufferType = (ConstantBufferType) constantBufferReader.ReadUInt32();

			return result;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine(string.Format("// {0} {1}", BufferType.GetDescription(), Name));
			sb.AppendLine("// {");

			foreach (var variable in Variables)
				sb.Append(variable);

			sb.AppendLine("//");
			sb.AppendLine("// }");
			sb.AppendLine("//");

			return sb.ToString();
		}
	}
}