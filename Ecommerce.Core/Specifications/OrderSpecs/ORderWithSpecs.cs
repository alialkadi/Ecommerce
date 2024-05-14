using Ecommerce.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Specifications.OrderSpecs
{
	public class ORderWithSpecs : BaseSpecifications<Order>
	{
        public ORderWithSpecs(string Email) : base( O => O.buyerEmail == Email )
        {
            includes.Add( o=> o.deliveryMethod );
            includes.Add( o=> o.Items);
            AddOrederByDesc( o=> o.orderDate );

        }
        public ORderWithSpecs(string Email, int id) : base( O => O.buyerEmail == Email && O.Id == id )
        {
            includes.Add( o=> o.deliveryMethod );
            includes.Add( o=> o.Items);
            AddOrederByDesc( o=> o.orderDate );

        }
    }
}
