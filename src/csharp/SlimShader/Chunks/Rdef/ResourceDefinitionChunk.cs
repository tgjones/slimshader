using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SlimShader.Chunks.Common;
using SlimShader.Util;

namespace SlimShader.Chunks.Rdef
{
	/// <summary>
	/// Roughly corresponds to the D3D11_SHADER_DESC structure.
	/// </summary>
	public class ResourceDefinitionChunk : BytecodeChunk
	{
		public List<ConstantBuffer> ConstantBuffers { get; private set; }
		public List<ResourceBinding> ResourceBindings { get; private set; }
		public ShaderVersion Target { get; private set; }
		public ShaderFlags Flags { get; private set; }
		public string Creator { get; private set; }
		public uint InterfaceSlotCount { get; private set; }

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
			var target = ShaderVersion.ParseRdef(headerReader);
			uint flags = headerReader.ReadUInt32();

			var creatorOffset = headerReader.ReadUInt32();
			var creatorReader = reader.CopyAtOffset((int) creatorOffset);
			var creator = creatorReader.ReadString();

			var result = new ResourceDefinitionChunk
			{
				Target = target,
				Flags = (ShaderFlags) flags,
				Creator = creator
			};

			if (target.MajorVersion >= 5)
			{
				string rd11 = headerReader.ReadUInt32().ToFourCcString();
				if (rd11 != "RD11")
					throw new ParseException("Expected RD11.");

				var unknown1 = headerReader.ReadUInt32(); // TODO
				Debug.Assert(unknown1 == 60);

				var unknown2 = headerReader.ReadUInt32();
				Debug.Assert(unknown2 == 24);

				var unknown3 = headerReader.ReadUInt32();
				Debug.Assert(unknown3 == 32);

				var unknown4 = headerReader.ReadUInt32();
				Debug.Assert(unknown4 == 40);

				var unknown5 = headerReader.ReadUInt32();
				Debug.Assert(unknown5 == 36);

				var unknown6 = headerReader.ReadUInt32();
				Debug.Assert(unknown6 == 12);

				result.InterfaceSlotCount = headerReader.ReadUInt32();
			}

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