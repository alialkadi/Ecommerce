using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Specifications.ProductSpecs
{
	public class ProductSpecParams
	{
        public string? sort { get; set; }
        
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public int pageIndex { get; set; } = 1;

		private const int maxPageSize = 10;
		private int pageSize = 5;

		public int pagesize
		{
			get { return pageSize; }
			set { pageSize = value > maxPageSize ? maxPageSize : value ; }
		}

		private string? search;

		public string? Search
		{
			get { return search; }
			set { search = value?.ToLower(); }
		}



	}
}
