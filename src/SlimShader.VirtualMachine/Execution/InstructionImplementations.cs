using System;
using SlimShader.Chunks.Shex;

namespace SlimShader.VirtualMachine.Execution
{
    /// <summary>
    /// Contains implementations of Shader Model 5 assembly instructions.
    /// </summary>
    /// <remarks>
    /// XML documentation comments are from MSDN (Shader Model 5 Assembly):
    /// http://msdn.microsoft.com/en-us/library/windows/desktop/hh447232(v=vs.85).aspx
    /// </remarks>
    public static class InstructionImplementations
    {
        private static Number GetCompareResult(bool result)
        {
            return Number.FromUInt(result ? 0xFFFFFFFF : 0x0000000);
        }

        private static bool TestCondition(ref Number4 number, InstructionTestBoolean testBoolean)
        {
            switch (testBoolean)
            {
                case InstructionTestBoolean.Zero:
                    return number.AllZero;
                case InstructionTestBoolean.NonZero:
                    return number.AnyNonZero;
                default:
                    throw new ArgumentOutOfRangeException("testBoolean");
            }
        }

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
        /// Component-wise floating point equality comparison.
        /// </summary>
        /// <remarks>
        /// Performs the integer comparison (src0 == src1) for each component, and writes the result to dest.
        /// If the comparison is true, then 0xFFFFFFFF is returned for that component. 
        /// Otherwise 0x0000000 is returned.
        /// </remarks>
        /// <param name="src0">The value to compare to src1.</param>
        /// <param name="src1">The value to compare to src0.</param>
        /// <returns>The result of the operation.</returns>
        public static Number4 Eq(ref Number4 src0, ref Number4 src1)
        {
            return new Number4
            {
                // ReSharper disable CompareOfFloatsByEqualityOperator
                Number0 = GetCompareResult(src0.Number0.Float == src1.Number0.Float),
                Number1 = GetCompareResult(src0.Number1.Float == src1.Number1.Float),
                Number2 = GetCompareResult(src0.Number2.Float == src1.Number2.Float),
                Number3 = GetCompareResult(src0.Number3.Float == src1.Number3.Float),
                // ReSharper restore CompareOfFloatsByEqualityOperator
            };
        }

        /// <summary>
        /// Component-wise 2-exponent.
        /// </summary>
        /// <param name="saturate">True to clamp the result to [0...1], otherwise false.</param>
        /// <param name="src0">The exponent.</param>
        /// <returns>The result of the operation. dest = 2^src0</returns>
        public static Number4 Exp(bool saturate, ref Number4 src0)
        {
            return new Number4
            {
                Number0 = Number.FromFloat((float) Math.Pow(2, src0.Number0.Float), saturate),
                Number1 = Number.FromFloat((float) Math.Pow(2, src0.Number1.Float), saturate),
                Number2 = Number.FromFloat((float) Math.Pow(2, src0.Number2.Float), saturate),
                Number3 = Number.FromFloat((float) Math.Pow(2, src0.Number3.Float), saturate)
            };
        }

        /// <summary>
        /// Component-wise signed floating point to integer conversion.
        /// </summary>
        /// <remarks>
        /// The conversion is performed per-component. Rounding is always performed towards zero, 
        /// following the C convention for casts from float to int.
        /// 
        /// Applications that require different 
        /// rounding semantics can invoke the round instructions before casting to integer.
        /// 
        /// Inputs are clamped to the range [-2147483648.999f ... 2147483647.999f] prior to conversion, 
        /// and input NaN values produce a zero result.
        /// </remarks>
        /// <param name="src0">The components to convert.</param>
        /// <returns>The result of the operation.</returns>
        public static Number4 FtoI(ref Number4 src0)
        {
            return new Number4
            {
                Number0 = Number.FromInt(Convert.ToInt32(src0.Number0.Float)),
                Number1 = Number.FromInt(Convert.ToInt32(src0.Number1.Float)),
                Number2 = Number.FromInt(Convert.ToInt32(src0.Number2.Float)),
                Number3 = Number.FromInt(Convert.ToInt32(src0.Number3.Float))
            };
        }

        /// <summary>
        /// Component-wise floating point to unsigned integer conversion.
        /// </summary>
        /// <remarks>
        /// The conversion is performed per-component. Rounding is always performed towards zero, 
        /// following the C convention for casts from float to int.
        /// 
        /// Applications that require different 
        /// rounding semantics can invoke the round instructions before casting to integer.
        /// 
        /// Inputs are clamped to the range [-2147483648.999f ... 2147483647.999f] prior to conversion, 
        /// and input NaN values produce a zero result.
        /// </remarks>
        /// <param name="src0">The components to convert.</param>
        /// <returns>The result of the operation.</returns>
        public static Number4 FtoU(ref Number4 src0)
        {
            return new Number4
            {
                Number0 = Number.FromUInt(Convert.ToUInt32(src0.Number0.Float)),
                Number1 = Number.FromUInt(Convert.ToUInt32(src0.Number1.Float)),
                Number2 = Number.FromUInt(Convert.ToUInt32(src0.Number2.Float)),
                Number3 = Number.FromUInt(Convert.ToUInt32(src0.Number3.Float))
            };
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
                Number0 = GetCompareResult(src0.Number0.Float >= src1.Number0.Float),
                Number1 = GetCompareResult(src0.Number1.Float >= src1.Number1.Float),
                Number2 = GetCompareResult(src0.Number2.Float >= src1.Number2.Float),
                Number3 = GetCompareResult(src0.Number3.Float >= src1.Number3.Float),
            };
        }

        /// <summary>
        /// Component-wise integer add of 2 vectors.
        /// </summary>
        /// <param name="src0">The vector to add to src1.</param>
        /// <param name="src1">The vector to add to src0.</param>
        /// <returns>The result of the operation. dest = src0 + src1.</returns>
        public static Number4 IAdd(ref Number4 src0, ref Number4 src1)
        {
            return new Number4
            {
                Number0 = Number.FromInt(src0.Number0.Int + src1.Number0.Int),
                Number1 = Number.FromInt(src0.Number1.Int + src1.Number1.Int),
                Number2 = Number.FromInt(src0.Number2.Int + src1.Number2.Int),
                Number3 = Number.FromInt(src0.Number3.Int + src1.Number3.Int)
            };
        }

        /// <summary>
        /// Component-wise vector integer equality comparison.
        /// </summary>
        /// <remarks>
        /// Performs the integer comparison (src0 == src1) for each component, and writes the result to dest.
        /// If the comparison is true, then 0xFFFFFFFF is returned for that component. 
        /// Otherwise 0x0000000 is returned.
        /// </remarks>
        /// <param name="src0">The value to compare to src1.</param>
        /// <param name="src1">The value to compare to src0.</param>
        /// <returns>The result of the operation.</returns>
        public static Number4 IEq(ref Number4 src0, ref Number4 src1)
        {
            return new Number4
            {
                Number0 = GetCompareResult(src0.Number0.Int == src1.Number0.Int),
                Number1 = GetCompareResult(src0.Number1.Int == src1.Number1.Int),
                Number2 = GetCompareResult(src0.Number2.Int == src1.Number2.Int),
                Number3 = GetCompareResult(src0.Number3.Int == src1.Number3.Int),
            };
        }

        /// <summary>
        /// Component-wise vector integer greater-than-or-equal comparison.
        /// </summary>
        /// <remarks>
        /// This instruction performs the integer comparison (src0 &gt;= src1) for each component, 
        /// and writes the result to dest.
        /// If the comparison is true, then 0xFFFFFFFF is returned for that component. 
        /// Otherwise 0x0000000 is returned.
        /// </remarks>
        /// <param name="src0">The value to compare to src1.</param>
        /// <param name="src1">The value to compare to src0.</param>
        /// <returns>The result of the operation.</returns>
        public static Number4 IGe(ref Number4 src0, ref Number4 src1)
        {
            return new Number4
            {
                Number0 = GetCompareResult(src0.Number0.Int >= src1.Number0.Int),
                Number1 = GetCompareResult(src0.Number1.Int >= src1.Number1.Int),
                Number2 = GetCompareResult(src0.Number2.Int >= src1.Number2.Int),
                Number3 = GetCompareResult(src0.Number3.Int >= src1.Number3.Int),
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
        public static Number4 ILt(ref Number4 src0, ref Number4 src1)
        {
            return new Number4
            {
                Number0 = GetCompareResult(src0.Number0.Int < src1.Number0.Int),
                Number1 = GetCompareResult(src0.Number1.Int < src1.Number1.Int),
                Number2 = GetCompareResult(src0.Number2.Int < src1.Number2.Int),
                Number3 = GetCompareResult(src0.Number3.Int < src1.Number3.Int),
            };
        }

        /// <summary>
        /// Component-wise signed integer multiply and add.
        /// </summary>
        /// <param name="src0">The multiplicand.</param>
        /// <param name="src1">The multiplier.</param>
        /// <param name="src2">The addend.</param>
        /// <returns>The result of the operation. dest = src0 * src1 + src2.</returns>
        public static Number4 IMad(ref Number4 src0, ref Number4 src1, ref Number4 src2)
        {
            return new Number4
            {
                Number0 = Number.FromInt((src0.Number0.Int * src1.Number0.Int) + src2.Number0.Int),
                Number1 = Number.FromInt((src0.Number1.Int * src1.Number0.Int) + src2.Number1.Int),
                Number2 = Number.FromInt((src0.Number2.Int * src1.Number0.Int) + src2.Number2.Int),
                Number3 = Number.FromInt((src0.Number3.Int * src1.Number0.Int) + src2.Number3.Int)
            };
        }

        /// <summary>
        /// Component-wise integer minimum.
        /// </summary>
        /// <param name="saturate">True to clamp the result to [0...1], otherwise false.</param>
        /// <param name="src0">The components to compare to src1.</param>
        /// <param name="src1">The components to compare to src0.</param>
        /// <returns>The result of the operation. dest = src0 &lt; src1 ? src0 : src1.</returns>
        public static Number4 IMin(bool saturate, ref Number4 src0, ref Number4 src1)
        {
            return new Number4
            {
                Number0 = Number.FromInt(IMin(src0.Number0.Int, src1.Number0.Int)),
                Number1 = Number.FromInt(IMin(src0.Number1.Int, src1.Number1.Int)),
                Number2 = Number.FromInt(IMin(src0.Number2.Int, src1.Number2.Int)),
                Number3 = Number.FromInt(IMin(src0.Number3.Int, src1.Number3.Int))
            };
        }

        private static int IMin(int src0, int src1)
        {
            return (src0 < src1) ? src0 : src1;
        }

        /// <summary>
        /// Component-wise vector integer not-equal comparison.
        /// </summary>
        /// <remarks>
        /// Performs the integer comparison (src0 != src1) for each component, and writes the result to dest.
        /// If the comparison is true, then 0xFFFFFFFF is returned for that component. 
        /// Otherwise 0x0000000 is returned.
        /// </remarks>
        /// <param name="src0">The value to compare to src1.</param>
        /// <param name="src1">The value to compare to src0.</param>
        /// <returns>The result of the operation.</returns>
        public static Number4 INe(ref Number4 src0, ref Number4 src1)
        {
            return new Number4
            {
                Number0 = GetCompareResult(src0.Number0.Int != src1.Number0.Int),
                Number1 = GetCompareResult(src0.Number1.Int != src1.Number1.Int),
                Number2 = GetCompareResult(src0.Number2.Int != src1.Number2.Int),
                Number3 = GetCompareResult(src0.Number3.Int != src1.Number3.Int),
            };
        }

        /// <summary>
        /// Component-wise integer 2's complement.
        /// </summary>
        /// <remarks>
        /// This instruction performs component-wise 2's complement of each 32-bit value in src0. 
        /// The 32-bit results are stored in dest.
        /// </remarks>
        /// <param name="src0">Contains the values for the operation.</param>
        /// <returns>The result of the operation.</returns>
        public static Number4 INeg(ref Number4 src0)
        {
            return new Number4
            {
                Number0 = Number.FromInt(-src0.Number0.Int),
                Number1 = Number.FromInt(-src0.Number1.Int),
                Number2 = Number.FromInt(-src0.Number2.Int),
                Number3 = Number.FromInt(-src0.Number3.Int)
            };
        }

        /// <summary>
        /// Component-wise shift left.
        /// </summary>
        /// <remarks>
        /// This instruction performs a component-wise shift of each 32-bit value in src0 
        /// left by an unsigned integer bit count provided by the LSB 5 bits (0-31 range) 
        /// in src1.select_component, inserting 0. The 32-bit per component results are 
        /// placed in dest. The count is a scalar value applied to all components.
        /// TODO: Check where shift amount comes from.
        /// </remarks>
        /// <param name="src0">Contains the values to be shifted.</param>
        /// <param name="src1">Contains the shift amount.</param>
        /// <returns>The result of the operation.</returns>
        public static Number4 IShl(ref Number4 src0, ref Number4 src1)
        {
            return new Number4
            {
                Number0 = Number.FromInt(src0.Number0.Int << (int) src1.Number0.UInt),
                Number1 = Number.FromInt(src0.Number1.Int << (int) src1.Number0.UInt),
                Number2 = Number.FromInt(src0.Number2.Int << (int) src1.Number0.UInt),
                Number3 = Number.FromInt(src0.Number3.Int << (int) src1.Number0.UInt)
            };
        }

        /// <summary>
        /// Component-wise shift right.
        /// </summary>
        /// <remarks>
        /// This instruction performs a component-wise shift of each 32-bit value in src0 
        /// right by an unsigned integer bit count provided by the LSB 5 bits (0-31 range) 
        /// in src1.select_component, inserting 0. The 32-bit per component results are 
        /// placed in dest. The count is a scalar value applied to all components.
        /// TODO: Check where shift amount comes from.
        /// </remarks>
        /// <param name="src0">Contains the values to be shifted.</param>
        /// <param name="src1">Contains the shift amount.</param>
        /// <returns>The result of the operation.</returns>
        public static Number4 IShr(ref Number4 src0, ref Number4 src1)
        {
            return new Number4
            {
                Number0 = Number.FromInt(src0.Number0.Int >> (int) src1.Number0.UInt),
                Number1 = Number.FromInt(src0.Number1.Int >> (int) src1.Number0.UInt),
                Number2 = Number.FromInt(src0.Number2.Int >> (int) src1.Number0.UInt),
                Number3 = Number.FromInt(src0.Number3.Int >> (int) src1.Number0.UInt)
            };
        }

        /// <summary>
        /// Component-wise signed integer to floating point conversion.
        /// </summary>
        /// <remarks>
        /// This signed integer-to-float conversion instruction assumes that src0 
        /// contains a signed 32-bit integer 4-tuple. After the instruction executes, 
        /// dest will contain a floating-point 4-tuple.
        /// The conversion is performed per-component.
        /// When an integer input value is too large in magnitude to be represented exactly 
        /// in the floating point format, rounding to nearest even mode is strongly 
        /// recommended but not required.
        /// </remarks>
        /// <param name="src0">The components to convert.</param>
        /// <returns>The result of the operation.</returns>
        public static Number4 ItoF(ref Number4 src0)
        {
            return new Number4
            {
                Number0 = Number.FromFloat(Convert.ToSingle(src0.Number0.Int)),
                Number1 = Number.FromFloat(Convert.ToSingle(src0.Number1.Int)),
                Number2 = Number.FromFloat(Convert.ToSingle(src0.Number2.Int)),
                Number3 = Number.FromFloat(Convert.ToSingle(src0.Number3.Int))
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
                Number0 = GetCompareResult(src0.Number0.Float < src1.Number0.Float),
                Number1 = GetCompareResult(src0.Number1.Float < src1.Number1.Float),
                Number2 = GetCompareResult(src0.Number2.Float < src1.Number2.Float),
                Number3 = GetCompareResult(src0.Number3.Float < src1.Number3.Float),
            };
        }

        /// <summary>
        /// Component-wise multiply and add.
        /// </summary>
        /// <param name="saturate">True to clamp the result to [0...1], otherwise false.</param>
        /// <param name="src0">The multiplicand.</param>
        /// <param name="src1">The multiplier.</param>
        /// <param name="src2">The addend.</param>
        /// <returns>The result of the operation. dest = src0 * src1 + src2.</returns>
        public static Number4 Mad(bool saturate, ref Number4 src0, ref Number4 src1, ref Number4 src2)
        {
            return new Number4
            {
                Number0 = Number.FromFloat((src0.Number0.Float * src1.Number0.Float) + src2.Number0.Float, saturate),
                Number1 = Number.FromFloat((src0.Number1.Float * src1.Number0.Float) + src2.Number1.Float, saturate),
                Number2 = Number.FromFloat((src0.Number2.Float * src1.Number0.Float) + src2.Number2.Float, saturate),
                Number3 = Number.FromFloat((src0.Number3.Float * src1.Number0.Float) + src2.Number3.Float, saturate)
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
                return Number4.Saturate(ref src0);

            return new Number4
            {
                Number0 = src0.Number0,
                Number1 = src0.Number1,
                Number2 = src0.Number2,
                Number3 = src0.Number3
            };
        }

        /// <summary>
        /// Component-wise conditional move.
        /// </summary>
        /// <remarks>
        /// The modifiers, other than swizzle, assume the data is floating point.
        /// The absence of modifiers just moves data without altering bits.
        /// </remarks>
        /// <param name="saturate">True to clamp the result to [0...1], otherwise false.</param>
        /// <param name="src0">The components on which to test the condition.</param>
        /// <param name="src1">The components to move.</param>
        /// <param name="src2">The components to move.</param>
        /// <returns>The result of the operation. If src0, then dest = src1 else dest = src2.</returns>
        public static Number4 MovC(bool saturate, ref Number4 src0, ref Number4 src1, ref Number4 src2)
        {
            var result = TestCondition(ref src0, InstructionTestBoolean.NonZero);
            if (saturate)
                return (result) ? Number4.Saturate(ref src1) : Number4.Saturate(ref src2);

            return (result) ? src1 : src2;
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
        /// Component-wise floating point not-equal comparison.
        /// </summary>
        /// <remarks>
        /// Performs the integer comparison (src0 != src1) for each component, and writes the result to dest.
        /// If the comparison is true, then 0xFFFFFFFF is returned for that component. 
        /// Otherwise 0x0000000 is returned.
        /// </remarks>
        /// <param name="src0">The value to compare to src1.</param>
        /// <param name="src1">The value to compare to src0.</param>
        /// <returns>The result of the operation.</returns>
        public static Number4 Ne(ref Number4 src0, ref Number4 src1)
        {
            return new Number4
            {
                // ReSharper disable CompareOfFloatsByEqualityOperator
                Number0 = GetCompareResult(src0.Number0.Float != src1.Number0.Float),
                Number1 = GetCompareResult(src0.Number1.Float != src1.Number1.Float),
                Number2 = GetCompareResult(src0.Number2.Float != src1.Number2.Float),
                Number3 = GetCompareResult(src0.Number3.Float != src1.Number3.Float),
                // ReSharper restore CompareOfFloatsByEqualityOperator
            };
        }

        /// <summary>
        /// Floating-point round to integral float.
        /// </summary>
        /// <remarks>
        /// This instruction performs a component-wise floating-point round of the values in src0, 
        /// writing integral floating-point values to dest. round_ne rounds toward nearest even.
        /// </remarks>
        /// <param name="saturate">True to clamp the result to [0...1], otherwise false.</param>
        /// <param name="src0">The components in the operation.</param>
        /// <returns>The result of the operation.</returns>
        public static Number4 RoundNe(bool saturate, ref Number4 src0)
        {
            return new Number4
            {
                Number0 = Number.FromFloat(RoundNe(src0.Number0.Float), saturate),
                Number1 = Number.FromFloat(RoundNe(src0.Number1.Float), saturate),
                Number2 = Number.FromFloat(RoundNe(src0.Number2.Float), saturate),
                Number3 = Number.FromFloat(RoundNe(src0.Number3.Float), saturate)
            };
        }

        private static float RoundNe(float value)
        {
            return (float) Math.Round(value, MidpointRounding.ToEven);
        }

        /// <summary>
        /// Floating-point round to integral float.
        /// </summary>
        /// <remarks>
        /// This instruction performs a component-wise floating-point round of the values in src0, 
        /// writing integral floating-point values to dest.
        /// 
        /// round_ni rounds toward -infinity, commonly known as floor().
        /// </remarks>
        /// <param name="saturate">True to clamp the result to [0...1], otherwise false.</param>
        /// <param name="src0">The components in the operation.</param>
        /// <returns>The result of the operation.</returns>
        public static Number4 RoundNi(bool saturate, ref Number4 src0)
        {
            return new Number4
            {
                Number0 = Number.FromFloat(RoundNi(src0.Number0.Float), saturate),
                Number1 = Number.FromFloat(RoundNi(src0.Number1.Float), saturate),
                Number2 = Number.FromFloat(RoundNi(src0.Number2.Float), saturate),
                Number3 = Number.FromFloat(RoundNi(src0.Number3.Float), saturate)
            };
        }

        private static float RoundNi(float value)
        {
            return (float) Math.Floor(value);
        }

        /// <summary>
        /// Floating-point round to integral float.
        /// </summary>
        /// <remarks>
        /// This instruction performs a component-wise floating-point round of the values in src0, 
        /// writing integral floating-point values to dest.
        /// 
        /// round_pi rounds toward +infinity, commonly known as ceil().
        /// </remarks>
        /// <param name="saturate">True to clamp the result to [0...1], otherwise false.</param>
        /// <param name="src0">The components in the operation.</param>
        /// <returns>The result of the operation.</returns>
        public static Number4 RoundPi(bool saturate, ref Number4 src0)
        {
            return new Number4
            {
                Number0 = Number.FromFloat(RoundPi(src0.Number0.Float), saturate),
                Number1 = Number.FromFloat(RoundPi(src0.Number1.Float), saturate),
                Number2 = Number.FromFloat(RoundPi(src0.Number2.Float), saturate),
                Number3 = Number.FromFloat(RoundPi(src0.Number3.Float), saturate)
            };
        }

        private static float RoundPi(float value)
        {
            return (float) Math.Ceiling(value);
        }

        /// <summary>
        /// Floating-point round to integral float.
        /// </summary>
        /// <remarks>
        /// This instruction performs a component-wise floating-point round of the values in src0, 
        /// writing integral floating-point values to dest.
        /// 
        /// round_z rounds toward zero.
        /// </remarks>
        /// <param name="saturate">True to clamp the result to [0...1], otherwise false.</param>
        /// <param name="src0">The components in the operation.</param>
        /// <returns>The result of the operation.</returns>
        public static Number4 RoundZ(bool saturate, ref Number4 src0)
        {
            return new Number4
            {
                Number0 = Number.FromFloat(RoundZ(src0.Number0.Float), saturate),
                Number1 = Number.FromFloat(RoundZ(src0.Number1.Float), saturate),
                Number2 = Number.FromFloat(RoundZ(src0.Number2.Float), saturate),
                Number3 = Number.FromFloat(RoundZ(src0.Number3.Float), saturate)
            };
        }

        private static float RoundZ(float value)
        {
            return (float) Math.Truncate(value);
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
        /// </remarks>
        /// <param name="src0">The components to convert.</param>
        /// <returns>The result of the operation.</returns>
        public static Number4 UtoF(ref Number4 src0)
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