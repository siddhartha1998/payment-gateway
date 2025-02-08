using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PaymentGateway.Server.Application.Common;
using PaymentGateway.Server.Application.Payments.Commands;
using System.Text;

namespace PaymentGateway.Server.Api.Controllers
{
    [Authorize]
    public class PaymentController : ApiControllerBase
    {

        
        [HttpPost("/process")]
        public async Task<ActionResult<Result>> paymentProcess([FromBody] CreatePaymentCommand createPayment)
        {
                return await Mediator.Send(createPayment);
            
        }
    }
}
