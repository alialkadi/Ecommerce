using Ecommerce.Core.Entities.Order;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.APIs.Dtos
{
	public class OrderResponseDto
	{
        public int Id { get; set; }
        public string buyerEmail { get; set; }
		public DateTimeOffset orderDate { get; set; } = DateTimeOffset.Now;
		public string orderStatus { get; set; } 
		public Address shippingAddress { get; set; }
		public string deliveryMethod { get; set; }
		public ICollection<OrderItemDto> Items { get; set; } = new HashSet<OrderItemDto>();
		public decimal subTotal { get; set; }
		public decimal total { get; set; }
		public decimal DeliveryMethodCost { get; set; }
		public string PaymentIntentId { get; set; } = string.Empty;
	}
}
