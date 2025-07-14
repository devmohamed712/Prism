using Microsoft.EntityFrameworkCore;
using Prism.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Repository
{
    public class GroupTestsRepository : Repository<LkpGroupTests>, IGroupTestsRepository
    {
        public GroupTestsRepository(DbContext context) : base(context)
        {
        }
    }
}
