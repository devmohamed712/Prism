using AutoMapper;
using Prism.BL.Dtos;
using Prism.DAL;
using Prism.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.BL.Managers.Notification
{
    public interface INotificationManager
    {
        public NotificationDtoList GetNotifications(int pageNumber, int pageSize, string role);

        public NotificationDto? GetNotification(int id);

        public NotificationDto CreateOrEditNotification(NotificationDto model);

        public bool DeleteNotification(int id);

        public MobileNotificationTokensDto CreateEditMobileNotificationTokens(MobileNotificationTokensDto model);

        public void SetNotificationToUser(NotificationDto model);

        public NotificationDto Mapping(TblNotifications notificationDB);
    }
}
