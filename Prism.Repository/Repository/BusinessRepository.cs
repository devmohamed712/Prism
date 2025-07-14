using Prism.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Repository
{
    public class BusinessRepository : Repository<TblBusinesses>, IBusinessRepository
    {
        public BusinessRepository(PrismContext context) : base(context) { }
    }
}
