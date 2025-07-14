using System;
using System.Collections.Generic;
using System.Text;
using Prism.DAL;

namespace Prism.Repository
{
	public class CustomerResultsRepository : Repository<TblCustomerResults>, ICustomerResultsRepository
	{
		public CustomerResultsRepository(PrismContext context) : base(context) { }
	}
}
