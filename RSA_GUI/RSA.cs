using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSA_GUI
{
     public class RSA
    {

        public BigInteger PHI { get; private set; }
        public int E { get; private set; }
        public BigInteger N { get; private set; }
        public int D { get; private set; }

        public BigInteger C { get; private set; }

        public Dictionary<int,int> PrimeNumbersDict { get; private set; }

        public RSA(int primeNumsLimit)
        {

            GeneratePrimeNumbers(primeNumsLimit);
        }


        private static bool CheckNumberPrimeNumber(BigInteger num)
        {
            bool CheckPrimeNumber = true;
            BigInteger factor = num / 2;
            BigInteger i = 0;
            for (i = 2; i <= factor; i++)
            {
                if ((num % i) == 0) CheckPrimeNumber = false;
            }
            return CheckPrimeNumber;
        }

        private void GeneratePrimeNumbers(int limit)
        {
            PrimeNumbersDict = new Dictionary<int, int>();

            for (int i = 2; i < limit; i++)
            {
                //if (CheckNumberPrimeNumber(i) == true) primeNums.Add(i);
                if (CheckNumberPrimeNumber(i) == true) PrimeNumbersDict.Add(i, i);
            }

        }


        public static BigInteger NWD(BigInteger a, BigInteger b)
        {
            BigInteger c;
            while (b != 0)
            {
                c = a % b;
                a = b;
                b = c;
            }

            return a;
        }

        public void GenerateKey(int p, int q)
        {
            N = p * q;
            PHI = (p - 1) * (q - 1);


            for (int i = 0; i < PrimeNumbersDict.Keys.Count; i++)
            {
                foreach (var item in PrimeNumbersDict)
                {
                    if (NWD(item.Value, PHI) == 1)
                    {
                        E = item.Value;
                        break;
                    }
                }

                
            }

            if (E == 0)
            {
                throw new ArgumentException("Za mało liczb pierwszych!");
            }

            for (int d = 2; d < int.MaxValue; d++)
            {
                if ((d * E) % PHI == 1)
                {
                    D = d;
                    break;
                }
            }
        }


        public BigInteger Encrypt(int m)
        {
            C = BigInteger.ModPow(m, E,N);

            return C;
        }

        public BigInteger Encrypt(int m, int e, BigInteger n)
        {
            var result = BigInteger.ModPow(m, e,n);

            return result;
        }

        public BigInteger Encrypt(BigInteger m)
        {
            C = BigInteger.ModPow(m, E,N);

            return C;
        }

        public BigInteger Decrypt(int c)
        {
            var m = BigInteger.ModPow(c, D, N);

            return m;
        }

        public BigInteger Decrypt(BigInteger c)
        {
            var m = BigInteger.ModPow(c, D,N);

            return m;
        }

        public int Decrypt(BigInteger c, int d, BigInteger n)
        {
            var m = BigInteger.ModPow(c, d,n);

            
            return (int)m;
        }


    }
}
