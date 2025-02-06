using DemoPaymentServer.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace DemoPaymentServer.Services
{
    public class PaymentProcessService
    {
        private static readonly TransactionStatus[] Statuses =
        { TransactionStatus.Success, TransactionStatus.Pending, TransactionStatus.Failed };

        private static readonly Random _random = new Random();

        public async Task<PaymentProcessResponse> ProcessPaymentAsync()
        {
            await Task.Delay(3000);

            var status = Statuses[_random.Next(Statuses.Length)];

            return new PaymentProcessResponse
            {
                Status = status,
                Message = "Return Transaction Status",
                TransactionDateTime = DateTime.UtcNow,
                ReferenceNo = RandomNumGenerator(12)
            };
        }


        private long RandomNumGenerator(int length)
        {
            if(length < 1 || length > 18) // Limit to 18 to prevent overflow for long
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
