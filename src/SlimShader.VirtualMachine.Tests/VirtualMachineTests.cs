using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SharpDX;
using SlimShader.Chunks.Shex;
using SlimShader.VirtualMachine.Execution;
using SlimShader.VirtualMachine.Jitter;
using SlimShader.VirtualMachine.Registers;
using SlimShader.VirtualMachine.Resources;
using SlimShader.VirtualMachine.Tests.Shaders.VS;
using SlimShader.VirtualMachine.Tests.Util;

namespace SlimShader.VirtualMachine.Tests
{
	[TestFixture]
	public class VirtualMachineTests
	{
        [TestCaseSource("ShaderExecutors")]
		public void CanExecuteVertexShaderBasicHlsl(IShaderExecutor shaderExecutor)
	    {
            // Arrange.
            VirtualMachine.ShaderExecutor = shaderExecutor;
            var vm = new VirtualMachine(BytecodeContainer.Parse(File.ReadAllBytes("Shaders/VS/BasicHLSL_VS.o")), 1);

            var globals = new BasicHlsl.ConstantBufferGlobals
            {
                WorldViewProjection = Matrix.LookAtRH(Vector3.UnitZ, Vector3.Zero, Vector3.UnitY)
                    * Matrix.PerspectiveFovRH(MathUtil.PiOverFour, 1, 1, 10)
            };

            var vertexInput = new VertexPositionNormalTexture
            {
                Position = new Vector4(3, 0, 2, 1),
                Normal = new Vector3(0, 1, 0),
                TextureCoordinate = new Vector2(0, 1)
            };

            var direct3DResult = Direct3DUtility.ExecuteVertexShader("Shaders/VS/BasicHLSL_VS.o", globals, vertexInput);

            SetConstantBuffer(vm, 0, globals);

            vm.SetRegister(0, OperandType.ConstantBuffer, new RegisterIndex(1, 0), new Number4
            {
                Number0 = Number.FromInt(3), // nNumLights = 3
                Number1 = Number.FromInt(1) // bTexture = true
            });

            vm.SetRegister(0, OperandType.Input, new RegisterIndex(0), vertexInput.Position.ToNumber4());
            vm.SetRegister(0, OperandType.Input, new RegisterIndex(1), vertexInput.Normal.ToNumber4());
            vm.SetRegister(0, OperandType.Input, new RegisterIndex(2), vertexInput.TextureCoordinate.ToNumber4());

            // Act.
            vm.Execute();

			// Assert.
			var output0 = vm.GetRegister(0, OperandType.Output, new RegisterIndex(0));
			Assert.That(output0.Number0.Float, Is.EqualTo(direct3DResult.Position.X));
            Assert.That(output0.Number1.Float, Is.EqualTo(direct3DResult.Position.Y));
            Assert.That(output0.Number2.Float, Is.EqualTo(direct3DResult.Position.Z));
            Assert.That(output0.Number3.Float, Is.EqualTo(direct3DResult.Position.W));
		}

		[TestCaseSource("ShaderExecutors")]
		public void CanExecuteCubeMapGeometryShader(IShaderExecutor shaderExecutor)
		{
			// Arrange.
			VirtualMachine.ShaderExecutor = shaderExecutor;
			var vm = new VirtualMachine(BytecodeContainer.Parse(File.ReadAllBytes("Shaders/GS/GS_CubeMap_GS.o")), 1);

			// Set constant buffer values. (These values are taken from the Rasterizr environment mapping sample).
			vm.SetConstantBufferRegisterValue(0, 0, new Number4(0.143317f, 0f, 0.976717f, 0.976619f));
			vm.SetConstantBufferRegisterValue(0, 1, new Number4(0f, 1.000000f, 0f, 0));
			vm.SetConstantBufferRegisterValue(0, 2, new Number4(-0.651080f, 0f, 0.214997f, 0.214975f));
			vm.SetConstantBufferRegisterValue(0, 3, new Number4(-32.553982f, 0f, 9.749746f, 10.748772f));
			vm.SetConstantBufferRegisterValue(0, 4, new Number4(-0.143317f, 0f, -0.976717f, -0.976619f));
			vm.SetConstantBufferRegisterValue(0, 5, new Number4(0f, 1.000000f, 0f, 0));
			vm.SetConstantBufferRegisterValue(0, 6, new Number4(0.651080f, 0f, -0.214997f, -0.214975f));
			vm.SetConstantBufferRegisterValue(0, 7, new Number4(32.553982f, 0f, -11.749947f, -10.748772f));
			vm.SetConstantBufferRegisterValue(0, 8, new Number4(0.651080f, 0.214975f, 0f, 0));
			vm.SetConstantBufferRegisterValue(0, 9, new Number4(0f, 0f, 1.000100f, 1.000000f));
			vm.SetConstantBufferRegisterValue(0, 10, new Number4(0.143317f, -0.976619f, 0f, 0));
			vm.SetConstantBufferRegisterValue(0, 11, new Number4(7.165847f, -48.830967f, -1.000100f, 0));
			vm.SetConstantBufferRegisterValue(0, 12, new Number4(0.651080f, -0.214975f, 0f, 0));
			vm.SetConstantBufferRegisterValue(0, 13, new Number4(0f, 0f, -1.000100f, -1.000000f));
			vm.SetConstantBufferRegisterValue(0, 14, new Number4(0.143317f, 0.976619f, 0f, 0));
			vm.SetConstantBufferRegisterValue(0, 15, new Number4(7.165847f, 48.830967f, -1.000100f, 0));
			vm.SetConstantBufferRegisterValue(0, 16, new Number4(0.651080f, 0f, -0.214997f, -0.214975f));
			vm.SetConstantBufferRegisterValue(0, 17, new Number4(0f, 1.000000f, 0f, 0));
			vm.SetConstantBufferRegisterValue(0, 18, new Number4(0.143317f, 0f, 0.976717f, 0.976619f));
			vm.SetConstantBufferRegisterValue(0, 19, new Number4(7.165847f, 0f, 47.835758f, 48.830975f));
			vm.SetConstantBufferRegisterValue(0, 20, new Number4(-0.651080f, 0f, 0.214997f, 0.214975f));
			vm.SetConstantBufferRegisterValue(0, 21, new Number4(0f, 1.000000f, 0f, 0));
			vm.SetConstantBufferRegisterValue(0, 22, new Number4(-0.143317f, 0f, -0.976717f, -0.976619f));
			vm.SetConstantBufferRegisterValue(0, 23, new Number4(-7.165847f, 0f, -49.835957f, -48.830975f));

			// Set input values.
			vm.SetInputRegisterValue(0, 0, 0, new Number4(12.834300f,11.131500f,-0.087300f,0));
			vm.SetInputRegisterValue(0, 0, 1, new Number4(12.515460f,11.131500f,-2.844318f,0));
			vm.SetInputRegisterValue(0, 0, 2, new Number4(-0.944098f,-0.255800f,0.207817f,0));
			vm.SetInputRegisterValue(0, 0, 3, new Number4(56.420399f,-55.420399f,0,0));
			vm.SetInputRegisterValue(0, 1, 0, new Number4(11.860000f,11.131500f,4.847000f,0));
			vm.SetInputRegisterValue(0, 1, 1, new Number4(12.624693f,11.131500f,2.184066f,0));
			vm.SetInputRegisterValue(0, 1, 2, new Number4(-0.951641f,-0.256300f,-0.169278f,0));
			vm.SetInputRegisterValue(0, 1, 3, new Number4(49.367802f,-55.420399f,0,0));
			vm.SetInputRegisterValue(0, 2, 0, new Number4(11.698200f,11.794400f,4.778200f,0));
			vm.SetInputRegisterValue(0, 2, 1, new Number4(12.451886f,11.794400f,2.151658f,0));
			vm.SetInputRegisterValue(0, 2, 2, new Number4(-0.951859f,0.256000f,-0.168411f,0));
			vm.SetInputRegisterValue(0, 2, 3, new Number4(49.367802f, -54.715099f, 0, 0));

			var expectedOutputsForFirstVertex = new[]
			{
				new Number4(-30.657768f, 11.131500f, 22.266457f, 23.264225f),
				new Number4(12.515460f, 11.131500f, -2.844318f, 0),
				new Number4(-0.944098f, -0.255800f, 0.207817f, 0),
				new Number4(56.420399f, -55.420399f, 0, 0),
				new Number4(0, 0, 0, 0)
			};

			// Act.
			var executionEnumerator = vm.ExecuteMultiple().GetEnumerator();
			int index = 0;
			while (true)
			{
				executionEnumerator.MoveNext();
				if (executionEnumerator.Current == ExecutionResponse.Finished)
					break;
				Assert.That(executionEnumerator.Current, Is.EqualTo(ExecutionResponse.Emit));

				if (index == 0)
				{
					for (ushort i = 0; i < expectedOutputsForFirstVertex.Length; i++)
					{
						var output = vm.GetRegister(0, OperandType.Output, new RegisterIndex(i));
						Assert.That(output, Is.EqualTo(expectedOutputsForFirstVertex[i]));
					}
				}

				executionEnumerator.MoveNext();
				Assert.That(executionEnumerator.Current, Is.EqualTo(ExecutionResponse.Emit));

				executionEnumerator.MoveNext();
				Assert.That(executionEnumerator.Current, Is.EqualTo(ExecutionResponse.Emit));

				executionEnumerator.MoveNext();
				Assert.That(executionEnumerator.Current, Is.EqualTo(ExecutionResponse.Cut));

				// SV_RenderTargetArrayIndex
				var output5 = vm.GetRegister(0, OperandType.Output, new RegisterIndex(4));
				Assert.That(output5.Number0.Int, Is.EqualTo(index));

				index++;
			}

			Assert.That(index, Is.EqualTo(6));
		}

		[TestCaseSource("ShaderExecutors")]
		public void CanExecuteSimplePixelShader(IShaderExecutor shaderExecutor)
		{
			// Arrange.
			VirtualMachine.ShaderExecutor = shaderExecutor;
			var vm = new VirtualMachine(BytecodeContainer.Parse(File.ReadAllBytes("Shaders/PS/Simple.o")), 4);

			// Act.
			vm.Execute();

			// Assert.
			var output0 = vm.GetRegister(0, OperandType.Output, new RegisterIndex(0));
			Assert.That(output0.Number0.Float, Is.EqualTo(1.0f));
			Assert.That(output0.Number1.Float, Is.EqualTo(0.5f));
			Assert.That(output0.Number2.Float, Is.EqualTo(0.4f));
			Assert.That(output0.Number3.Float, Is.EqualTo(1.0f));
		}

        [TestCaseSource("ShaderExecutors")]
        public void CanExecutePixelShaderBasicHlsl(IShaderExecutor shaderExecutor)
        {
            // Arrange.
            VirtualMachine.ShaderExecutor = shaderExecutor;
            var vm = new VirtualMachine(BytecodeContainer.Parse(File.ReadAllBytes("Shaders/PS/BasicHLSL_PS.o")), 4);

            vm.SetRegister(0, OperandType.ConstantBuffer, new RegisterIndex(0, 0), new Number4
            {
                Number0 = Number.FromUInt(1) // bTexture = true
            });

            vm.SetRegister(0, OperandType.Input, new RegisterIndex(1), new Number4(0.5f, 0.5f, 1.5f, 1)); // COLOR0
            vm.SetRegister(0, OperandType.Input, new RegisterIndex(2), new Number4(0, 0, 0, 0)); // TEXCOORD0

            vm.SetTexture(new RegisterIndex(0), new FakeTexture(new Number4(0.8f, 0.6f, 0.4f, 1)));
            vm.SetSampler(new RegisterIndex(0), new SamplerState());

            // Act.
            vm.Execute();

            // Assert.
            var output0 = vm.GetRegister(0, OperandType.Output, new RegisterIndex(0));
            Assert.That(output0.Number0.Float, Is.EqualTo(0.4f));
            Assert.That(output0.Number1.Float, Is.EqualTo(0.3f));
            Assert.That(output0.Number2.Float, Is.EqualTo(0.6f));
            Assert.That(output0.Number3.Float, Is.EqualTo(1));
        }

	    [TestCaseSource("ShaderExecutors"), Ignore]
        public void PerformanceTest(IShaderExecutor shaderExecutor)
	    {
	        var bytecodeContainer = BytecodeContainer.Parse(File.ReadAllBytes("Shaders/VS/BasicHLSL_VS.o"));
            VirtualMachine.ShaderExecutor = shaderExecutor;
            var vm = new VirtualMachine(bytecodeContainer, 1);

            var globals = new BasicHlsl.ConstantBufferGlobals
            {
                WorldViewProjection = Matrix.LookAtRH(Vector3.UnitZ, Vector3.Zero, Vector3.UnitY)
                    * Matrix.PerspectiveFovRH(MathUtil.PiOverFour, 1, 1, 10)
            };

            var vertexInput = new VertexPositionNormalTexture
            {
                Position = new Vector4(3, 0, 2, 1),
                Normal = new Vector3(0, 1, 0),
                TextureCoordinate = new Vector2(0, 1)
            };

            SetConstantBuffer(vm, 0, globals);

            vm.SetRegister(0, OperandType.ConstantBuffer, new RegisterIndex(1, 0), new Number4
            {
                Number0 = Number.FromInt(3), // nNumLights = 3
                Number1 = Number.FromInt(1) // bTexture = true
            });

            vm.SetRegister(0, OperandType.Input, new RegisterIndex(0), vertexInput.Position.ToNumber4());
            vm.SetRegister(0, OperandType.Input, new RegisterIndex(1), vertexInput.Normal.ToNumber4());
            vm.SetRegister(0, OperandType.Input, new RegisterIndex(2), vertexInput.TextureCoordinate.ToNumber4());

            // Prime the pump by executing shader once.
	        vm.Execute();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            const int iterations = 100000;
	        for (var i = 0; i < iterations; i++)
	            vm.Execute();

            stopwatch.Stop();

            Debug.WriteLine("Time: " + stopwatch.Elapsed);
        }

        [TestCaseSource("ShaderExecutors")]
		public void CanExecuteSimpleVertexShader(IShaderExecutor shaderExecutor)
		{
			// Arrange.
            VirtualMachine.ShaderExecutor = shaderExecutor;
			var vm = new VirtualMachine(BytecodeContainer.Parse(File.ReadAllBytes("Shaders/VS/Simple.o")), 1);

			// Set WorldViewProjection matrix into constant buffer
			vm.SetRegister(0, OperandType.ConstantBuffer, new RegisterIndex(0, 0), new Number4(1, 2, 3, 4));
			vm.SetRegister(0, OperandType.ConstantBuffer, new RegisterIndex(0, 1), new Number4(5, 6, 7, 8));
			vm.SetRegister(0, OperandType.ConstantBuffer, new RegisterIndex(0, 2), new Number4(4, 3, 2, 1));
			vm.SetRegister(0, OperandType.ConstantBuffer, new RegisterIndex(0, 3), new Number4(8, 7, 6, 5));

			// Set Inputs
			vm.SetRegister(0, OperandType.Input, new RegisterIndex(0), new Number4(3.0f, -5.0f, 2.0f, 0.0f)); // POSITION
			vm.SetRegister(0, OperandType.Input, new RegisterIndex(1), new Number4(0.0f, 1.0f, 0.0f, 0.0f)); // NORMAL
			vm.SetRegister(0, OperandType.Input, new RegisterIndex(2), new Number4(0.5f, 0.3f, 0.0f, 0.0f)); // TEXCOORD

			// Act.
			vm.Execute();

			// Assert.
			var output0 = vm.GetRegister(0, OperandType.Output, new RegisterIndex(0)); 
			Assert.That(output0, Is.EqualTo(new Number4(-14.0f, -18.0f, -22.0f, -26.0f)));

			var output1 = vm.GetRegister(0, OperandType.Output, new RegisterIndex(1));
			Assert.That(output1.Number0.Float, Is.EqualTo(0.0f));
			Assert.That(output1.Number1.Float, Is.EqualTo(1.0f));
			Assert.That(output1.Number2.Float, Is.EqualTo(0.0f));

			var output2 = vm.GetRegister(0, OperandType.Output, new RegisterIndex(2));
			Assert.That(output2.Number0.Float, Is.EqualTo(0.5f));
			Assert.That(output2.Number1.Float, Is.EqualTo(0.3f));
		}

	    private IEnumerable<IShaderExecutor> ShaderExecutors
	    {
	        get
	        {
	            yield return new Interpreter();
	            yield return new JitShaderExecutor();
	        }
	    }

	    private static void SetConstantBuffer<T>(VirtualMachine vm, ushort constantBufferIndex, T data)
	    {
            var bytes = StructUtility.ToBytes(data);

	        for (var i = 0; i < bytes.Length; i += 16)
	        {
	            vm.SetRegister(0, OperandType.ConstantBuffer,
	                new RegisterIndex(constantBufferIndex, (ushort) (i / 16)),
	                Number4.FromByteArray(bytes, i));
	        }
	    }
	}
}