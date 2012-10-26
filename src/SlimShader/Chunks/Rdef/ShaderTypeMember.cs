using System;
using SlimShader.Chunks.Shex;
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
			// For example:
			// row_major modelview;               // Offset:    0 Size:    64
			// float4x4 modelview;                // Offset:    0 Size:    64
			// int unusedTestA;                   // Offset:   64 Size:     4 [unused]
			// float4 cool;                       // Offset:    0 Size:    16
			// interface iBaseLight g_abstractAmbientLighting;// Offset:    1 Size:     1
			// struct cAmbientLight
			// {
			//       
			//     float3 m_vLightColor;          // Offset:    0
			//     bool m_bEnable;                // Offset:   12
			//
			// } g_ambientLight;                  // Offset:    0 Size:    16

			//string variableType = string.Empty;
			//switch (Type.VariableClass)
			//{
			//	case ShaderVariableClass.MatrixRows:
			//		variableType += Type.VariableClass.GetDescription() + " ";
			//		break;
			//}

			//if (string.IsNullOrEmpty(Type.BaseTypeName))
			//{
			//	variableType += Type.VariableType.GetDescription();
			//	// TODO: This might not all be necessary - perhaps BaseTypeName is always set when it's an array?
			//	if (Type.Columns > 1 && Type.VariableType != ShaderVariableType.InterfacePointer)
			//	{
			//		variableType += Type.Columns;
			//		if (Type.Rows > 1)
			//			variableType += "x" + Type.Rows;
			//	}
			//}
			//else
			//{
			//	if (Type.VariableType == ShaderVariableType.InterfacePointer)
			//		variableType += Type.VariableType.GetDescription() + " ";
			//	variableType += Type.BaseTypeName;
			//}

			//string arrayCount = string.Empty;
			//if (Type.ElementCount > 0)
			//	arrayCount = "[" + Type.ElementCount + "]";

			//string declaration = string.Format("{0} {1}{2};", variableType, Name, arrayCount);
			//return string.Format("{0,-35}// Offset: {1,4}", declaration, Offset);

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