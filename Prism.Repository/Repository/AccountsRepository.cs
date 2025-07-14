using Prism.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Repository.Repository
{
    public class AccountsRepository : Repository<TblAccounts>, IAccountsRepository
    {
        public AccountsRepository(PrismContext context) : base(context) { }
    }
}
