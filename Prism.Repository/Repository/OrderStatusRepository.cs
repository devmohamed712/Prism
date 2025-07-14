using Microsoft.EntityFrameworkCore;
using Prism.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Repository
{
    public class OrderStatusRepository : Repository<LkpOrderStatus>, IOrderStatusRepository
    {
        public OrderStatusRepository(DbContext context) : base(context)
        {
        }
    }
}
