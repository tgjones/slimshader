using SharpDX;

namespace SlimShader.VirtualMachine.Tests.Util
{
    public static class ConversionExtensions
    {
        public static Number4 ToNumber4(this Vector4 v)
        {
            return new Number4(v.X, v.Y, v.Z, v.W);
        }

        public static Number4 ToNumber4(this Vector3 v)
        {
            return new Number4(v.X, v.Y, v.Z, 0);
        }

        public static Number4 ToNumber4(this Vector2 v)
        {
            return new Number4(v.X, v.Y, 0, 0);
        }
    }
}