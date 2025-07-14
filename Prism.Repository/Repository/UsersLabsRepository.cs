using System;
using System.Collections.Generic;
using System.Text;
using Prism.DAL;

namespace Prism.Repository
{
	public class UsersLabsRepository : Repository<TblUsersLabs>, IUsersLabsRepository
	{
		public UsersLabsRepository(PrismContext context) : base(context) { }
	}
}
