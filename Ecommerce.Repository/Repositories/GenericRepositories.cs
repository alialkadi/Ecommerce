using Ecommerce.Core.Entities;
using Ecommerce.Core.Repositories.Interfaces;
using Ecommerce.Core.Specifications;
using Ecommerce.Repository.Data;
using Ecommerce.Repository.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Repository.Repositories
{
	public class GenericRepositories<T> : IGenericRepository<T> where T : BaseEntity
	{
		private readonly StoreDbContext _context;

		public GenericRepositories( StoreDbContext context  )
        {
			_context = context;
		}


        public async Task<IReadOnlyList<T>> GetAllAsync()
		{
			if(typeof(T)== typeof(Product))
			{
				return (IReadOnlyList<T>) await _context.Products.Include(P=>P.Brand).Include(P=>P.Category).ToListAsync();
			}
			return await _context.Set<T>().ToListAsync();
		}

		public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec )
		{
			return await SpecificationEvaluator<T>.GetQuery(_context.Set<T>(), spec ).ToListAsync();

		}

		public async Task<T?> GetAsync(int id)
		{
			if (typeof(T) == typeof(Product))
			{
				return await _context.Products.Where(p=>p.Id == id).Include(P => P.Brand).Include(P => P.Category).FirstOrDefaultAsync() as T;
			}
			return await _context.Set<T>().FindAsync(id); 
		}

		public async Task<int> GetCountAsync(ISpecifications<T> specifications)
		{
			return  await ApplySpecs( specifications).CountAsync();
		}

		public async Task<T?> GetWithSpecAsync(ISpecifications<T> spec)
		{
			return await SpecificationEvaluator<T>.GetQuery(_context.Set<T>(), spec).FirstOrDefaultAsync();
		}
		private IQueryable<T?> ApplySpecs(ISpecifications<T> spec)
		{
			return  SpecificationEvaluator<T>.GetQuery(_context.Set<T>(), spec);
		}

		
	}
}
