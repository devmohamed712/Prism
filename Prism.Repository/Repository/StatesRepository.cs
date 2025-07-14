using Microsoft.EntityFrameworkCore;
using Prism.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Repository
{
    public class StatesRepository : Repository<LkpStates>, IStatesRepository
    {
        public StatesRepository(DbContext context) : base(context)
        {
        }
    }
}
