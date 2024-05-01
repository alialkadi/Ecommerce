using Ecommerce.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Repository.Data.Configuration
{
	public class ProductConfiguration : IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> builder)
		{
			builder.Property(E => E.Name).IsRequired();
			builder.Property(E => E.PictureUrl).IsRequired();
			builder.Property(E => E.Description).IsRequired();
			builder.Property(E => E.Price).IsRequired().HasColumnType("decimal(18,2)");

			builder.HasOne(E=> E.Brand).WithMany().HasForeignKey(b => b.BrandId).OnDelete(DeleteBehavior.ClientSetNull);
			builder.HasOne(E=> E.Category).WithMany().HasForeignKey(b => b.CategoryId).OnDelete(DeleteBehavior.ClientSetNull);


			//builder.Navigation(e => e.Category).AutoInclude();

		}
	}
}
