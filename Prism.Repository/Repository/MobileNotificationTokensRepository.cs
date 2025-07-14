using Prism.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Repository
{
    public class MobileNotificationTokensRepository : Repository<TblMobileNotificationTokens>, IMobileNotificationTokensRepository
    {
        public MobileNotificationTokensRepository(PrismContext context) : base(context) { }
    }
}
