using Ecommerce.Core.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Repository.Data.Configuration
{
	public class OrderConfiguration : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.Property(O=>O.subTotal).HasColumnType("decimal(18,2)");
			builder.Property(O=>O.orderStatus).HasConversion(Ostatus => Ostatus.ToString(), 
															 Ostatus => (OrderStatus)Enum.Parse(typeof(OrderStatus),Ostatus));

			builder.OwnsOne(o => o.shippingAddress, SA => SA.WithOwner()); 
			builder.HasOne(o=> o.deliveryMethod).WithMany().OnDelete(DeleteBehavior.NoAction);

		}
	}
}
