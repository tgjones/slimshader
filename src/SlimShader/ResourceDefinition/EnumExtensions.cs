using System;
using SlimShader.Shader;

namespace SlimShader.ResourceDefinition
{
	internal static class EnumExtensions
	{
		public static bool IsMultiSampled(this ResourceDimension value)
		{
			switch (value)
			{
				case ResourceDimension.Texture2DMultiSampled :
				case ResourceDimension.Texture2DMultiSampledArray :
					return true;
				default :
					return false;
			}
		}

		public static string GetDescription(this ResourceDimension value, ShaderInputType shaderInputType)
		{
			switch (shaderInputType)
			{
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