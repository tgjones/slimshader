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
		public void CanExecutePixelShader()
		{
			// Arrange.
			var vm = new VirtualMachine(BytecodeContainer.Parse(File.ReadAllBytes("Shaders/PS/Simple.o")), 1);

			// Act.
			vm.Execute();

			// Assert.
			var registerKey = new RegisterKey(OperandType.Output, new RegisterIndex(0));
			var output0 = (NumberRegister) vm.GetRegister(0, registerKey);
			Assert.That(output0.Value.Number0.Float, Is.EqualTo(1.0f));
			Assert.That(output0.Value.Number1.Float, Is.EqualTo(0.5f));
			Assert.That(output0.Value.Number2.Float, Is.EqualTo(0.4f));
			Assert.That(output0.Value.Number3.Float, Is.EqualTo(1.0f));
		}
	}
}