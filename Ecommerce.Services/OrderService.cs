using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.Order;
using Ecommerce.Core.Repositories.Interfaces;
using Ecommerce.Core.Specifications.OrderSpecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services
{
	public class OrderService : IOrderService
	{
		private readonly IBasketRepository _basketRepository;
		private readonly IUnitOfWork _unitOfWork;
		

		public OrderService( IBasketRepository basketRepository,IUnitOfWork unitOfWork)
		{
			this._basketRepository = basketRepository;
			this._unitOfWork = unitOfWork;
			
		}
		public async Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMthodId, Address ShippingAddress)
		{
			//1. Get the basket with the items From the Basket REPO
			var basket = await _basketRepository.GetCustomerBasketAsync(BasketId);
			//2. Get Selected Items of basket

			var orderItems = new List<OrderItem>();
			if ( basket?.Items.Count()  > 0 )
			{
				foreach(var item in basket.Items )
				{
					var product =await _unitOfWork.Repository<Product>().GetAsync(item.Id);
					var productItemOrdered = new ProductItemOrder(product.Id, product.Name, product.PictureUrl);
					var orderItem = new OrderItem(productItemOrdered, item.Price, item.Quantity);
					orderItems.Add(orderItem);
				}
			}
			//3. calc the subtotal
			var subTotal = orderItems.Sum( orederItem=> orederItem.Price * orederItem.Quantity );
			//4. Delivery Method
			var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(DeliveryMthodId);
			//5. Create Order

			var order = new Order( BuyerEmail,ShippingAddress,DeliveryMethod,orderItems,subTotal );
			//6. Add Order To Database
			await _unitOfWork.Repository<Order>().AddAsync(order);
			//7 save to database
			var result = await _unitOfWork.CompleteAsync();

			if(result <= 0) return null;

			return order;

		}

		public async Task<Order?> GetOrderByIdForSpecificUserAsync(string BuyerEmail, int orderID)
		{
			var spec = new ORderWithSpecs(BuyerEmail, orderID);
			var res = await _unitOfWork.Repository<Order>().GetWithSpecAsync(spec);
			return res;
		}

		public async Task<IReadOnlyList<Order?>> GetOrdersOFSpecificUSerAsync(string BuyerEmail)
		{
			var spec = new ORderWithSpecs(BuyerEmail);
		 	var res = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
			if(res is null) return null;

			return res;

		}
	}
}
