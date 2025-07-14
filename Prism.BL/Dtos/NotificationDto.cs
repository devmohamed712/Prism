using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Prism.BL.Dtos
{
    public class NotificationDto
    {
        public int Id { get; set; }
        [Required]
        public string NotificationText { get; set; }
        [Required]
        public int NotificationTypeId { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        public int? OrderId { get; set; }
        public bool IsDeleted { get; set; }

        public List<string> Roles { get; set; }
        public string NotificationTypeName { get; set; }
    }

    public class NotificationDtoList
    {
        public NotificationDtoList()
        {
            Notifications = new List<NotificationDto>();
        }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool IsLastPage { get; set; }
        public List<NotificationDto> Notifications { get; set; }
    }

    public class UserNotificationsDto
    {
        public int Id { get; set; }
        [Required]
        public int NotificationId { get; set; }
        [Required]
        public string UserId { get; set; }
        public bool IsRead { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class MobileNotificationTokensDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [Required]
        public string DeviceId { get; set; }
        [Required]
        public string DeviceToken { get; set; }
        [Required]
        public int Platform { get; set; }
        public bool IsStoped { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class NotificationMessageDto
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public object Data { get; set; }
    }
}
