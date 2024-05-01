using Ecommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Specifications.ProductSpecs
{
	public class ProductWithBrandsAndCategorySpecs :BaseSpecifications<Product>
	{
        // constructor to be used for creating object for getAllProducts
        public ProductWithBrandsAndCategorySpecs(ProductSpecParams productSpec)
                                                : base(P =>
                                                (string.IsNullOrEmpty(productSpec.Search) || P.Name.ToLower().Contains(productSpec.Search) )
                                                &&
                                                (!productSpec.BrandId.HasValue || P.BrandId ==  productSpec.BrandId.Value)
                                                &&
                                                (!productSpec.CategoryId.HasValue || P.CategoryId == productSpec.CategoryId.Value))
        {
            includes.Add(P=>P.Brand); 
            includes.Add(P=>P.Category);

            if (!string.IsNullOrEmpty(productSpec.sort))
            {
                switch (productSpec.sort)
                {
                    case "priceAsc":
                        AddOrederBy(p=>p.Price);
                        break;
                    case "priceDesc":
                        AddOrederByDesc(p=>p.Price);
                        break;
                    default:
                        AddOrederBy(p=> p.Name);
                        break;

                }
            }
            else
            {
                AddOrederBy(p=>p.Id);
            }
        
            applyPagination((productSpec.pagesize * (productSpec.pageIndex-1)),productSpec.pagesize);
        }


		public ProductWithBrandsAndCategorySpecs(int id) : base( p=>p.Id == id  )
        {
			includes.Add(P => P.Brand);
			includes.Add(P => P.Category);
		}

    }
}
