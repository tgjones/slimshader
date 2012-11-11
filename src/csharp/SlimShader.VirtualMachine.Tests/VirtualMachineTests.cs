using System.IO;
using NUnit.Framework;
using SlimShader.VirtualMachine.Resources;

namespace SlimShader.VirtualMachine.Tests
{
	[TestFixture]
	public class VirtualMachineTests
	{
		//public void CanExecuteComputeShader(string file)
		//{
		//	// Arrange.
		//	var vm = new VirtualMachine(DxbcContainer.Parse(File.ReadAllBytes(file)));
		//	vm.SetUnorderedAccessViews(0, new[] { new UnorderedAccessView(1024, 1024) });
			
		//	// Act.
		//	vm.Execute();

		//	// Assert.
		//}

		[Test]
		public void CanExecutePixelShader()
		{
			// Arrange.
			var vm = new VirtualMachine(DxbcContainer.Parse(File.ReadAllBytes("Shaders/PS/Simple.o")), 1);

			// Act.
			vm.Execute();

			// Assert.
			Assert.That(vm.GlobalMemory.Outputs[0].Number0.Float, Is.EqualTo(1.0f));
			Assert.That(vm.GlobalMemory.Outputs[0].Number1.Float, Is.EqualTo(0.5f));
			Assert.That(vm.GlobalMemory.Outputs[0].Number2.Float, Is.EqualTo(0.4f));
			Assert.That(vm.GlobalMemory.Outputs[0].Number3.Float, Is.EqualTo(1.0f));
		}

		[Test]
		public void CanExecuteFxDisPixelShader()
		{
			// Arrange.
			var vm = new VirtualMachine(DxbcContainer.Parse(File.ReadAllBytes("../../../../../shaders/FxDis/test.o")), 1);

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