using Microsoft.EntityFrameworkCore;
using Prism.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Repository
{
    public class NotificationTypesRepository : Repository<LkpNotificationTypes>, INotificationTypesRepository
    {
        public NotificationTypesRepository(DbContext context) : base(context)
        {
        }
    }
}
