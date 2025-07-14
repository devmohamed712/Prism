using Prism.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Repository
{
    public class AboutUsRepository : Repository<LkpAboutUs>, IAboutUsRepository
    {
        public AboutUsRepository(PrismContext context) : base(context) { }
    }
}
