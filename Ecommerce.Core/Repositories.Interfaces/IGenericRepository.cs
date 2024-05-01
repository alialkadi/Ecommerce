using Ecommerce.Core.Entities;
using Ecommerce.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Repositories.Interfaces
{
	public interface IGenericRepository<T> where T : BaseEntity
	{
		// GetAll
		Task<IReadOnlyList<T>> GetAllAsync();
		// GEtById
		Task<T?> GetAsync(int id);


		// GetAll WithSpec
		Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T>  spec);
		// GEtById WithSpec
		Task<T?> GetWithSpecAsync(ISpecifications<T> spec);
		Task<int> GetCountAsync( ISpecifications<T> specifications );
	}
}
