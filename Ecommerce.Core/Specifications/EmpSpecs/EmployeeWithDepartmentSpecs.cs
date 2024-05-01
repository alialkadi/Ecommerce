using Ecommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Specifications.EmpSpecs
{
	public class EmployeeWithDepartmentSpecs : BaseSpecifications<Employee>
	{
		public EmployeeWithDepartmentSpecs() : base() 
		{
			includes.Add(E=>E.Department);
		}
		public EmployeeWithDepartmentSpecs(int id) : base(E=>E.Id == id) 
		{
			includes.Add(E=>E.Department);
		}
	}
}
