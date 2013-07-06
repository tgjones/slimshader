using System;

namespace SlimShader.VirtualMachine.Execution
{
    public static class InstructionImplementations
    {
        private const uint TrueUInt = 0xFFFFFFFF;
        private const uint FalseUInt = 0x0000000;

        /// <summary>
        /// Component-wise add of 2 vectors.
        /// </summary>
        /// <param name="saturate">True to clamp the result to [0...1], otherwise false.</param>
        /// <param name="src0">The vector to add to src1.</param>
        /// <param name="src1">The vector to add to src0.</param>
        /// <returns>The result of the operation. dest = src0 + src1.</returns>
        public static Number4 Add(bool saturate, ref Number4 src0, ref Number4 src1)
        {
            return new Number4
            {
                Number0 = Number.FromFloat(src0.Number0.Float + src1.Number0.Float, saturate),
                Number1 = Number.FromFloat(src0.Number1.Float + src1.Number1.Float, saturate),
                Number2 = Number.FromFloat(src0.Number2.Float + src1.Number2.Float, saturate),
                Number3 = Number.FromFloat(src0.Number3.Float + src1.Number3.Float, saturate)
            };
        }

        /// <summary>
        /// Component-wise bitwise AND.
        /// </summary>
        /// <remarks>
        /// Component-wise logical AND of each pair of 32-bit values from
        /// <paramref name="src0"/> and <paramref name="src1"/>.
        /// </remarks>
        /// <param name="src0">The 32-bit value to AND with src1.</param>
        /// <param name="src1">The 32-bit value to AND with src0.</param>
        /// <returns>The result of the operation. dest = src0 & src1.</returns>
        public static Number4 And(ref Number4 src0, ref Number4 src1)
        {
            return new Number4
            {
                Number0 = Number.FromUInt(src0.Number0.UInt & src1.Number0.UInt),
                Number1 = Number.FromUInt(src0.Number1.UInt & src1.Number1.UInt),
                Number2 = Number.FromUInt(src0.Number2.UInt & src1.Number2.UInt),
                Number3 = Number.FromUInt(src0.Number3.UInt & src1.Number3.UInt)
            };
        }

        /// <summary>
        /// Component-wise divide.
        /// </summary>
        /// <param name="saturate">True to clamp the result to [0...1], otherwise false.</param>
        /// <param name="src0">The dividend.</param>
        /// <param name="src1">The divisor.</param>
        /// <returns>The result of the operation. dest = src / src1.</returns>
        public static Number4 Div(bool saturate, ref Number4 src0, ref Number4 src1)
        {
            return new Number4
            {
                Number0 = Number.FromFloat(src0.Number0.Float / src1.Number0.Float, saturate),
                Number1 = Number.FromFloat(src0.Number1.Float / src1.Number1.Float, saturate),
                Number2 = Number.FromFloat(src0.Number2.Float / src1.Number2.Float, saturate),
                Number3 = Number.FromFloat(src0.Number3.Float / src1.Number3.Float, saturate)
            };
        }

        /// <summary>
        /// 2-dimensional vector dot-product of components rg, POS-swizzle.
        /// </summary>
        /// <param name="saturate">True to clamp the result to [0...1], otherwise false.</param>
        /// <param name="src0">The components in the operation.</param>
        /// <param name="src1">The components in the operation.</param>
        /// <returns>
        /// The result of the operation.
        /// <example>dest = src0.r * src1.r + src0.g * src1.g</example>
        /// </returns>
        public static Number Dp2(bool saturate, ref Number4 src0, ref Number4 src1)
        {
            return Number.FromFloat(
                src0.Number0.Float * src1.Number0.Float
                    + src0.Number1.Float * src1.Number1.Float,
                saturate);
        }

        /// <summary>
        /// 3-dimensional vector dot-product of components rgb, POS-swizzle.
        /// </summary>
        /// <param name="saturate">True to clamp the result to [0...1], otherwise false.</param>
        /// <param name="src0">The components in the operation.</param>
        /// <param name="src1">The components in the operation.</param>
        /// <returns>
        /// The result of the operation.
        /// <example>dest = src0.r * src1.r + src0.g * src1.g + src0.b * src1.b</example>
        /// </returns>
        public static Number Dp3(bool saturate, ref Number4 src0, ref Number4 src1)
        {
            return Number.FromFloat(
                src0.Number0.Float * src1.Number0.Float
                    + src0.Number1.Float * src1.Number1.Float
                    + src0.Number2.Float * src1.Number2.Float,
                saturate);
        }

        /// <summary>
        /// 4-dimensional vector dot-product of components rgba, POS-swizzle.
        /// </summary>
        /// <param name="saturate">True to clamp the result to [0...1], otherwise false.</param>
        /// <param name="src0">The components in the operation.</param>
        /// <param name="src1">The components in the operation.</param>
        /// <returns>
        /// The result of the operation.
        /// <example>dest = src0.r * src1.r + src0.g * src1.g + src0.b * src1.b + src0.a * src1.a</example>
        /// </returns>
        public static Number Dp4(bool saturate, ref Number4 src0, ref Number4 src1)
        {
            return Number.FromFloat(
                src0.Number0.Float * src1.Number0.Float
                    + src0.Number1.Float * src1.Number1.Float
                    + src0.Number2.Float * src1.Number2.Float
                    + src0.Number3.Float * src1.Number3.Float,
                saturate);
        }

        /// <summary>
        /// Component-wise vector floating point greater-than-or-equal comparison.
        /// </summary>
        /// <remarks>
        /// This instruction performs the float comparison (src0 &gt;= src1) for each component, 
        /// and writes the result to dest.
        /// If the comparison is true, then 0xFFFFFFFF is returned for that component. 
        /// Otherwise 0x0000000 is returned.
        /// </remarks>
        /// <param name="src0">The component to compare to src1.</param>
        /// <param name="src1">The component to compare to src0.</param>
        /// <returns>The result of the operation.</returns>
        public static Number4 Ge(ref Number4 src0, ref Number4 src1)
        {
            return new Number4
            {
                Number0 = Number.FromUInt((src0.Number0.Float >= src1.Number0.Float) ? TrueUInt : FalseUInt),
                Number1 = Number.FromUInt((src0.Number1.Float >= src1.Number1.Float) ? TrueUInt : FalseUInt),
                Number2 = Number.FromUInt((src0.Number2.Float >= src1.Number2.Float) ? TrueUInt : FalseUInt),
                Number3 = Number.FromUInt((src0.Number3.Float >= src1.Number3.Float) ? TrueUInt : FalseUInt),
            };
        }

        /// <summary>
        /// Component-wise vector integer less-than comparison.
        /// </summary>
        /// <remarks>
        /// This instruction performs the integer comparison (src0 &lt; src1) for each component, 
        /// and writes the result to dest.
        /// If the comparison is true, then 0xFFFFFFFF is returned for that component. 
        /// Otherwise 0x0000000 is returned.
        /// </remarks>
        /// <param name="src0">The value to compare to src1.</param>
        /// <param name="src1">The value to compare to src0.</param>
        /// <returns>The result of the operation.</returns>
        public static Number4 Ilt(ref Number4 src0, ref Number4 src1)
        {
            return new Number4
            {
                Number0 = Number.FromUInt((src0.Number0.Int < src1.Number0.Int) ? TrueUInt : FalseUInt),
                Number1 = Number.FromUInt((src0.Number1.Int < src1.Number1.Int) ? TrueUInt : FalseUInt),
                Number2 = Number.FromUInt((src0.Number2.Int < src1.Number2.Int) ? TrueUInt : FalseUInt),
                Number3 = Number.FromUInt((src0.Number3.Int < src1.Number3.Int) ? TrueUInt : FalseUInt),
            };
        }

        /// <summary>
        /// Component-wise vector floating point less-than comparison.
        /// </summary>
        /// <remarks>
        /// This instruction performs the float comparison (src0 &lt; src1) for each component, 
        /// and writes the result to dest.
        /// If the comparison is true, then 0xFFFFFFFF is returned for that component. 
        /// Otherwise 0x0000000 is returned.
        /// </remarks>
        /// <param name="src0">The value to compare to src1.</param>
        /// <param name="src1">The value to compare to src0.</param>
        /// <returns>The result of the operation.</returns>
        public static Number4 Lt(ref Number4 src0, ref Number4 src1)
        {
            return new Number4
            {
                Number0 = Number.FromUInt((src0.Number0.Float < src1.Number0.Float) ? TrueUInt : FalseUInt),
                Number1 = Number.FromUInt((src0.Number1.Float < src1.Number1.Float) ? TrueUInt : FalseUInt),
                Number2 = Number.FromUInt((src0.Number2.Float < src1.Number2.Float) ? TrueUInt : FalseUInt),
                Number3 = Number.FromUInt((src0.Number3.Float < src1.Number3.Float) ? TrueUInt : FalseUInt),
            };
        }

        /// <summary>
        /// Component-wise float maximum.
        /// </summary>
        /// <remarks>
        /// &gt;= is used instead of &gt; so that if min(x,y) = x then max(x,y) = y.
        /// </remarks>
        /// <param name="saturate">True to clamp the result to [0...1], otherwise false.</param>
        /// <param name="src0">The components to compare to src1.</param>
        /// <param name="src1">The components to compare to src0.</param>
        /// <returns>The result of the operation. dest = src0 &gt;= src1 ? src0 : src1.</returns>
        public static Number4 Max(bool saturate, ref Number4 src0, ref Number4 src1)
        {
            return new Number4
            {
                Number0 = Number.FromFloat(Max(src0.Number0.Float, src1.Number0.Float), saturate),
                Number1 = Number.FromFloat(Max(src0.Number1.Float, src1.Number1.Float), saturate),
                Number2 = Number.FromFloat(Max(src0.Number2.Float, src1.Number2.Float), saturate),
                Number3 = Number.FromFloat(Max(src0.Number3.Float, src1.Number3.Float), saturate)
            };
        }

        private static float Max(float src0, float src1)
        {
            return (src0 >= src1) ? src0 : src1;
        }

        /// <summary>
        /// Component-wise float minimum.
        /// </summary>
        /// <remarks>
        /// &gt;= is used instead of &gt; so that if min(x,y) = x then max(x,y) = y.
        /// </remarks>
        /// <param name="saturate">True to clamp the result to [0...1], otherwise false.</param>
        /// <param name="src0">The components to compare to src1.</param>
        /// <param name="src1">The components to compare to src0.</param>
        /// <returns>The result of the operation. dest = src0 &lt; src1 ? src0 : src1.</returns>
        public static Number4 Min(bool saturate, ref Number4 src0, ref Number4 src1)
        {
            return new Number4
            {
                Number0 = Number.FromFloat(Min(src0.Number0.Float, src1.Number0.Float), saturate),
                Number1 = Number.FromFloat(Min(src0.Number1.Float, src1.Number1.Float), saturate),
                Number2 = Number.FromFloat(Min(src0.Number2.Float, src1.Number2.Float), saturate),
                Number3 = Number.FromFloat(Min(src0.Number3.Float, src1.Number3.Float), saturate)
            };
        }

        private static float Min(float src0, float src1)
        {
            return (src0 < src1) ? src0 : src1;
        }

        /// <summary>
        /// Component-wise move.
        /// </summary>
        /// <remarks>
        /// The modifiers, other than swizzle, assume the data is floating point.
        /// The absence of modifiers just moves data without altering bits.
        /// </remarks>
        /// <param name="saturate">True to clamp the result to [0...1], otherwise false.</param>
        /// <param name="src0">The components to move.</param>
        /// <returns>The result of the operation.</returns>
        public static Number4 Mov(bool saturate, ref Number4 src0)
        {
            if (saturate)
                return new Number4
                {
                    Number0 = Number.FromFloat(src0.Number0.Float, true),
                    Number1 = Number.FromFloat(src0.Number0.Float, true),
                    Number2 = Number.FromFloat(src0.Number0.Float, true),
                    Number3 = Number.FromFloat(src0.Number0.Float, true)
                };

            return new Number4
            {
                Number0 = src0.Number0,
                Number1 = src0.Number1,
                Number2 = src0.Number2,
                Number3 = src0.Number3
            };
        }

        /// <summary>
        /// Component-wise multiply.
        /// </summary>
        /// <param name="saturate">True to clamp the result to [0...1], otherwise false.</param>
        /// <param name="src0">The multiplicand.</param>
        /// <param name="src1">The multiplier.</param>
        /// <returns>The result of the operation. dest = src0 * src1.</returns>
        public static Number4 Mul(bool saturate, ref Number4 src0, ref Number4 src1)
        {
            return new Number4
            {
                Number0 = Number.FromFloat(src0.Number0.Float * src1.Number0.Float, saturate),
                Number1 = Number.FromFloat(src0.Number1.Float * src1.Number1.Float, saturate),
                Number2 = Number.FromFloat(src0.Number2.Float * src1.Number2.Float, saturate),
                Number3 = Number.FromFloat(src0.Number3.Float * src1.Number3.Float, saturate)
            };
        }

        /// <summary>
        /// Component-wise reciprocal square root.
        /// </summary>
        /// <param name="saturate">True to clamp the result to [0...1], otherwise false.</param>
        /// <param name="src0">The components for the operation.</param>
        /// <returns>The results of the operation. dest = 1.0f / sqrt(src0).</returns>
        public static Number4 Rsq(bool saturate, ref Number4 src0)
        {
            return new Number4
            {
                Number0 = Number.FromFloat((float) (1.0f / Math.Sqrt(src0.Number0.Float)), saturate),
                Number1 = Number.FromFloat((float) (1.0f / Math.Sqrt(src0.Number1.Float)), saturate),
                Number2 = Number.FromFloat((float) (1.0f / Math.Sqrt(src0.Number2.Float)), saturate),
                Number3 = Number.FromFloat((float) (1.0f / Math.Sqrt(src0.Number3.Float)), saturate)
            };
        }

        /// <summary>
        /// Component-wise square root.
        /// </summary>
        /// <param name="saturate">True to clamp the result to [0...1], otherwise false.</param>
        /// <param name="src0">The components for which to take the square root.</param>
        /// <returns>The results of the operation. dest = sqrt(src0).</returns>
        public static Number4 Sqrt(bool saturate, ref Number4 src0)
        {
            return new Number4
            {
                Number0 = Number.FromFloat((float) Math.Sqrt(src0.Number0.Float), saturate),
                Number1 = Number.FromFloat((float) Math.Sqrt(src0.Number1.Float), saturate),
                Number2 = Number.FromFloat((float) Math.Sqrt(src0.Number2.Float), saturate),
                Number3 = Number.FromFloat((float) Math.Sqrt(src0.Number3.Float), saturate)
            };
        }

        /// <summary>
        /// Component-wise unsigned integer to floating point conversion.
        /// </summary>
        /// <remarks>
        /// <paramref name="src0"/> must contain an unsigned 32-bit integer 4-tuple. 
        /// After the instruction executes, dest will contain a floating-point 4-tuple. 
        /// The conversion is performed per-component.
        /// TODO: When an integer input value is too large to be represented exactly in 
        /// the floating point format, round to nearest even mode.
        /// </remarks>
        /// <param name="src0">The components to convert.</param>
        /// <returns>The result of the operation.</returns>
        public static Number4 Utof(ref Number4 src0)
        {
            return new Number4
            {
                Number0 = Number.FromFloat(Convert.ToSingle(src0.Number0.UInt)),
                Number1 = Number.FromFloat(Convert.ToSingle(src0.Number1.UInt)),
                Number2 = Number.FromFloat(Convert.ToSingle(src0.Number2.UInt)),
                Number3 = Number.FromFloat(Convert.ToSingle(src0.Number3.UInt))
            };
        }

        /// <summary>
        /// Component-wise bitwise XOR.
        /// </summary>
        /// <param name="src0">The components to XOR with src1.</param>
        /// <param name="src1">The components to XOR with src0.</param>
        /// <returns>The result of the operation. dest = src0 | src1.</returns>
        public static Number4 Xor(ref Number4 src0, ref Number4 src1)
        {
            return new Number4
            {
                Number0 = Number.FromUInt(src0.Number0.UInt | src1.Number0.UInt),
                Number1 = Number.FromUInt(src0.Number1.UInt | src1.Number1.UInt),
                Number2 = Number.FromUInt(src0.Number2.UInt | src1.Number2.UInt),
                Number3 = Number.FromUInt(src0.Number3.UInt | src1.Number3.UInt)
            };
        }
    }
}