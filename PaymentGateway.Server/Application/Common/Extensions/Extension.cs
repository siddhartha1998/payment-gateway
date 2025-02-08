using System.Security.Cryptography;

namespace PaymentGateway.Server.Application.Common.Extensions
{
    public static class Extension
    {
        public static long RandomNumber(this int length)
        {
            if (length < 1 || length > 18) // Limit to 18 to prevent overflow for long
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be between 1 and 18.");

            byte[] buffer = new byte[8];
            RandomNumberGenerator.Fill(buffer);
            long value = BitConverter.ToInt64(buffer, 0);

            long minValue = (long)Math.Pow(10, length - 1);
            long maxValue = (long)Math.Pow(10, length) - 1;

            return Math.Abs(value % (maxValue - minValue + 1)) + minValue;
        }
    }
}
