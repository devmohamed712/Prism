using Microsoft.EntityFrameworkCore;
using Prism.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Repository
{
    public class SamplerDocumentTypesRepository : Repository<LkbSamplerDocumentTypes>, ISamplerDocumentTypesRepository
    {
        public SamplerDocumentTypesRepository(DbContext context) : base(context)
        {
        }
    }
}
