using Prism.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Repository
{
    public class TestSubTypesRepository : Repository<LkpTestSubTypes>, ITestSubTypesRepository
    {
        public TestSubTypesRepository(PrismContext context) : base(context) { }
    }
}
