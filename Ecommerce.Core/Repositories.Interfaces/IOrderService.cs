using Ecommerce.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Repositories.Interfaces
{
	public interface IOrderService
	{
		Task<Order?> CreateOrderAsync(string BuyerEmail,string BasketId, int DeliveryMthodId, Address ShippingAddress);
		Task<IReadOnlyList<Order?>> GetOrdersOFSpecificUSerAsync(string BuyerEmail);
		Task<Order?> GetOrderByIdForSpecificUserAsync(string BuyerEmail, int orderID);
	}
}
