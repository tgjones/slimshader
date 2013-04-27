using System.IO;
using NUnit.Framework;
using SlimShader.Chunks.Shex;
using SlimShader.VirtualMachine.Registers;

namespace SlimShader.VirtualMachine.Tests
{
	[TestFixture]
	public class VirtualMachineTests
	{
		[Test]
		public void CanExecuteSimplePixelShader()
		{
			// Arrange.
			var vm = new VirtualMachine(BytecodeContainer.Parse(File.ReadAllBytes("Shaders/PS/Simple.o")), 1);

			// Act.
			vm.Execute();

			// Assert.
			var output0 = vm.GetRegister(0, OperandType.Output, new RegisterIndex(0));
			Assert.That(output0.Number0.Float, Is.EqualTo(1.0f));
			Assert.That(output0.Number1.Float, Is.EqualTo(0.5f));
			Assert.That(output0.Number2.Float, Is.EqualTo(0.4f));
			Assert.That(output0.Number3.Float, Is.EqualTo(1.0f));
		}

		[Test]
		public void CanExecuteVertexShaderBasicHlsl()
		{
			// Arrange.
			var vm = new VirtualMachine(BytecodeContainer.Parse(File.ReadAllBytes("Shaders/VS/BasicHLSL_VS.o")), 1);

			vm.SetRegister(0, OperandType.ConstantBuffer, new RegisterIndex(1, 0), new Number4
			{
				Number0 = Number.FromInt(3), // nNumLights = 3
				Number1 = Number.FromInt(1) // bTexture = true
			});

			vm.SetRegister(0, OperandType.Input, new RegisterIndex(0), new Number4(3.0f, 0.0f, 2.0f, 1.0f)); // vPos
			vm.SetRegister(0, OperandType.Input, new RegisterIndex(1), new Number4(0.0f, 1.0f, 0.0f, 0.0f)); // vNormal
			vm.SetRegister(0, OperandType.Input, new RegisterIndex(2), new Number4(0.0f, 1.0f, 0.0f, 0.0f)); // vTexCoord0

			// Act.
			vm.Execute();

			// Assert.
			var output0 = vm.GetRegister(0, OperandType.Output, new RegisterIndex(0));
			Assert.That(output0.Number0.Float, Is.EqualTo(1.0f));
			Assert.That(output0.Number1.Float, Is.EqualTo(0.5f));
			Assert.That(output0.Number2.Float, Is.EqualTo(0.4f));
			Assert.That(output0.Number3.Float, Is.EqualTo(1.0f));
		}

		[Test]
		public void CanExecuteSimpleVertexShader()
		{
			// Arrange.
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
	}
}