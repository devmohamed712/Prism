using Prism.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Repository
{
    public class OrderRepository : Repository<TblOrders>, IOrderRepository
    {
        public OrderRepository(PrismContext context) : base(context) { }
    }
}
