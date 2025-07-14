using Microsoft.EntityFrameworkCore;
using Prism.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Repository
{
    public class SamplerDocumentsRepository : Repository<TblSamplerDocuments>, ISamplerDocumentsRepository
    {
        public SamplerDocumentsRepository(DbContext context) : base(context)
        {
        }
    }
}
