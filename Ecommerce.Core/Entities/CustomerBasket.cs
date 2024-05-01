using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entities
{
	public class CustomerBasket
	{
		public CustomerBasket(string id)
		{
			this.id = id;
			Items =new List<BasketItem>();
		}

		public string id { get; set; }
        public List<BasketItem>  Items { get; set; }

    }
}
