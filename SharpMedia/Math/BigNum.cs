// This file constitutes a part of the SharpMedia project, (c) 2007 by the SharpMedia team
// and is licensed for your use under the conditions of the NDA or other legally binding contract
// that you or a legal entity you represent has signed with the SharpMedia team.
// In an event that you have received or obtained this file without such legally binding contract
// in place, you MUST destroy all files and other content to which this lincese applies and
// contact the SharpMedia team for further instructions at the internet mail address:
//
//    legal@sharpmedia.com
//
using System;
using System.Collections.Generic;
using System.Text;
using SharpMedia.Testing;
using SharpMedia.AspectOriented;

namespace SharpMedia.Math
{
    /// <summary>
    /// A big number can represent any number. Operations may be much slower than
    /// on hardware supported numbers because big numbers are implemented as an array
    /// of smaller.
    /// </summary>
    /// <remarks>It is immutable, e.g. thread safe.</remarks>
    [Serializable]
    public class BigNum : ICloneable<BigNum>
    {
        #region Private Methods

        bool sign;
        uint[] numbers;

        #endregion

        #region Internal Methods

        /// <summary>
        /// Adds the internal.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <returns></returns>
        private static uint[] AddInternal(uint[] first, uint[] second)
        {
            // We swap so the first one is always longer.
            if (first.Length < second.Length)
            {
                uint[] tmp = first;
                first = second;
                second = tmp;
            }

            // We must add two big numbers. We need array of maximum size. We will truncate it later
            // to correct size.
            uint[] resultArray = new uint[first.Length];

            // We go through all common.
            ulong countForward = 0;
            int i;
            for (i = 0; i < second.Length; i++)
            {
                // We compute result and count forward.
                ulong res = countForward + (ulong)second[i] + (ulong)first[i];
                countForward = res >> 32;

                // We write the result.
                resultArray[i] = (uint)(res & 0xFFFFFFFF);
            }

            // We fo through all long array's.
            for (; i < first.Length; i++)
            {
                // We compute result and count forward.
                ulong res = countForward + (ulong)first[i];
                countForward = res >> 32;

                // We write the result.
                resultArray[i] = (uint)(res & 0xFFFFFFFF);
            }

            if (countForward > 0)
            {
                // We need to enlarge array.
                uint[] newArray = new uint[resultArray.Length + 1];
                resultArray.CopyTo(newArray, 0);
                newArray[resultArray.Length] = (uint)countForward;
                return newArray;
            }
            else
            {
                return resultArray;
            }
        }

        /// <summary>
        /// Subs the internal.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <returns></returns>
        private static uint[] SubInternal(uint[] first, uint[] second, out bool sign)
        {
            sign = true;

            // We swap so the first one is always longer.
            if (first.Length < second.Length)
            {
                uint[] tmp = first;
                first = second;
                second = tmp;
                sign = false;
            }

            // We must add two big numbers. We need array of maximum size. We will truncate it later
            // to correct size.
            uint[] resultArray = new uint[first.Length];

            // We go through all common.
            long countForward = 0;
            int i;
            for (i = 0; i < second.Length; i++)
            {
                // We compute result and count forward.
                long res = (long)first[i] - (long)second[i] - countForward;
                
                // We compute count forward.
                countForward = 0;
                while (res < 0)
                {
                    countForward++;
                    res += (long)uint.MaxValue;
                }

                // We write the result.
                resultArray[i] = (uint)(res & 0xFFFFFFFF);
            }

            // We go through all long array's.
            for (; i < first.Length; i++)
            {
                // We compute result and count forward.
                long res = (long)first[i] - countForward;
                countForward = 0;
                while (res < 0)
                {
                    countForward++;
                    res += (long)uint.MaxValue;
                }

                // We write the result.
                resultArray[i] = (uint)(res & 0xFFFFFFFF);
            }

            // We may need to zero-out array.
            int firstNonZero = 0;
            for (int j = resultArray.Length - 1; j >= 0; j--)
            {
                if (resultArray[j] != 0)
                {
                    firstNonZero = j;
                    break;
                }
            }

            if (firstNonZero == resultArray.Length - 1)
            {
                return resultArray;
            }
            else
            {
                uint[] newArray = new uint[firstNonZero+1];
                for (int k = 0; k <= firstNonZero; k++)
                {
                    newArray[k] = resultArray[k];
                }
                return newArray;
            }
            
        }

        /// <summary>
        /// A fast is-one validation.
        /// </summary>
        private bool IsZero
        {
            get
            {
                return numbers.Length == 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is odd.
        /// </summary>
        /// <value><c>true</c> if this instance is odd; otherwise, <c>false</c>.</value>
        private bool IsOdd
        {
            get
            {
                return (numbers[0] & 0x1) == 0;
            }
        }

        /// <summary>
        /// Shifts the in place internal.
        /// </summary>
        /// <param name="data">The data.</param>
        private static uint[] RShiftInPlaceInternal(uint[] data)
        {
            uint toNext = 0;
            int maxLenght = -1;
            for (int i = data.Length - 1; i >= 0; i--)
            {
                uint tmp = data[i] & 0x1;
                data[i] >>= 1;
                data[i] |= toNext << 31;

                if (data[i] != 0 && i > maxLenght)
                {
                    maxLenght = i;
                }

                toNext = tmp;
            }

            if (maxLenght == -1)
            {
                return new uint[0];
            }

            // We may need to make array smaller.
            if (maxLenght != data.Length - 1)
            {
                uint[] newData = new uint[maxLenght + 1];
                for (int j = 0; j < newData.Length; j++)
                {
                    newData[j] = data[j];
                }

                return newData;
            }
            else
            {
                return data;
            }
        }

        /// <summary>
        /// Shifts the in place internal.
        /// </summary>
        /// <param name="data">The data.</param>
        private static uint[] LShiftInPlaceInternal(uint[] data)
        {
            uint toNext = 0;
            for (int i = 0; i < data.Length; i++)
            {
                uint tmp = (data[i] & 0x80000000) >> 31;
                data[i] <<= 1;
                data[i] |= toNext;
                toNext = tmp;
            }

            // We may need to enlarge.
            if (toNext > 0)
            {
                uint[] newData = new uint[data.Length + 1];
                data.CopyTo(newData, 0);
                newData[data.Length] = toNext;
                return newData;
            }
            else
            {
                return data;
            }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BigNum"/> class.
        /// </summary>
        /// <param name="number">The number.</param>
        public BigNum(int number)
        {
            this.sign = number >= 0 ? true : false;
            this.numbers = new uint[] { number > 0 ? (uint)number : (uint)-number };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BigNum"/> class.
        /// </summary>
        /// <param name="number">The number.</param>
        public BigNum(long number)
        {
            this.sign = number >= 0 ? true : false;
            if (number < 0) number = -number;
            this.numbers = new uint[] { (uint)(number & 0xFFFFFFFF), (uint)(number >> 32) };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BigNum"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="sign">if set to <c>true</c> [sign].</param>
        private BigNum([NotNull] uint[] data, bool sign)
        {
            this.numbers = data;
            this.sign = sign;
        }

        #endregion

        #region Operators


        public static BigNum operator -([NotNull] BigNum first, [NotNull] BigNum second)
        {
            // Convert to addition if needed.
            if (first.sign == false && second.sign == true)
            {
                return new BigNum(AddInternal(first.numbers, second.numbers), false);
            }
            if (first.sign == true && second.sign == false)
            {
                return new BigNum(AddInternal(first.numbers, second.numbers), true);
            }

            bool sign;
            uint[] data = SubInternal(first.numbers, second.numbers, out sign);

            return new BigNum(data, first.sign ? sign : !sign);

        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <returns>The result of the operator.</returns>
        public static BigNum operator +([NotNull] BigNum first, [NotNull] BigNum second)
        {
            // Convert to substraction if needed.
            if (first.sign == false && second.sign == true)
            {
                // We have -first + second
                bool sign;
                uint[] data = SubInternal(second.numbers, first.numbers, out sign);
                return new BigNum(data, sign);

            }
            if (first.sign == true && second.sign == false)
            {
                // We have first - second.
                bool sign;
                uint[] data = SubInternal(first.numbers, second.numbers, out sign);
                return new BigNum(data, sign);
            }

            return new BigNum(AddInternal(first.numbers, second.numbers), first.sign);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <returns>The result of the operator.</returns>
        public static BigNum operator *([NotNull] BigNum first, [NotNull] BigNum second)
        {
            // TODO: rewrite multiplication to FFT for big numbers (since it is O(n*log2(N)) instead of O(N^2)).

            // We always make the first smaller (quicked operation).
            if (first.numbers.Length > second.numbers.Length)
            {
                BigNum tmp = first;
                first = second;
                second = tmp;
            }

            // We clone first and second (because we change them in place).
            first = first.Clone() as BigNum;
            second = second.Clone() as BigNum;

            // We implement russian multiplication. In future, FFT could be used.
            List<BigNum> factorsToSum = new List<BigNum>();

            // We do russion multiplication.
            for (; !first.IsZero; 
                 first.numbers  = RShiftInPlaceInternal(first.numbers),
                 second.numbers = LShiftInPlaceInternal(second.numbers))
            {
                if (!first.IsOdd)
                {
                    factorsToSum.Add(second.Clone() as BigNum);
                }
            }

            // Make sure we preterminate.
            if (factorsToSum.Count == 0)
            {
                return new BigNum(0);
            }

            // Multiplication is sum of all factors.
            BigNum sum = factorsToSum[0];
            for (int i = 1; i < factorsToSum.Count; i++)
            {
                sum = sum + factorsToSum[i];
            }

            // Returns the sum.
            return sum;
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="num">The num.</param>
        /// <param name="number">The number.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==([NotNull] BigNum num, int number)
        {
            if (num.numbers.Length != 1) return false;
            if (number < 0)
            {
                if (num.sign != false) return false;
                return (uint)-number == num.numbers[0];
            }
            else
            {
                if (num.sign != true) return false;
                return (uint)number == num.numbers[0];
            }
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="num">The num.</param>
        /// <param name="number">The number.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator!=([NotNull]BigNum num, int number)
        {
            return !(num == number);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="num">The num.</param>
        /// <param name="number">The number.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==([NotNull] BigNum num1, [NotNull] BigNum num2)
        {
            if (num1.numbers.Length != num2.numbers.Length) return false;
            if (num2.sign != num1.sign) return false;

            for (int i = 0; i < num1.numbers.Length; i++)
            {
                if (num1.numbers[i] != num2.numbers[i]) return false;
            }
            return true;
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="num">The num.</param>
        /// <param name="number">The number.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=([NotNull] BigNum num1, [NotNull] BigNum num2)
        {
            return !(num1 == num2);
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Tries the parse.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        public static bool TryParse([NotEmptyArray] string str, out BigNum number)
        {
            number = null;
            BigNum result = new BigNum(0);

            // We trim string first.
            str = str.Trim();
            bool sign = true;

            // We begin parsing from the end.
            int index = str.Length - 1;

            // 1) We read numbers.

            BigNum position = new BigNum(1);
            for (; index >= 0; index--)
            {
                if (str[index] < '0' || str[index] > '9')
                {
                    break;
                }

                // Do not waste a multiply by zero.
                if (str[index] != '0')
                {
                    result = result + position * new BigNum(str[index] - '0');
                }

                position *= new BigNum(10);
            }

            // 2) check the sign.
            if (index == 0)
            {
                if (str[0] == '-')
                {
                    sign = false;
                    index--;
                }
                else if (str[0] == '+')
                {
                    sign = true;
                    index--;
                }
                else
                {
                    return false;
                }
            }

            if (index != -1) return false;

            // Obtain result.
            if (sign)
            {
                number = result;
            }
            else
            {
                number = new BigNum(result.numbers, false);
            }
            return true;
        }

        /// <summary>
        /// Parses the specified STR.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static BigNum Parse([NotEmptyArray] string str)
        {
            BigNum num;
            if(!TryParse(str, out num))
            {
                throw new InvalidOperationException("Cannot parse a big number.");
            }
            return num;
        }

        #endregion

        #region Overrides

        public override bool Equals([NotNull] object obj)
        {
            if (obj.GetType() == this.GetType())
            {
                return (BigNum)obj == this;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return numbers.GetHashCode() + sign.GetHashCode();
        }

        public override string ToString()
        {
            
            return StringHexadecimal;
        }

        public string StringBinary
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                if (!sign) builder.Append("-");

                for (int i = numbers.Length - 1; i >= 0; i--)
                {
                    uint mask = 0x80000000;
                    for (int j = 0; j < 32; j++, mask>>=1)
                    {
                        if ((mask & numbers[i]) != 0)
                        {
                            builder.Append("1");
                        }
                        else
                        {
                            builder.Append("0");
                        }
                    }
                }
                return builder.ToString();
            }
        }


        public string StringHexadecimal
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                if (!sign) builder.Append("-0x");
                else builder.Append("0x");

                for (int i = numbers.Length-1; i >= 0; i--)
                {
                    string s = string.Format("{0:X}", numbers[i]);
                    for (int j = 0; j < 8 - s.Length; j++)
                    {
                        builder.Append("0");
                    }
                    builder.Append(s);

                }
                return builder.ToString();
            }
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Performs an explicit conversion from <see cref="SharpMedia.Math.BigNum"/> to <see cref="System.Int32"/>.
        /// Throws if not possible.
        /// </summary>
        /// <param name="num">The num.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator int([NotNull] BigNum num)
        {
            if (num.numbers.Length > 1 ||
               (num.numbers.Length == 1 && (num.numbers[0] & 0x8000000) != 0))
            {
                throw new InvalidCastException("The big number does not fit into integer range.");
            }

            // No lenght, we have a zero value.
            if (num.numbers.Length == 0) return 0;
            return num.sign ? (int)num.numbers[0] : -(int)num.numbers[0];
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SharpMedia.Math.BigNum"/> to <see cref="System.Int64"/>.
        /// Throws if not possible.
        /// </summary>
        /// <param name="num">The num.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator long([NotNull] BigNum num)
        {
            if (num.numbers.Length > 12 ||
               (num.numbers.Length == 2 && (num.numbers[1] & 0x8000000)!=0))
            {
                throw new InvalidCastException("The big number does not fit into long range.");
            }

            // No lenght, we have a zero value.
            if (num.numbers.Length == 0) return 0;

            long number = (long)num.numbers[0];

            if(num.numbers.Length == 2)
            {
                number += (long)num.numbers[1] << 32;
            }

            return num.sign ? number : -number;
        }

        #endregion

        #region ICloneable<BigNum> Members

        public BigNum Clone()
        {
            return new BigNum(numbers.Clone() as uint[], sign);
        }

        #endregion
    }

#if SHARPMEDIA_TESTSUITE

    [TestSuite]
    internal class BigNumTest
    {
        [CorrectnessTest]
        public void Parse()
        {
            BigNum num;
            
            Assert.IsTrue(BigNum.TryParse("1232", out num));
            Assert.IsTrue(BigNum.TryParse("-323223", out num));
            Assert.IsTrue(BigNum.TryParse("+3223", out num));
            Assert.IsTrue(BigNum.TryParse("3124348242462744325424372493542473254651803698656464561264", out num));
            Assert.IsTrue(!BigNum.TryParse("1+21", out num));
            Assert.IsTrue(!BigNum.TryParse("+-323423", out num));
        }

        [CorrectnessTest]
        public void Formatting()
        {
            BigNum num = new BigNum(0xFAF8767DC);
            Assert.AreEqual("0xFAF8767DC", num.StringHexadecimal);
        }

        [CorrectnessTest]
        public void Add()
        {
            BigNum num1 = new BigNum(0x400000000000000L);
            BigNum num2 = new BigNum(0x400000000000000L);
            BigNum num3 = new BigNum(0x800000000000000L);

            Assert.AreEqual(num1 + num2, num3);

            num1 = BigNum.Parse("999999999999999999999999999999999999999999999999999999999");
            num2 = BigNum.Parse("1");
            num3 = BigNum.Parse("1000000000000000000000000000000000000000000000000000000000");

            Assert.AreEqual(num1 + num2, num3);
        }

        [CorrectnessTest]
        public void Sub()
        {
            BigNum num1 = new BigNum(1000000000);
            BigNum num2 = new BigNum(200000000000);
            BigNum num3 = new BigNum(1000000000 - 200000000000);

            Assert.AreEqual(num1 - num2, num3);
        }

        [CorrectnessTest]
        public void Mul()
        {
            BigNum num1 = new BigNum(10000000000000);
            BigNum num2 = new BigNum(20000000000000);
            BigNum num3 = new BigNum(-500);

            Assert.AreEqual(num1 * num2, num2 * num1);
            Assert.AreEqual(num1 * num3, num3 * num1);
        }
    }

#endif
}
