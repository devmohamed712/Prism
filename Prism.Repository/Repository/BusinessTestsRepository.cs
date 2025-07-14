using Microsoft.EntityFrameworkCore;
using Prism.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Repository
{
    public class BusinessTestsRepository : Repository<TblBusinessTests>, IBusinessTestsRepository
    {
        public BusinessTestsRepository(DbContext context) : base(context)
        {
        }
    }
}
