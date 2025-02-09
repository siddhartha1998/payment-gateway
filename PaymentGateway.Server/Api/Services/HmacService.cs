using System.Security.Cryptography;
using System.Text;

namespace PaymentGateway.Server.Api.Services
{
    public class HmacService
    {
        private readonly string _secretKey;

        public HmacService(string secretKey)
        {
            _secretKey = secretKey;
        }

        public string ComputeHmac(string data)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secretKey)))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return Convert.ToBase64String(hash);
            }
        }

        public bool VerifyHmac(string data, string providedHmac)
        {
            string computedHmac = ComputeHmac(data);
            return computedHmac == providedHmac;
        }
    }
}
