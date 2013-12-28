using NUnit.Framework;
using SlimShader.VirtualMachine.Resources;

namespace SlimShader.VirtualMachine.Tests.Resources
{
	[TestFixture]
	public class CubeMapUtilityTests
	{
		[TestCase(1.0f, 0.0f, 0.0f, CubeMapUtility.PositiveX, 0.5f, 0.5f)]
		[TestCase(-1.0f, 0.0f, 0.0f, CubeMapUtility.NegativeX, 0.5f, 0.5f)]
		[TestCase(0.0f, 1.0f, 0.0f, CubeMapUtility.PositiveY, 0.5f, 0.5f)]
		[TestCase(0.0f, -1.0f, 0.0f, CubeMapUtility.NegativeY, 0.5f, 0.5f)]
		[TestCase(0.0f, 0.0f, 1.0f, CubeMapUtility.PositiveZ, 0.5f, 0.5f)]
		[TestCase(0.0f, 0.0f, -1.0f, CubeMapUtility.NegativeZ, 0.5f, 0.5f)]
		[TestCase(1.0f, 1.0f, 1.0f, CubeMapUtility.PositiveZ, 1.0f, 0.0f)]
		[TestCase(0.5f, 1.0f, -0.3f, CubeMapUtility.PositiveY, 0.75f, 0.350000024f)]
		public void GetCubeMapCoordinatesWorksCorrectly(
			float x, float y, float z,
			int expectedArrayIndex,
			float expectedS, float expectedT)
		{
			// Act.
			var location = new Number4(x, y, z, 0.0f);
			int arrayIndex; Number4 coordinates;
			CubeMapUtility.GetCubeMapCoordinates(ref location,
				out arrayIndex, out coordinates);

			// Assert.
			Assert.That(arrayIndex, Is.EqualTo(expectedArrayIndex));
			Assert.That(coordinates.X, Is.EqualTo(expectedS));
			Assert.That(coordinates.Y, Is.EqualTo(expectedT));
		}
	}
}