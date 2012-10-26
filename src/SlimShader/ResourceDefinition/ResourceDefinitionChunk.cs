using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimShader.IO;
using SlimShader.Shader;
using SlimShader.Util;

namespace SlimShader.ResourceDefinition
{
	/// <summary>
	/// Most of this was adapted from 
	/// https://devel.nuclex.org/framework/browser/graphics/Nuclex.Graphics.Native/trunk/Source/Introspection/HlslShaderReflector.cpp?rev=1743
	/// Roughly corresponds to the D3D11_SHADER_DESC structure.
	/// </summary>
	public class ResourceDefinitionChunk : DxbcChunk
	{
		public List<ConstantBuffer> ConstantBuffers { get; private set; }
		public List<ResourceBinding> ResourceBindings { get; private set; }
		public ShaderVersion Target { get; private set; }
		public ShaderFlags Flags { get; private set; }
		public string Creator { get; private set; }

		public ResourceDefinitionChunk()
		{
			ConstantBuffers = new List<ConstantBuffer>();
			ResourceBindings = new List<ResourceBinding>();
		}

		public static ResourceDefinitionChunk Parse(BytecodeReader reader)
		{
			var headerReader = reader.CopyAtCurrentPosition();

			uint constantBufferCount = headerReader.ReadUInt32();
			uint constantBufferOffset = headerReader.ReadUInt32();
			uint resourceBindingCount = headerReader.ReadUInt32();
			uint resourceBindingOffset = headerReader.ReadUInt32();
			var target = ShaderTarget.Parse(headerReader);
			uint flags = headerReader.ReadUInt32();

			var creatorOffset = headerReader.ReadUInt32();
			var creatorReader = reader.CopyAtOffset((int) creatorOffset);
			var creator = creatorReader.ReadString();

			// TODO: Parse Direct3D 11 resource definition stuff.
			// https://github.com/mirrors/wine/blob/master/dlls/d3dcompiler_43/reflection.c#L1429

			if (target.MajorVersion >= 5)
			{
				string rd11 = headerReader.ReadUInt32().ToFourCcString();
				if (rd11 != "RD11")
					throw new ParseException("Expected RD11.");
				var unknown1 = headerReader.ReadUInt32();
				var unknown2 = headerReader.ReadUInt32();
				var unknown3 = headerReader.ReadUInt32();
				var unknown4 = headerReader.ReadUInt32();
				var unknown5 = headerReader.ReadUInt32();
				var unknown6 = headerReader.ReadUInt32();
				var unknown7 = headerReader.ReadUInt32();
			}

			var result = new ResourceDefinitionChunk
			{
				Target = target,
				Flags = (ShaderFlags) flags,
				Creator = creator
			};

			var constantBufferReader = reader.CopyAtOffset((int) constantBufferOffset);
			for (int i = 0; i < constantBufferCount; i++)
				result.ConstantBuffers.Add(ConstantBuffer.Parse(reader, constantBufferReader, result.Target));

			var resourceBindingReader = reader.CopyAtOffset((int) resourceBindingOffset);
			for (int i = 0; i < resourceBindingCount; i++)
				result.ResourceBindings.Add(ResourceBinding.Parse(reader, resourceBindingReader));

			return result;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			if (ConstantBuffers.Any())
			{
				sb.AppendLine("// Buffer Definitions: ");
				sb.AppendLine("//");

				foreach (var constantBuffer in ConstantBuffers)
					sb.Append(constantBuffer);

				sb.AppendLine("//");
			}

			if (ResourceBindings.Any())
			{
				sb.AppendLine("// Resource Bindings:");
				sb.AppendLine("//");
				sb.AppendLine("// Name                                 Type  Format         Dim Slot Elements");
				sb.AppendLine("// ------------------------------ ---------- ------- ----------- ---- --------");

				foreach (var resourceBinding in ResourceBindings)
					sb.AppendLine(resourceBinding.ToString());

				sb.AppendLine("//");
				sb.AppendLine("//");
			}

			return sb.ToString();
		}
	}
}