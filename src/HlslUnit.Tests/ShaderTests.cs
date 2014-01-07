using HlslUnit.Tests.Shaders.VS;
using NUnit.Framework;
using SharpDX;
using SharpDX.D3DCompiler;

namespace HlslUnit.Tests
{
	[TestFixture]
	public class ShaderTests
	{
		[Test]
		public void CanExecuteVertexShader()
		{
			// Arrange.
            var shader = new Shader(ShaderTestUtility.CompileShader(
                "Shaders/VS/BasicHLSL.fx", "RenderSceneVS", "vs_4_0"));
		    shader.SetConstantBuffer("$Globals", new BasicHlsl.VertexConstantBufferGlobals
		    {
                World = Matrix.Identity,
		        WorldViewProjection =
                    Matrix.LookAtRH(Vector3.UnitZ, Vector3.Zero, Vector3.UnitY) *
                    Matrix.PerspectiveFovRH(MathUtil.PiOverFour, 1, 1, 10),
		        LightDir = Vector3.Normalize(new Vector3(0.3f, 0.5f, 0.7f)),
                LightDiffuse = Vector4.One,
                LightAmbient = new Vector4(0.1f, 0.1f, 0.1f, 1.0f),
                MaterialDiffuseColor = new Vector4(0.8f, 0.5f, 0.3f, 1.0f),
                MaterialAmbientColor = new Vector4(0.2f, 0.1f, 0.15f, 1.0f)
		    });
            shader.SetConstantBuffer("$Params", new BasicHlsl.VertexConstantBufferParams
            {
                NumLights = 3,
                Texture = true
            });
			var vertexInput = new BasicHlsl.VertexShaderInput
			{
				Position = new Vector4(3, 0, 2, 1),
				Normal = new Vector3(0, 1, 0),
				TextureCoordinate = new Vector2(0, 1)
			};

			// Act.
			var output = shader.Execute<BasicHlsl.VertexShaderInput, BasicHlsl.VertexShaderOutput>(vertexInput);

			// Assert.
            Assert.That(output, Is.EqualTo(new BasicHlsl.VertexShaderOutput
            {
                Position = new Vector4(7.24264f, 0, -3.222222f, 1),
                Diffuse = new Vector4(1.337171f, 0.833232f, 0.5089392f, 1),
                TextureUV = new Vector2(0, 1)
            }));
		}

	    [Test]
        public void CanExecutePixelShader()
        {
            // Arrange.
            var shader = new Shader(ShaderTestUtility.CompileShader(
                "Shaders/VS/BasicHLSL.fx", "RenderScenePS", "ps_4_0",
                ShaderFlags.EnableBackwardsCompatibility));
            shader.SetConstantBuffer("$Params", new BasicHlsl.PixelConstantBufferParams
            {
                Texture = true
            });
            shader.SetResource("MeshTextureSampler", (u, v, w) => new Vector4(0.5f, 1.0f, 0.0f, 1.0f));
            var vertexOutput = new BasicHlsl.VertexShaderOutput
            {
                Position = new Vector4(0, 1, 2, 1),
                Diffuse = new Vector4(0.8f, 0.7f, 0.6f, 1.0f),
                TextureUV = new Vector2(0.5f, 0.3f)
            };

            // Act.
            var output = shader.Execute<BasicHlsl.VertexShaderOutput, Vector4>(vertexOutput);

            // Assert.
            Assert.That(output, Is.EqualTo(new Vector4(0.4f, 0.7f, 0.0f, 1.0f)));
        }
	}
}