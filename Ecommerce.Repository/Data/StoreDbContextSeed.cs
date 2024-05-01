using Ecommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommerce.Repository.Data
{
	public static class StoreDbContextSeed
	{
		public static async Task SeedAsync( StoreDbContext _context )
		{
			// Get Data From JSon file 

			var productsData = File.ReadAllText("../Ecommerce.Repository/Data/DataSeeding/products.json");
			var BrandssData = File.ReadAllText("../Ecommerce.Repository/Data/DataSeeding/brands.json");
			var CategoriesData = File.ReadAllText("../Ecommerce.Repository/Data/DataSeeding/categories.json");

            Console.WriteLine(productsData);

			// convert json to Wanted type
			if (_context.ProductBrands.Count() == 0)
			{

				var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandssData);

				if (Brands?.Count() > 0)
				{
					foreach (var p in Brands)
					{
						_context.Set<ProductBrand>().Add(p);
					}
					await _context.SaveChangesAsync();
				}
			}

			if (_context.ProductCategories.Count() == 0)
			{
				var Categories = JsonSerializer.Deserialize<List<ProductCategory>>(CategoriesData);
				if (Categories?.Count() > 0)
				{
					foreach (var p in Categories)
					{
						_context.Set<ProductCategory>().Add(p);
					}
					await _context.SaveChangesAsync();
				}
			}


			if (_context.Products.Count() == 0)
			{
				var products = JsonSerializer.Deserialize<List<Product>>(productsData);
				if (products?.Count() > 0)
				{
					foreach (var p in products)
					{
						_context.Set<Product>().Add(p);
					}
					await _context.SaveChangesAsync();
				}
			}


        }
	}
}
