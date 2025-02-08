using DemoPaymentServer.Models;
using DemoPaymentServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoPaymentServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProcessPaymentController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest request)
        {
            PaymentResponse response = await PaymentProcessService.ProcessPaymentAsync(request) ;
            return Ok(response);
        }
    }
}
