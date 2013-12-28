using System;

namespace SlimShader.VirtualMachine.Resources
{
	// For reference, see http://msdn.microsoft.com/en-us/library/windows/desktop/bb204881(v=vs.85).aspx.
	internal static class CubeMapUtility
	{
		public const int PositiveX = 0;
		public const int NegativeX = 1;
		public const int PositiveY = 2;
		public const int NegativeY = 3;
		public const int PositiveZ = 4;
		public const int NegativeZ = 5;

		/// <summary>
		/// Converts a 3D vector to an array index and 2D texture coordinates.
		/// </summary>
		public static void GetCubeMapCoordinates(ref Number4 location,
			out int arrayIndex, out Number4 coordinates)
		{
			var absX = Math.Abs(location.X);
			var absY = Math.Abs(location.Y);
			var absZ = Math.Abs(location.Z);

			// Select cube face using greatest magnitude component.
			// Then divide other components by that component,
			// and scale to [0..1].
			coordinates = new Number4();
			if (absX > absY && absX > absZ)
			{
				if (location.X > 0)
				{
					arrayIndex = PositiveX;
					coordinates.X = ScaleTexCoord(-location.Z, absX);
					coordinates.Y = 1.0f - ScaleTexCoord(location.Y, absX);
				}
				else
				{
					arrayIndex = NegativeX;
					coordinates.X = ScaleTexCoord(location.Z, absX);
					coordinates.Y = 1.0f - ScaleTexCoord(location.Y, absX);
				}
			}
			else if (absY > absX && absY > absZ)
			{
				if (location.Y > 0)
				{
					arrayIndex = PositiveY;
					coordinates.X = ScaleTexCoord(location.X, absY);
					coordinates.Y = 1.0f - ScaleTexCoord(-location.Z, absY);
				}
				else
				{
					arrayIndex = NegativeY;
					coordinates.X = ScaleTexCoord(location.X, absY);
					coordinates.Y = 1.0f - ScaleTexCoord(location.Z, absY);
				}
			}
			else
			{
				if (location.Z > 0)
				{
					arrayIndex = PositiveZ;
					coordinates.X = ScaleTexCoord(location.X, absZ);
					coordinates.Y = 1.0f - ScaleTexCoord(location.Y, absZ);
				}
				else
				{
					arrayIndex = NegativeZ;
					coordinates.X = ScaleTexCoord(-location.X, absZ);
					coordinates.Y = 1.0f - ScaleTexCoord(location.Y, absZ);
				}
			}
		}

		private static float ScaleTexCoord(float coord, float scale)
		{
			return (coord / scale * 0.5f) + 0.5f;
		}
	}
}