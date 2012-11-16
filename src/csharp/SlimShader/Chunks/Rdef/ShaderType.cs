using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SlimShader.Chunks.Common;
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

		/// <summary>
		/// Name of the shader-variable type. This member can be NULL if it isn't used. This member supports 
		/// dynamic shader linkage interface types, which have names.
		/// TODO: Is this the right description?
		/// </summary>
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
				if (parentTypeOffset != 0)
				{
					var parentTypeReader = reader.CopyAtOffset((int) parentTypeOffset);
					var parentTypeClass = (ShaderVariableClass) parentTypeReader.ReadUInt16();
					Debug.Assert(parentTypeClass == ShaderVariableClass.Vector || parentTypeClass == ShaderVariableClass.InterfaceClass);

					var unknown1 = parentTypeReader.ReadUInt16();
					Debug.Assert(unknown1 == 0);
				}

				var unknown2 = typeReader.ReadUInt32();
				if (unknown2 != 0)
				{
					var unknownReader = reader.CopyAtOffset((int) unknown2);
					uint unknown3 = unknownReader.ReadUInt32();
					Debug.Assert(unknown3 == 0 || unknown3 == 5);
				}

				var unknown4 = typeReader.ReadUInt32();
				Debug.Assert(unknown4 == 0 || unknown4 == 1);

				var unknown5 = typeReader.ReadUInt32();
				if (unknown5 != 0)
				{
					var unknownReader = reader.CopyAtOffset((int) unknown5);
					var unknown6 = unknownReader.ReadUInt32();
					Debug.Assert(unknown6 == 580 || unknown6 == 740);
				}

				var parentNameOffset = typeReader.ReadUInt32();
				if (parentNameOffset > 0)
				{
					var parentNameReader = reader.CopyAtOffset((int) parentNameOffset);
					result.BaseTypeName = parentNameReader.ReadString();
				}
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
					throw new InvalidOperationException(string.Format("Variable class '{0}' is not currently supported.", VariableClass));
			}
			return sb.ToString();
		}
	}
}