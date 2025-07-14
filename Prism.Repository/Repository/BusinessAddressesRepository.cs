using Microsoft.EntityFrameworkCore;
using Prism.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Repository
{
    public class BusinessAddressesRepository : Repository<TblBusinessAddresses>, IBusinessAddressesRepository
    {
        public BusinessAddressesRepository(DbContext context) : base(context)
        {
        }
    }
}
