using Prism.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Repository
{
    public class BusinessContactsRepository : Repository<TblBusinessContacts>, IBusinessContactsRepository
    {
        public BusinessContactsRepository(PrismContext context) : base(context) { }
    }
}
