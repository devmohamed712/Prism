using Prism.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Repository
{
    public class SampleTypesRepository : Repository<LkpSampleTypes>, ISampleTypesRepository
    {
        public SampleTypesRepository(PrismContext context) : base(context) { }
    }
}
