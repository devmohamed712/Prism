using Microsoft.EntityFrameworkCore;
using Prism.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Repository
{
    public class OrderDetailsRepository : Repository<TblOrderDetails>, IOrderDetailsRepository
    {
        public OrderDetailsRepository(DbContext context) : base(context)
        {
        }
    }
}
