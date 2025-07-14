using Microsoft.EntityFrameworkCore;
using Prism.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Repository
{
    public class SampleTestStatusRepository : Repository<LkpSampleTestStatus>, ISampleTestStatusRepository
    {
        public SampleTestStatusRepository(DbContext context) : base(context)
        {
        }
    }
}
