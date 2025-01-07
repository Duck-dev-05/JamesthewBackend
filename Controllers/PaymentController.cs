using Microsoft.AspNetCore.Mvc;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;

namespace JamesThewPaymentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PayPalController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PayPalController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder([FromBody] PayPalOrderRequest request)
        {
            try
            {
                var environment = new SandboxEnvironment(
                    _configuration["PayPal:ClientId"],
                    _configuration["PayPal:Secret"]);
                var client = new PayPalHttpClient(environment);

                var orderRequest = new OrderRequest
                {
                    CheckoutPaymentIntent = "CAPTURE",
                    PurchaseUnits = new List<PurchaseUnitRequest>
                    {
                        new PurchaseUnitRequest
                        {
                            AmountWithBreakdown = new AmountWithBreakdown
                            {
                                CurrencyCode = "USD",
                                Value = request.SubscriptionType == "Monthly" ? "10.00" : "100.00"
                            },
                            Description = request.SubscriptionType + " Subscription"
                        }
                    },
                    ApplicationContext = new ApplicationContext
                    {
                        ReturnUrl = _configuration["PayPal:ReturnUrl"],
                        CancelUrl = _configuration["PayPal:CancelUrl"]
                    }
                };

                var requestMessage = new OrdersCreateRequest();
                requestMessage.Prefer("return=representation");
                requestMessage.RequestBody(orderRequest);

                var response = await client.Execute(requestMessage);
                var result = response.Result<Order>();

                return Ok(new
                {
                    id = result.Id,
                    links = result.Links.Select(link => new { link.Rel, link.Href })
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("capture-order/{orderId}")]
        public async Task<IActionResult> CaptureOrder(string orderId)
        {
            try
            {
                var environment = new SandboxEnvironment(
                    _configuration["PayPal:ClientId"],
                    _configuration["PayPal:Secret"]);
                var client = new PayPalHttpClient(environment);

                var request = new OrdersCaptureRequest(orderId);
                request.RequestBody(new OrderActionRequest());

                var response = await client.Execute(request);
                var result = response.Result<Order>();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }

    public class PayPalOrderRequest
    {
        public required string SubscriptionType { get; set; } // Monthly or Yearly
    }
}
