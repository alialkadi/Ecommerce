using Ecommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Specifications
{
	public class BaseSpecifications<T> : ISpecifications<T> where T : BaseEntity
	{
        public Expression<Func<T, bool>> criteria { get; set; } = null!;
        public List<Expression<Func<T, object>>> includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrdersBy { get; set; } = null!;
		public Expression<Func<T, object>> OrdersByDesc { get; set; } = null!;
		public int skip { get ; set ; }
		public int take { get ; set ; }
		public bool isPaginationEnabled { get ; set ; }

		public BaseSpecifications()
        {
        //    includes = new List<Expression<Func<T, object>>>();
        //    criteria = null;
        }
        public BaseSpecifications(Expression<Func<T, bool>> expression)
        {
            
            criteria = expression; //Example:    P=> P.id == 10
        }
        public void AddOrederBy(Expression<Func<T, object>> OrderByExpression)
        {
            OrdersBy = OrderByExpression;
        }
        public void AddOrederByDesc(Expression<Func<T, object>> OrderByDescExpression)
        {
            OrdersByDesc = OrderByDescExpression;
        }

        public void applyPagination(int skip, int take)
        {
            this.skip = skip;
            this.take = take;
            isPaginationEnabled = true;
        }
    }
}
