using Ecommerce.APIs.Errors;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.APIs.Controllers
{
	
	public class BasketController : BaseApiController
	{
		private readonly IBasketRepository _basketRepository;

		public BasketController( IBasketRepository basketRepository )
        {
			this._basketRepository = basketRepository;
		}
		[HttpGet]
		public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
		{
			var basket = await _basketRepository.GetCustomerBasketAsync(id);

			return Ok( basket?? new CustomerBasket(id) );
		}
		[HttpPost]
		public async Task<ActionResult<CustomerBasket>> UpdateBasket( CustomerBasket basket)
		{
			var createdOrUpdated = await _basketRepository.UpdateBasketAsync(basket);

			if(createdOrUpdated is null)
			{
				return BadRequest(new ApiResponse(400));
			}
			else
			{
				return Ok(createdOrUpdated);
			}
		}

		[HttpDelete]
		public async Task<string> DeleteBasket (string id)
		{
			var res = await _basketRepository.DeleteBasketAsync(id);

			if (res is false) return  BadRequest().ToString();

			return "Deleted";
			
		}
    }
}
