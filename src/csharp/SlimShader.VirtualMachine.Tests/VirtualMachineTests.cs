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
			var output0 = (NumberRegister) vm.GetRegister(0, OperandType.Output, new RegisterIndex(0));
			Assert.That(output0.Value.Number0.Float, Is.EqualTo(1.0f));
			Assert.That(output0.Value.Number1.Float, Is.EqualTo(0.5f));
			Assert.That(output0.Value.Number2.Float, Is.EqualTo(0.4f));
			Assert.That(output0.Value.Number3.Float, Is.EqualTo(1.0f));
		}

		[Test]
		public void CanExecuteVertexShaderBasicHlsl()
		{
			// Arrange.
			var vm = new VirtualMachine(BytecodeContainer.Parse(File.ReadAllBytes("Shaders/VS/BasicHLSL_VS.o")), 1);

			var paramsRegister = (NumberRegister) vm.GetRegister(0, OperandType.ConstantBuffer, new RegisterIndex(1, 0));
			paramsRegister.Value.Number0.Int = 3; // nNumLights = 3
			paramsRegister.Value.Number1.Int = 1; // bTexture = true

			var inputRegister0 = (NumberRegister) vm.GetRegister(0, OperandType.Input, new RegisterIndex(0));
			inputRegister0.Value = new Number4(3.0f, 0.0f, 2.0f, 1.0f); // vPos

			var inputRegister1 = (NumberRegister) vm.GetRegister(0, OperandType.Input, new RegisterIndex(1));
			inputRegister1.Value = new Number4(0.0f, 1.0f, 0.0f, 0.0f); // vNormal

			var inputRegister2 = (NumberRegister) vm.GetRegister(0, OperandType.Input, new RegisterIndex(2));
			inputRegister2.Value = new Number4(0.0f, 1.0f, 0.0f, 0.0f); // vTexCoord0

			// Act.
			vm.Execute();

			// Assert.
			var output0 = (NumberRegister) vm.GetRegister(0, OperandType.Output, new RegisterIndex(0));
			Assert.That(output0.Value.Number0.Float, Is.EqualTo(1.0f));
			Assert.That(output0.Value.Number1.Float, Is.EqualTo(0.5f));
			Assert.That(output0.Value.Number2.Float, Is.EqualTo(0.4f));
			Assert.That(output0.Value.Number3.Float, Is.EqualTo(1.0f));
		}
	}
}