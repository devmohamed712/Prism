using Microsoft.EntityFrameworkCore;
using Prism.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Repository
{
    public class OrderSampleTestsRepository : Repository<TblOrderSampleTests>, IOrderSampleTestsRepository
    {
        public OrderSampleTestsRepository(DbContext context) : base(context)
        {
        }
    }
}
