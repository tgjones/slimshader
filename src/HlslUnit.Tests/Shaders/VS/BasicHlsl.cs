using System.Runtime.InteropServices;
using SharpDX;

namespace HlslUnit.Tests.Shaders.VS
{
    internal static class BasicHlsl
    {
        [StructLayout(LayoutKind.Explicit, Size = 224)]
        public struct ConstantBufferGlobals
        {
            [FieldOffset(0)]
            public Vector4 MaterialAmbientColor;

            [FieldOffset(16)]
            public Vector4 MaterialDiffuseColor;

            [FieldOffset(32)]
            public int NumLights;

            [FieldOffset(36)]
            public Vector3 LightDir;

            [FieldOffset(48)]
            public Vector4 LightDiffuse;

            [FieldOffset(64)]
            public Vector4 LightAmbient;

            [FieldOffset(80)]
            public float Time;

            [FieldOffset(96)]
            public Matrix World;

            [FieldOffset(160)]
            public Matrix WorldViewProjection;
        }

		[StructLayout(LayoutKind.Explicit, Size = 12)]
		public struct ConstantBufferParams
		{
			[FieldOffset(0)]
			public int NumLights;

			[FieldOffset(4)]
			public bool Texture;

			[FieldOffset(8)]
			public bool Animate;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct VertexShaderInput
		{
			public static int SizeInBytes
			{
				get { return 36; }
			}

			public Vector4 Position;
			public Vector3 Normal;
			public Vector2 TextureCoordinate;
		}

        [StructLayout(LayoutKind.Sequential)]
        public struct VertexShaderOutput
        {
            public static int SizeInBytes
            {
                get { return 40; }
            }

            public Vector4 Position;
            public Vector4 Diffuse;
            public Vector2 TextureUV;
        }
    }
}