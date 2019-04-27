using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace WideWorldImporters.Core.Helpers
{
    /// <summary>
    /// Utility functions for integers
    /// </summary>
    public static class IntHelpers
    {

        /// <summary>
        /// Generates a cryptographically secure random number between min and max
        /// </summary>
        /// <param name="min">Lower bound</param>
        /// <param name="max">Upper bound</param>
        /// <returns></returns>
        public static int GetRandomNumber(int min, int max)
        {
            RNGCryptoServiceProvider Rand = new RNGCryptoServiceProvider();

            uint scale = uint.MaxValue;
            while (scale == uint.MaxValue)
            {
                // Get four random bytes.
                byte[] four_bytes = new byte[4];
                Rand.GetBytes(four_bytes);

                // Convert that into an uint.
                scale = BitConverter.ToUInt32(four_bytes, 0);
            }

            // Add min to the scaled difference between max and min.
            return (int)(min + (max - min) * (scale / (double)uint.MaxValue));
        }

        /// <summary>
        /// Generates a cryptographically secure random number between 0 and max
        /// </summary>
        /// <param name="max">Upper bound</param>
        /// <returns></returns>
        public static int GetRandomNumber(int max)
        {
            return GetRandomNumber(0, max);
        }

    }

}
