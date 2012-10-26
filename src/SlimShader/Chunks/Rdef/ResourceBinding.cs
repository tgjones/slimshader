using SlimShader.Chunks.Shex;
using SlimShader.Util;

namespace SlimShader.Chunks.Rdef
{
	/// <summary>
	/// Roughly corresponds to the D3D11_SHADER_INPUT_BIND_DESC structure.
	/// </summary>
	public class ResourceBinding
	{
		public string Name { get; private set; }
		public ShaderInputType Type { get; private set; }
		public uint BindPoint { get; private set; }
		public uint BindCount { get; private set; }
		public ShaderInputFlags Flags { get; private set; }
		public ResourceDimension Dimension { get; private set; }
		public ResourceReturnType ReturnType { get; private set; }
		public uint NumSamples { get; private set; }

		public static ResourceBinding Parse(BytecodeReader reader, BytecodeReader resourceBindingReader)
		{
			uint nameOffset = resourceBindingReader.ReadUInt32();
			var nameReader = reader.CopyAtOffset((int) nameOffset);
			return new ResourceBinding
			{
				Name = nameReader.ReadString(),
				Type = (ShaderInputType) resourceBindingReader.ReadUInt32(),
				ReturnType = (ResourceReturnType) resourceBindingReader.ReadUInt32(),
				Dimension = (ResourceDimension) resourceBindingReader.ReadUInt32(),
				NumSamples = resourceBindingReader.ReadUInt32(),
				BindPoint = resourceBindingReader.ReadUInt32(),
				BindCount = resourceBindingReader.ReadUInt32(),
				Flags = (ShaderInputFlags) resourceBindingReader.ReadUInt32()
			};
		}

		public override string ToString()
		{
			string returnType = ReturnType.GetDescription(Type);
			if (ReturnType != ResourceReturnType.NotApplicable && ReturnType != ResourceReturnType.Mixed)
				returnType += "4";
			return string.Format("// {0,-30} {1,10} {2,7} {3,11} {4,4} {5,8}",
				Name, Type.GetDescription(), returnType,
				Dimension.GetDescription(Type) + (Dimension.IsMultiSampled() ? NumSamples.ToString() : string.Empty),
				BindPoint, BindCount);
		}
	}
}