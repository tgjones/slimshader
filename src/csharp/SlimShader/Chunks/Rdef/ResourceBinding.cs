using SlimShader.Chunks.Common;
using SlimShader.Util;

namespace SlimShader.Chunks.Rdef
{
	/// <summary>
	/// Roughly corresponds to the D3D11_SHADER_INPUT_BIND_DESC structure.
	/// </summary>
	public class ResourceBinding
	{
		/// <summary>
		/// Name of the shader resource.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Identifies the type of data in the resource.
		/// </summary>
		public ShaderInputType Type { get; private set; }

		/// <summary>
		/// Starting bind point.
		/// </summary>
		public uint BindPoint { get; private set; }

		/// <summary>
		/// Number of contiguous bind points for arrays.
		/// </summary>
		public uint BindCount { get; private set; }

		/// <summary>
		/// Shader input-parameter options.
		/// </summary>
		public ShaderInputFlags Flags { get; private set; }

		/// <summary>
		/// Identifies the dimensions of the bound resource.
		/// </summary>
		public ShaderResourceViewDimension Dimension { get; private set; }

		/// <summary>
		/// If the input is a texture, the return type.
		/// </summary>
		public ResourceReturnType ReturnType { get; private set; }

		/// <summary>
		/// The number of samples for a multisampled texture; otherwise 0.
		/// </summary>
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
				Dimension = (ShaderResourceViewDimension) resourceBindingReader.ReadUInt32(),
				NumSamples = resourceBindingReader.ReadUInt32(),
				BindPoint = resourceBindingReader.ReadUInt32(),
				BindCount = resourceBindingReader.ReadUInt32(),
				Flags = (ShaderInputFlags) resourceBindingReader.ReadUInt32()
			};
		}

		public override string ToString()
		{
			string returnType = ReturnType.GetDescription(Type);
			if (Flags.HasFlag(ShaderInputFlags.TextureComponent0) && Flags.HasFlag(ShaderInputFlags.TextureComponent1))
				returnType += "4";
			string typeDescription = Type.GetDescription();
			if (Flags.HasFlag(ShaderInputFlags.ComparisonSampler))
				typeDescription += "_c";
			return string.Format("// {0,-30} {1,10} {2,7} {3,11} {4,4} {5,8}",
				Name, typeDescription, returnType,
				Dimension.GetDescription(Type, ReturnType) + (Dimension.IsMultiSampled() ? NumSamples.ToString() : string.Empty),
				BindPoint, BindCount);
		}
	}
}