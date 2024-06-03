﻿using Ecommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Repositories.Interfaces
{
	public interface IUnitOfWork  
	{
		IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity ;

		Task<int> CompleteAsync();
	}
}
