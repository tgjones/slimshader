using System;
using SlimShader.Chunks.Common;

namespace SlimShader.Chunks.Rdef
{
	internal static class EnumExtensions
	{
		public static bool IsMultiSampled(this ShaderResourceViewDimension value)
		{
			switch (value)
			{
				case ShaderResourceViewDimension.Texture2DMultiSampled :
				case ShaderResourceViewDimension.Texture2DMultiSampledArray :
					return true;
				default :
					return false;
			}
		}

		public static string GetDescription(this ShaderResourceViewDimension value, ShaderInputType shaderInputType,
			ResourceReturnType format)
		{
			switch (shaderInputType)
			{
				case ShaderInputType.ByteAddress :
				case ShaderInputType.Structured:
					return "r/o";
				case ShaderInputType.UavRwByteAddress :
				case ShaderInputType.UavRwStructured :
				case ShaderInputType.UavRwStructuredWithCounter :
				case ShaderInputType.UavRwTyped :
					return "r/w";
				default :
					return value.GetDescription();
			}
		}

		public static string GetDescription(this ResourceReturnType value, ShaderInputType shaderInputType)
		{
			if (value == ResourceReturnType.Mixed)
			{
				switch (shaderInputType)
				{
					case ShaderInputType.Structured:
					case ShaderInputType.UavRwStructured:
						return "struct";
					case ShaderInputType.ByteAddress:
					case ShaderInputType.UavRwByteAddress:
						return "byte";
					default:
						throw new ArgumentOutOfRangeException("shaderInputType", 
							"Shader input type '" + shaderInputType + "' is not supported.");
				}
			}
			return value.GetDescription();
		}
	}
}