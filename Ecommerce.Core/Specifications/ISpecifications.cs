using Ecommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Specifications
{
	public interface ISpecifications<T> where T : BaseEntity
	{
		public Expression<Func<T, bool>> criteria { get; set; }
		public List<Expression<Func<T, object>>> includes { get; set;}
        public Expression<Func<T,object>> OrdersBy { get; set; }
        
        public Expression<Func<T,object>> OrdersByDesc { get; set; }
        public int skip { get; set; }
        public int take { get; set; }
        public bool isPaginationEnabled { get; set; }

    }
}


//_context.Products.Include(P=>P.Brand).Include(P=>P.Category).ToListAsync();
//_context.Products.Where(p=>p.Id == id).Include(P => P.Brand).Include(P => P.Category).FirstOrDefaultAsync() as T;