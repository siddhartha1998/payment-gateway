using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PaymentGateway.Server.Api.Services;
using PaymentGateway.Server.Application.Common;
using PaymentGateway.Server.Application.Payments.Commands;
using PaymentGateway.Server.Application.Payments.Queries;


namespace PaymentGateway.Server.Api.Controllers
{
    [Authorize]
    public class PaymentController : ApiControllerBase
    {
        private readonly HmacService _hmacService;

        public PaymentController(HmacService hmacService)
        {
            _hmacService = hmacService;
        }

        //[HttpPost("/generate-hmac")]
        //public string GenerateHmac([FromBody] CreatePaymentCommand command)
        //{
        //    string requestData = JsonConvert.SerializeObject(command);
        //    string hmac = _hmacService.ComputeHmac(requestData);
        //    return hmac;
        //}


        [HttpPost("process")]
        public async Task<ActionResult<Result>> paymentProcess([FromBody] CreatePaymentCommand createPayment)
        {
            //string requestData = JsonConvert.SerializeObject(createPayment);

            //if (!_hmacService.VerifyHmac(requestData, hmac))
            //{
            //    return Unauthorized(new { Message = "Invalid HMAC signature" });
            //}

            return await Mediator.Send(createPayment);

        }

        [HttpGet]
        public async Task<ActionResult<List<TransactionDetail>>> GetPaymentDetails()
        {
            var query = new GetAllTransactionDetailsQuery();
            return await Mediator.Send(query);
        }
    }
}
