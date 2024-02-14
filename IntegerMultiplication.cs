using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Problem
{
    // *****************************************
    // DON'T CHANGE CLASS OR FUNCTION NAME
    // YOU CAN ADD FUNCTIONS IF YOU NEED TO
    // *****************************************
    public static class IntegerMultiplication
    {
        #region YOUR CODE IS HERE

        //Your Code is Here:
        //==================
        /// <summary>
        /// Multiply 2 large integers of N digits in an efficient way [Karatsuba's Method]
        /// </summary>
        /// <param name="X">First large integer of N digits [0: least significant digit, N-1: most signif. dig.]</param>
        /// <param name="Y">Second large integer of N digits [0: least significant digit, N-1: most signif. dig.]</param>
        /// <param name="N">Number of digits (power of 2)</param>
        /// <returns>Resulting large integer of 2xN digits (left padded with 0's if necessarily) [0: least signif., 2xN-1: most signif.]</returns>
        static public byte[] IntegerMultiply(byte[] X, byte[] Y, int N)
        {
            N = Math.Max(X.Length, Y.Length);
            if (N % 2 != 0) N += 1;
            Array.Resize(ref X, N);
            Array.Resize(ref Y, N);

            //if (N == 1) //base case
            //{
            //    byte[] result = new byte[2];
            //    byte Mul1 = (byte)(X[0] * Y[0]);
            //    result[0] = (byte)(Mul1 % 10);
            //    result[1] = (byte)(Mul1 / 10);
            //    return result;
            //}

            if (N<=64)
            {
                byte[] result = new byte[2*N];
                for (int i = 0; i < N; i++)
                {
                    byte carry = 0;
                    for (int j = 0; j < N; j++)
                    {
                        byte mul = (byte)(X[i] * Y[j] + carry + result[i+j]);
                        carry = (byte)(mul / 10);
                        result[i+j] = (byte)(mul % 10);
                    }
                    result[i+N] = carry;
                }
                return result;
            }

            byte[] B = X.Skip(N / 2).ToArray();
            byte[] A = X.Take(N / 2).ToArray();
            byte[] D = Y.Skip(N / 2).ToArray();
            byte[] C = Y.Take(N / 2).ToArray();

            byte[] getSum(byte[] a, byte[] b)
            {
                byte carry = 0;
                int length = Math.Max(a.Length, b.Length);
                byte[] result = new byte[length + 1];
                for (int i = 0; i < length; ++i)
                {
                    byte first = 0;
                    byte second = 0;
                    if (i < a.Length) { first = a[i]; }
                    if (i < b.Length) { second = b[i]; }
                    byte sum = (byte)(first + second + carry);
                    carry = (byte)(sum / 10);
                    result[i] = (byte)(sum % 10);
                }
                result[length] = carry;
                while (result[length] == 0 && length > 0) length--;
                Array.Resize(ref result, length + 1);
                return result;
            }

            byte[] sumAB = getSum(A, B);
            byte[] sumCD = getSum(C, D);
            ////conquer
            byte[] M1 = IntegerMultiply(A, C, N / 2);
            byte[] M2 = IntegerMultiply(B, D, N / 2);
            byte[] Z = IntegerMultiply(sumAB, sumCD, N / 2);

            ////combine
            byte[] getSub(byte[] a, byte[] b)
            {
                int borrow = 0;
                byte[] result = new byte[a.Length];
                for (int i = 0; i < a.Length; ++i)
                {
                    int second = 0;
                    if (i < b.Length) { second = b[i]; }
                    int sub = a[i] - second - borrow;
                    if (sub < 0)
                    {
                        sub = sub + 10;
                        borrow = 1;
                    }
                    else borrow = 0;
                    result[i] = (byte)sub;
                }
                return result;
            }

            byte[] sub1 = getSub(Z, M2);
            byte[] res = getSub(sub1, M1);

            byte[] _M2 = new byte[M2.Length + N];
            Buffer.BlockCopy(M2, 0, _M2, N, M2.Length);

            byte[] _res = new byte[res.Length + N / 2];
            Buffer.BlockCopy(res, 0, _res, N / 2, res.Length);

            byte[] sum1 = getSum(_M2, _res);
            byte[] final = getSum(sum1, M1);
            Array.Resize(ref final, 2 * N);
            return final;
            #endregion


        }

    }
}