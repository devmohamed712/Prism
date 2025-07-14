using Prism.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Repository
{
    public class TestTypesRepository : Repository<LkpTestTypes>, ITestTypesRepository
    {
        public TestTypesRepository(PrismContext context) : base(context) { }
    }
}
