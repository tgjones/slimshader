using HlslUnit.Tests.Shaders.VS;
using NUnit.Framework;
using SharpDX;

namespace HlslUnit.Tests
{
	[TestFixture]
	public class ShaderTests
	{
		[Test]
		public void CanExecuteVertexShader()
		{
			// Arrange.
			var shader = new Shader("Shaders/VS/BasicHLSL.fx", "RenderSceneVS", "vs_4_0");
			shader.SetConstantBuffer("$Globals", new BasicHlsl.ConstantBufferGlobals
			{
				WorldViewProjection =
					Matrix.LookAtRH(Vector3.UnitZ, Vector3.Zero, Vector3.UnitY) *
					Matrix.PerspectiveFovRH(MathUtil.PiOverFour, 1, 1, 10)
			});
			var vertexInput = new BasicHlsl.VertexShaderInput
			{
				Position = new Vector4(3, 0, 2, 1),
				Normal = new Vector3(0, 1, 0),
				TextureCoordinate = new Vector2(0, 1)
			};

			// Act.
			var output = shader.Execute<BasicHlsl.VertexShaderOutput>(vertexInput);

			// Assert.
			Assert.That(output.Position, Is.EqualTo(new Vector4(7.24264f, 0, -3.222222f, 1)));
			Assert.That(output.Diffuse, Is.EqualTo(new Vector4(0, 0, 0, 1)));
			Assert.That(output.TextureUV, Is.EqualTo(new Vector2(0, 0)));
		}
	}
}