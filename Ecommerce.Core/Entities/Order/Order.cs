using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entities.Order
{
	public class Order :BaseEntity
	{
        public Order()
        {
            
        }
        public Order(string buyerEmail, Address shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal)
		{
			this.buyerEmail = buyerEmail;
			this.shippingAddress = shippingAddress;
			this.deliveryMethod = deliveryMethod;
			Items = items;
			this.subTotal = subTotal;
		}

		public string buyerEmail { get; set; }
        public DateTimeOffset orderDate { get; set; } = DateTimeOffset.Now;
        public OrderStatus orderStatus { get; set; } = OrderStatus.Pending;
        public Address shippingAddress { get; set; }
        public DeliveryMethod deliveryMethod { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
        public decimal subTotal { get; set; }
        [NotMapped]
        public decimal total { get => subTotal + deliveryMethod.Cost ; }
        public string PaymentIntentId { get; set; } = string.Empty;



    }
}
