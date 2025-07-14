using Microsoft.EntityFrameworkCore;
using Prism.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Repository
{
    public class NotificationsRepository : Repository<TblNotifications>, INotificationsRepository
    {
        public NotificationsRepository(DbContext context) : base(context)
        {
        }
    }
}
