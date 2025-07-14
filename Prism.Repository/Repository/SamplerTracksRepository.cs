using Microsoft.EntityFrameworkCore;
using Prism.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Repository
{
    public class SamplerTracksRepository : Repository<TblSamplerTracks>, ISamplerTracksRepository
    {
        public SamplerTracksRepository(DbContext context) : base(context)
        {
        }
    }
}
