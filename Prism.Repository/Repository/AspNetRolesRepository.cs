using Prism.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Repository
{
    public class AspNetRolesRepository : Repository<AspNetRoles>, IAspNetRolesRepository
    {
        public AspNetRolesRepository(PrismContext context) : base(context) { }
    }
}
