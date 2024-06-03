using Ecommerce.Core.Entities;

namespace Ecommerce.APIs.Dtos
{
	public class CustomerBasketDTO
	{
		public string id { get; set; }
		public List<BasketItem> Items { get; set; }
		public int? DeliveryMethodId { get; set; }
		public string? PaymentIntentId { get; set; }
		public string? clientSecret { get; set; }
	}
}
