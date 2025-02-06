using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Server.Application.Payments.Commands;

namespace PaymentGateway.Server.Api.Controllers
{
    [Authorize]
    public class PaymentController : ApiControllerBase
    {
        public PaymentController()
        {
            
        }

        [HttpPost("/process")]
        public async Task<IActionResult> paymentProcess([FromBody] CreatePaymentCommand createPayment)
        {

        }
    }
}
