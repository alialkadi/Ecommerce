using Ecommerce.APIs.Errors;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.APIs.Controllers
{
	
	public class PaymentController : BaseApiController
	{
		private readonly IPaymentService _paymentService;

		public PaymentController(IPaymentService paymentService) {
			this._paymentService = paymentService;
		}

		[ProducesResponseType(typeof(CustomerBasket),StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
		[HttpPost]
		public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentMethod(string basketId)
		{
			var basket = _paymentService.CreateOrUpdatePaymentIntent(basketId);
			if (basket == null) return BadRequest(new ApiResponse(400, "There is a problem with Customer Basket") );

			return Ok(basket);
		}
	}
}
