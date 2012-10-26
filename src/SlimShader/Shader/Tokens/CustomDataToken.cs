using System;
using SlimShader.IO;
using SlimShader.Util;

namespace SlimShader.Shader.Tokens
{
	/// <summary>
	/// Custom-Data Block Format
	///
	/// DWORD 0 (CustomDataDescTok):
	/// [10:00] == D3D10_SB_OPCODE_CUSTOMDATA
	/// [31:11] == D3D10_SB_CUSTOMDATA_CLASS
	///
	/// DWORD 1: 
	///          32-bit unsigned integer count of number
	///          of DWORDs in custom-data block,
	///          including DWORD 0 and DWORD 1.
	///          So the minimum value is 0x00000002,
	///          meaning empty custom-data.
	///
	/// Layout of custom-data contents, for the various meta-data classes,
	/// not defined in this file.
	/// </summary>
	public abstract class CustomDataToken : OpcodeToken
	{
		public CustomDataClass CustomDataClass { get; private set; }

		public static CustomDataToken Parse(BytecodeReader reader, uint token0)
		{
			var customDataClass = token0.DecodeValue<CustomDataClass>(11, 31);
			CustomDataToken token;
			switch (customDataClass)
			{
				case CustomDataClass.DclImmediateConstantBuffer:
					token = ImmediateConstantBufferDeclarationToken.Parse(reader);
					break;
				case CustomDataClass.ShaderMessage:
					token = ShaderMessageDeclarationToken.Parse(reader);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			token.CustomDataClass = customDataClass;
			return token;
		}
	}
}