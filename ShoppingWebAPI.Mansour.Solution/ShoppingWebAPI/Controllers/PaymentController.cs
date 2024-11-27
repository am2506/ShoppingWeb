using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RouteWebAPI.Helpers.HandleErrors;
using Stripe;
using Shopping.Core.IServices;
using Shopping.Core.Models.Basket;

namespace RouteWebAPI.Controllers
{    
    public class PaymentController : BaseAPIController
    {
        private readonly IPaymentService _paymentService;
        private const string endpointSecret = "whsec_9df5fa3c88479571a78825fed193c7194131c6c7d2b2392afb99f61be75924ca";

        public PaymentController(IPaymentService paymentService)
        {
          _paymentService = paymentService;
        }
        [Authorize]
        [HttpPost("{basketid}")]
        public async Task<ActionResult<CustomerBasket>> CreatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (basket is null)
                return BadRequest(new APIErrorResponse(StatusCodes.Status400BadRequest));
            return Ok(basket);
        }

        [AllowAnonymous]
        [HttpPost("webhook")]
        public async Task<IActionResult> WebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
           
            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);
                var signatureHeader = Request.Headers["Stripe-Signature"];

                stripeEvent = EventUtility.ConstructEvent(json,
                        signatureHeader, endpointSecret);
                #region Check Events
                var paymemtIntent = (PaymentIntent)stripeEvent.Data.Object;
                switch (stripeEvent.Type)
                {
                    case EventTypes.PaymentIntentSucceeded:
                       await _paymentService.UpdateOrderStatus(paymemtIntent.Id, true);
                        break;
                    case EventTypes.PaymentIntentPaymentFailed:
                       await _paymentService.UpdateOrderStatus(paymemtIntent.Id, false);
                        break;

                    default:
                        Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                        break;
                }
                #endregion

                return Ok();
            }
            catch (StripeException e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }


    }
}
