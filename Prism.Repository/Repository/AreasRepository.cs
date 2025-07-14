using Microsoft.EntityFrameworkCore;
using Prism.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Repository
{
    public class AreasRepository : Repository<LkpAreas>, IAreasRepository
    {
        public AreasRepository(DbContext context) : base(context)
        {
        }
    }
}
