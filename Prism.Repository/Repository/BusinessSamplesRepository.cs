using Microsoft.EntityFrameworkCore;
using Prism.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Repository
{
    public class BusinessSamplesRepository : Repository<TblBusinessSamples>, IBusinessSamplesRepository
    {
        public BusinessSamplesRepository(DbContext context) : base(context)
        {
        }
    }
}
