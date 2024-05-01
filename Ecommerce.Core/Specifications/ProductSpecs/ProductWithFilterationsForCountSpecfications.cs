using Ecommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Specifications.ProductSpecs
{
	public class ProductWithFilterationsForCountSpecfications : BaseSpecifications<Product>
	{
        public ProductWithFilterationsForCountSpecfications( ProductSpecParams productSpec) : base(P =>
												(string.IsNullOrEmpty(productSpec.Search) || P.Name.ToLower().Contains(productSpec.Search))
												&&
												(!productSpec.BrandId.HasValue || P.BrandId == productSpec.BrandId.Value)
												&&
												(!productSpec.CategoryId.HasValue || P.CategoryId == productSpec.CategoryId.Value))
		{
            
        }
    }
}
