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
			var vm = new VirtualMachine(DxbcContainer.Parse(File.ReadAllBytes("Shaders/PS/Simple.o")));

			// Act.
			vm.Execute();

			// Assert.
			Assert.That(vm.OutputRegisters[0].Values[0].AsFloat, Is.EqualTo(1.0f));
			Assert.That(vm.OutputRegisters[0].Values[1].AsFloat, Is.EqualTo(0.5f));
			Assert.That(vm.OutputRegisters[0].Values[2].AsFloat, Is.EqualTo(0.4f));
			Assert.That(vm.OutputRegisters[0].Values[3].AsFloat, Is.EqualTo(1.0f));
		}
	}
}