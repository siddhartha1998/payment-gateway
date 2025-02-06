using PaymentGateway.Server.Application.Common.Interface;

namespace PaymentGateway.Server.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
