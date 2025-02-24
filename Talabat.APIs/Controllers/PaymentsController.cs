using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Stripe;
using Stripe.Checkout;

using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
   
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        private const string _whSecret = "whsec_be9598a80ddc8630bda3e172e85535931808b4a514cc559828536861a6a6d4e3";
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(IPaymentService paymentService, IMapper mapper, ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
            _mapper = mapper;
            _logger = logger;
        }


        [Authorize]
        [ProducesResponseType(typeof(CustomerBasketDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("{basketId}")] //POST : /api/payments?id=
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntentAsync(basketId);
            if (basket is null) return BadRequest(new ApiResponse(400, "Problem with your basket"));

            return Ok(basket);


        }
        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], _whSecret);

            var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
            switch (stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                    await _paymentService.UpdatePaymentIntentWithSucceededOrFailed(paymentIntent.Id, true);
                    _logger.LogInformation("Payment is Succeedeed yasta", paymentIntent.Id);
                    break;


                case "payment_intent.failed":
                    await _paymentService.UpdatePaymentIntentWithSucceededOrFailed(paymentIntent.Object, false);
                    _logger.LogInformation("Payment is Faliedyasta", paymentIntent.Id);
                    break;
            }


            return Ok();



        }
    }
}
