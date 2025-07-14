using System;
using System.Collections.Generic;
using System.Text;
using Prism.DAL;

namespace Prism.Repository
{
	public class AspNetUserRepository : Repository<AspNetUsers>, IAspNetUserRepository
	{
		public AspNetUserRepository(PrismContext context) : base(context) { }
	}
}
