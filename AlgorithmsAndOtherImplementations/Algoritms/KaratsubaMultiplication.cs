using System;
using System.Text;
using System.Text.RegularExpressions;

namespace AlgorithmsAndOtherImplementations
{
    /// <summary>
    /// My implementation of Karatsuba multiplication algorithm
    /// </summary>
    public class KaratsubaMultiplication
    {
        static void Main(string[] args)
        {
            return;
        }

        /// <summary>
        /// Entry point for unit test.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public string Run(string x, string y)
        {
            // Regex for check if strings don't contain any
            // Non-numeric symbols
            Regex containsOnlyDigits = new Regex("^\\d+$");

            // Check if input numbers are appropriate
            if (x.Length != y.Length)
            {
                throw new Exception("Input numbers lengths aren't equal. Not implemented for unequal lengths");
            } 
            if (x.Length % 2 != 0 || y.Length % 2 != 0)
            {
                // Just implement logic for odd numbers by yourself :)
                throw new Exception("Lengths should be even");
            }
            if (!(containsOnlyDigits.IsMatch(x) && containsOnlyDigits.IsMatch(y)))
            {
                throw new Exception("Inputs aren't numeric-only");
            }

            return new KaratsubaMultiplication().Multiply(x, y);
        }

        /// <summary>
        /// Multiplication implements next formula: 10^n * ac + 10^(n/2) * (ad + bc) + bd
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private string Multiply(string x, string y)
        {
            int n = x.Length;
            if (n == 1)
            {
                return (Int32.Parse(x) * Int32.Parse(y)).ToString();
            }

            var a = x.Left();
            var b = x.Right();
            var c = y.Left();
            var d = y.Right();


            var ac = Multiply(a, c);
            var ad = Multiply(a, d);
            var bc = Multiply(b, c);
            var adPlusBc = Add(ad, bc);
            var bd = Multiply(b, d);

            var step1 = Add(ac.AddExtraTens(n), adPlusBc.AddExtraTens(n/2));
            var step2 = Add(step1, bd);

            return step2;
        }

        /// <summary>
        /// Method for addition of two string integers
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private string Add(string x, string y)
        {
            bool extraAdd = false;
            StringBuilder result = new StringBuilder();

            // Fill left side of number by zeroes, if lengths aren't equal
            if (x.Length < y.Length)
            {
                x = x.PadLeft(y.Length, '0');
            } 
            else if (x.Length > y.Length)
            {
                y = y.PadLeft(x.Length, '0');
            }

            for (var i = x.Length - 1; i >= 0; i--)
            {
                // pick digits on ith position, then make addition
                int addition = (int)(char.GetNumericValue(x[i]) + char.GetNumericValue(y[i]));

                // if we have carry from previous adition - then add extra 1
                if (extraAdd)
                    addition += 1;

                // let's check if result of addition is double digit number:
                // if result of mod operation is the same as original addition - 
                // then number if single digit and extra addition to 
                var unitDigit = addition % 10;
                // unitDigit goes to the left of result string
                result = result.Insert(0, unitDigit);
                if (unitDigit != addition)
                {
                    extraAdd = true;

                    if (i == 0)
                        result = result.Insert(0, '1');
                } else
                {
                    extraAdd = false;
                }


            }

            return result.ToString();
        }

        /// <summary>
        /// Just bonus implementation of subtraction of string integers :) Not in use in this implementation of algorithm
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private string Subtract(string x, string y)
        {
            bool extraMinus = false;
            StringBuilder result = new StringBuilder();
            bool isNegative = false;

            if (x.IsSmallerThen(y))
            {
                Swap(ref x, ref y);
                isNegative = true;
            }

            // digitX - minuend, digitY - subtrahend
            int digitX = 0;
            int digitY = 0;


            for (var i = x.Length - 1; i >= 0; i--)
            {
                // pick digits on ith position
                digitX = (int)char.GetNumericValue(x[i]);
                digitY = (int)char.GetNumericValue(y[i]);

                // subtract 1 if was borrowed
                if (extraMinus)
                {
                    digitX -= 1;
                }

                // if minuend < subtrahend - then extra ten for minuend is required
                extraMinus = digitX < digitY;

                // make subtraction
                int difference = (extraMinus ? 10 + digitX : digitX) - digitY;
                
                result = result.Insert(0, difference);
                if (i == 0 && isNegative)
                {
                    result.Insert(0, '-');
                }

            }

            return result.ToString();
        }

        private void Swap(ref string x, ref string y)
        {
            var temp = x;
            x = y;
            y = temp;
        }
    }

    #region extension methods

    public static class KaratsubaHelperExtension
    {
        /// <summary>
        /// This method takes left part of integer string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Left(this string input)
        {
            return input.Substring(0, input.Length / 2);
        }

        /// <summary>
        /// This one takes obviously right part of integer string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Right(this string input)
        {
            return input.Substring(input.Length / 2, input.Length / 2);
        }

        /// <summary>
        /// This extension method finds out if a integer is smaller then b integer
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IsSmallerThen(this string a, string b)
        {
            if (a.Length < b.Length)
            {
                return true;
            }
            if (a.Length > b.Length)
            {
                return false;
            }

            var aCharArr = a.ToCharArray();
            var bCharArr = b.ToCharArray();

            for (int i = 0; i < aCharArr.Length; i++)
            {
                if (aCharArr[i] > bCharArr[i])
                {
                    return false;
                }
                if (aCharArr[i] < bCharArr[i])
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Adds n zeroes to string integer
        /// </summary>
        /// <param name="input"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string AddExtraTens(this string input, int n)
        {
            StringBuilder inpWithExtraTens = new StringBuilder();
            inpWithExtraTens.Append(input);
            inpWithExtraTens.Append('0', n);

            return inpWithExtraTens.ToString();
        }
    }

    #endregion
}
