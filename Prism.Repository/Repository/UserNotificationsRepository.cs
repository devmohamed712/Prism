using Microsoft.EntityFrameworkCore;
using Prism.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Repository
{
    public class UserNotificationsRepository : Repository<TblUserNotifications>, IUserNotificationsRepository
    {
        public UserNotificationsRepository(DbContext context) : base(context)
        {
        }
    }
}
