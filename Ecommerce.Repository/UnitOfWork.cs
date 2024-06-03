using Ecommerce.Core.Entities;
using Ecommerce.Core.Repositories.Interfaces;
using Ecommerce.Repository.Data;
using Ecommerce.Repository.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly StoreDbContext _context;
        private Hashtable _repos { get; set; }
        public UnitOfWork( StoreDbContext context )
		{
			this._context = context;
			_repos = new Hashtable();
		}

		public async Task<int> CompleteAsync()
		{
			return await _context.SaveChangesAsync();
		}

		//public async ValueTask DisposeAsync() =>await _context.DisposeAsync();

		public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
		{
			var type = typeof(TEntity).Name;
			if (!_repos.ContainsKey(type))
			{
				var repository = new GenericRepositories<TEntity>(_context);
				_repos.Add(type, repository);
			}
			return _repos[type] as IGenericRepository<TEntity> ;
		}
	}
}
