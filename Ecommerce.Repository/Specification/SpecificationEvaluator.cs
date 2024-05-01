using Ecommerce.Core.Entities;
using Ecommerce.Core.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Repository.Specification
{
	public class SpecificationEvaluator<T> where T : BaseEntity 
	{
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecifications<T> specs)
		{
			var query = inputQuery; // _context.set<T>() 
			if (specs.criteria is not null)
			{
				query = query.Where(specs.criteria);  // _context.Set<T>.where(P=> p.id = 10) 
			}
			if(specs.OrdersBy is not null)
				query = query.OrderBy(specs.OrdersBy);

			if(specs.OrdersByDesc is not null)
				query = query.OrderByDescending(specs.OrdersByDesc);

			if (specs.isPaginationEnabled)
			{
				query = query.Skip(specs.skip).Take(specs.take);
			}

			query = specs.includes.Aggregate(query, (currentQuery , includeExpression) => currentQuery.Include(includeExpression));
			return query;
		}
    }
}
