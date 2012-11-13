using System.IO;
using NUnit.Framework;

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
			Assert.That(vm.GlobalMemory.Outputs[0].Number0.Float, Is.EqualTo(1.0f));
			Assert.That(vm.GlobalMemory.Outputs[0].Number1.Float, Is.EqualTo(0.5f));
			Assert.That(vm.GlobalMemory.Outputs[0].Number2.Float, Is.EqualTo(0.4f));
			Assert.That(vm.GlobalMemory.Outputs[0].Number3.Float, Is.EqualTo(1.0f));
		}
	}
}