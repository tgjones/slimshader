using SlimShader.Chunks.Common;
using SlimShader.Util;

namespace SlimShader.Chunks.Shex.Tokens
{
	/// <summary>
	/// Resource Return Type Token (ResourceReturnTypeToken) (used in resource
	/// declaration statements)
	///
	/// [03:00] D3D10_SB_RESOURCE_RETURN_TYPE for component X
	/// [07:04] D3D10_SB_RESOURCE_RETURN_TYPE for component Y
	/// [11:08] D3D10_SB_RESOURCE_RETURN_TYPE for component Z
	/// [15:12] D3D10_SB_RESOURCE_RETURN_TYPE for component W
	/// [31:16] Reserved, 0
	/// </summary>
	public class ResourceReturnTypeToken
	{
		public ResourceReturnType X { get; internal set; }
		public ResourceReturnType Y { get; internal set; }
		public ResourceReturnType Z { get; internal set; }
		public ResourceReturnType W { get; internal set; }

		public static ResourceReturnTypeToken Parse(BytecodeReader reader)
		{
			var token = reader.ReadUInt32();
			return new ResourceReturnTypeToken
			{
				X = token.DecodeValue<ResourceReturnType>(00, 03),
				Y = token.DecodeValue<ResourceReturnType>(04, 07),
				Z = token.DecodeValue<ResourceReturnType>(08, 11),
				W = token.DecodeValue<ResourceReturnType>(12, 15)
			};
		}

		public override string ToString()
		{
			return string.Format("{0},{1},{2},{3}",
				X.GetDescription(),
				Y.GetDescription(),
				Z.GetDescription(),
				W.GetDescription());
		}
	}
}