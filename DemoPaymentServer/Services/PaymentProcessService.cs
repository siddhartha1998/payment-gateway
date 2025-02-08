using DemoPaymentServer.Enums;
using DemoPaymentServer.Models;
using System.Security.Cryptography;

namespace DemoPaymentServer.Services
{
    public class PaymentProcessService
    {
        private static readonly TransactionStatus[] Statuses =
        { TransactionStatus.Success, TransactionStatus.Pending, TransactionStatus.Failed };

        private static readonly Random _random = new Random();

        public static async Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request)
        {
            if (request == null)
            {
                return new PaymentResponse
                {
                    Status = TransactionStatus.Failed,
                    Message = "Request is null",
                    TransactionDateTime = DateTime.UtcNow,
                    ReferenceNo = RandomNumGenerator(12),
                };
            }

            await Task.Delay(3000); // for real-time transaction delay, let's assume transaction time is 3 seconds

            var status = Statuses[_random.Next(Statuses.Length)];

            return new PaymentResponse
            {
                Status = status,
                Message = "Return Transaction Status",
                TransactionDateTime = DateTime.UtcNow,
                ReferenceNo = RandomNumGenerator(12),
                InvoiceNo = request.InvoiceNo
            };
        }


        private static long RandomNumGenerator(int length)
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
