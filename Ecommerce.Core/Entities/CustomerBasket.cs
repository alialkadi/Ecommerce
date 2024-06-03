using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entities
{
	public class CustomerBasket
	{
		

		public string id { get; set; }
        public List<BasketItem>  Items { get; set; }
		public int? DeliveryMethodId { get; set; }
		public string? PaymentIntentId { get; set; }
		public string? clientSecret { get; set; }

    }
}
