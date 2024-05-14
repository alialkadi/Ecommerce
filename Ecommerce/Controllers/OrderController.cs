using AutoMapper;
using Ecommerce.APIs.Dtos;
using Ecommerce.APIs.Errors;
using Ecommerce.Core.Entities.Order;
using Ecommerce.Core.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Ecommerce.APIs.Controllers
{
	
	public class OrderController : BaseApiController
	{
		private readonly IOrderService _orderService;
		private readonly IMapper _mapper;
		private readonly IBasketRepository _basketRepository;
		private readonly IUnitOfWork unitOfWork;

		public OrderController( IOrderService orderService , IMapper mapper, IBasketRepository basketRepository, IUnitOfWork unitOfWork  )
        {
			this._orderService = orderService;
			this._mapper = mapper;
			this._basketRepository = basketRepository;
			this.unitOfWork = unitOfWork;
		}
        [HttpPost("CreateOrder")]
		[ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
		public async Task<ActionResult<OrderResponseDto>> CreateOrder([FromBody] OrderDto order, [FromHeader]string Token )
		{
			if (!Request.Headers.ContainsKey("Token"))
			{
				return Unauthorized( new ApiResponse(401,"Token Header Is needed"));
			}
			var handler = new JwtSecurityTokenHandler();
			var Wtoken = handler.ReadJwtToken(Token);
			var basketExist =await _basketRepository.GetCustomerBasketAsync(order.BasketId);
			if (basketExist is null)
			{
				return NotFound(new ApiResponse(404, "Customer Doesn't have Any basket"));
			}
			// Check if the token contains an "email" claim
			var emailClaim = Wtoken.Payload.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
			var Address =_mapper.Map<AddressDto, Address>(order.Address);
			var orderApp = await _orderService.CreateOrderAsync(emailClaim, order.BasketId, order.DeliveryMethodId, Address);
			if (orderApp == null)
			{
				return BadRequest(new ApiResponse(400, "Problem In Order Creation"));
			}
			await _basketRepository.DeleteBasketAsync(order.BasketId);
			var res = _mapper.Map<Order, OrderResponseDto>(orderApp);

			return Ok(res);


		}

		[HttpGet("GetOrder")]
		[ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
		public async Task<ActionResult<IReadOnlyList<OrderResponseDto>>> GetOrderForUser([FromHeader] string Token )
		{
			if (!Request.Headers.ContainsKey("Token"))
			{
				return Unauthorized(new ApiResponse(401, "Token Header Is needed"));
			}
			var handler = new JwtSecurityTokenHandler();
			var Wtoken = handler.ReadJwtToken(Token);
			var emailClaim = Wtoken.Payload.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;

			var Orders = await _orderService.GetOrdersOFSpecificUSerAsync(emailClaim);
            if (Orders is null) return NotFound(new ApiResponse(404,"There is No Order For this User"));

			var res = _mapper.Map< IReadOnlyList<OrderResponseDto>>(Orders);

			return Ok(res);

		}
		[HttpGet("GetOrderById/{id}")]
		[ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
		public async Task<ActionResult<OrderResponseDto>> GetOrderForUserById(int orderId,[FromHeader] string Token)
		{
			if (!Request.Headers.ContainsKey("Token"))
			{
				return Unauthorized(new ApiResponse(401, "Token Header Is needed"));
			}
			var handler = new JwtSecurityTokenHandler();
			var Wtoken = handler.ReadJwtToken(Token);
			var emailClaim = Wtoken.Payload.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;

			var Orders = await _orderService.GetOrderByIdForSpecificUserAsync(emailClaim,orderId);
			if (Orders is null) return NotFound(new ApiResponse(404, "There is No Order For this User By this ID"));

			var res = _mapper.Map<OrderResponseDto>(Orders);

			return Ok(res);

		}

		[HttpGet]
		[Route("DeliveryMethods")]
		public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
		{
			var res = await unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
			if (res == null) return NotFound(new ApiResponse(404, "No Data Found"));
			return Ok(res);
		}
	}
}
