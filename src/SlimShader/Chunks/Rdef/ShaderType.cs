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
	/// Describes a shader-variable type.
	/// Based on D3D11_SHADER_TYPE_DESC.
	/// </summary>
	public class ShaderType
	{
		private readonly int _indent;
		private readonly bool _isFirst;

		/// <summary>
		/// Identifies the variable class as one of scalar, vector, matrix or object.
		/// </summary>
		public ShaderVariableClass VariableClass { get; private set; }

		/// <summary>
		/// The variable type.
		/// </summary>
		public ShaderVariableType VariableType { get; private set; }

		/// <summary>
		/// Number of rows in a matrix. Otherwise a numeric type returns 1, any other type returns 0.
		/// </summary>
		public ushort Rows { get; private set; }

		/// <summary>
		/// Number of columns in a matrix. Otherwise a numeric type returns 1, any other type returns 0.
		/// </summary>
		public ushort Columns { get; private set; }

		/// <summary>
		/// Number of elements in an array; otherwise 0.
		/// </summary>
		public ushort ElementCount { get; private set; }

		public List<ShaderTypeMember> Members { get; private set; }

		public string BaseTypeName { get; private set; }

		public ShaderType(int indent, bool isFirst)
		{
			_indent = indent;
			_isFirst = isFirst;
			Members = new List<ShaderTypeMember>();
		}

		public static ShaderType Parse(BytecodeReader reader, BytecodeReader typeReader, ShaderVersion target,
			int indent, bool isFirst, uint parentOffset)
		{
			var result = new ShaderType(indent, isFirst)
			{
				VariableClass = (ShaderVariableClass) typeReader.ReadUInt16(),
				VariableType = (ShaderVariableType) typeReader.ReadUInt16(),
				Rows = typeReader.ReadUInt16(),
				Columns = typeReader.ReadUInt16(),
				ElementCount = typeReader.ReadUInt16()
			};

			var memberCount = typeReader.ReadUInt16();
			var memberOffset = typeReader.ReadUInt32();

			if (target.MajorVersion >= 5)
			{
				var parentTypeOffset = typeReader.ReadUInt32(); // Guessing
				var parentTypeReader = reader.CopyAtOffset((int) parentTypeOffset);
				var parentTypeClass = (ShaderVariableClass) parentTypeReader.ReadUInt16();
				var unknown4 = parentTypeReader.ReadUInt16();

				var unknown1 = typeReader.ReadUInt32();
				if (unknown1 != 0)
				{
					var unknownReader = reader.CopyAtOffset((int) unknown1);
					uint unknown5 = unknownReader.ReadUInt32();
				}

				var unknown2 = typeReader.ReadUInt32();
				var unknown3 = typeReader.ReadUInt32();

				var parentNameOffset = typeReader.ReadUInt32();
				var parentNameReader = reader.CopyAtOffset((int) parentNameOffset);
				result.BaseTypeName = parentNameReader.ReadString();
			}

			if (memberCount > 0)
			{
				var memberReader = reader.CopyAtOffset((int) memberOffset);
				for (int i = 0; i < memberCount; i++)
					result.Members.Add(ShaderTypeMember.Parse(reader, memberReader, target, indent + 4, i == 0,
						parentOffset));
			}

			return result;
		}

		public override string ToString()
		{
			var indentString = "// " + new string(Enumerable.Repeat(' ', _indent).ToArray());
			var sb = new StringBuilder();
			if (_isFirst)
				sb.AppendLine(indentString);
			switch (VariableClass)
			{
				case ShaderVariableClass.InterfacePointer:
				case ShaderVariableClass.MatrixColumns:
				case ShaderVariableClass.MatrixRows:
				{
					sb.Append(indentString);
					if (!string.IsNullOrEmpty(BaseTypeName)) // BaseTypeName is only populated in SM 5.0
					{
						sb.Append(string.Format("{0}{1}", VariableClass.GetDescription(), BaseTypeName));
					}
					else
					{
						sb.Append(VariableClass.GetDescription());
						sb.Append(VariableType.GetDescription());
						if (Columns > 1)
						{
							sb.Append(Columns);
							if (Rows > 1)
								sb.Append("x" + Rows);
						}
					}
					break;
				}
				case ShaderVariableClass.Vector:
				{
					sb.Append(indentString + VariableType.GetDescription());
					if (Columns > 1)
						sb.Append(Columns);
					break;
				}
				case ShaderVariableClass.Struct:
					{
						if (!_isFirst)
							sb.AppendLine(indentString);
						sb.AppendLine(indentString + "struct " + BaseTypeName);
						sb.AppendLine(indentString + "{");
						foreach (var member in Members)
							sb.AppendLine(member.ToString());
						sb.AppendLine("//");
						sb.Append(indentString + "}");
						break;
					}
				case ShaderVariableClass.Scalar:
					{
						sb.Append(indentString + VariableType.GetDescription());
						break;
					}
				default:
					throw new ArgumentOutOfRangeException("Variable class '" + VariableClass + "' is not currently supported.");
			}
			return sb.ToString();
		}
	}
}