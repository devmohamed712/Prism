using AutoMapper;
using Hangfire;
using MailKit.Search;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Prism.BL.Configrations;
using Prism.BL.Dtos;
using Prism.BL.Managers.Business;
using Prism.BL.Managers.Common;
using Prism.BL.Managers.Order.OrderDetails;
using Prism.BL.Managers.Order.OrderSamples;
using Prism.BL.Managers.Utilities;
using Prism.DAL;
using Prism.Repository;
using QRCodeResults.BL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Prism.BL.Managers.Notification
{
    public class NotificationManager : INotificationManager
    {
        public readonly IMapper _mapper;
        public readonly IConfiguration _configuration;
        public readonly IUnitOfWork _unitOfWork;
        public readonly IUtilitiesManager _utilitiesManager;

        public NotificationManager(IMapper mapper, IConfiguration configuration, IUnitOfWork unitOfWork, IUtilitiesManager utilitiesManager)
        {
            _mapper = mapper;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _utilitiesManager = utilitiesManager;
        }

        public NotificationDtoList GetNotifications(int pageNumber, int pageSize, string role)
        {
            NotificationDtoList modelList = new NotificationDtoList();
            modelList.Notifications = new List<NotificationDto>();
            IEnumerable<TblNotifications> notificationsDB = _unitOfWork.Notifications.GetByPageOrderByDesc(x => !x.IsDeleted && x.UserNotifications.Any(c => c.Account.AspNetUser.AspNetUserRoles.Any(v => v.Role.Name.Equals(role))), pageNumber, pageSize, x => x.Id);
            foreach (var notificationDB in notificationsDB)
            {
                modelList.Notifications.Add(Mapping(notificationDB));
            }
            modelList.PageNumber = pageNumber;
            modelList.IsLastPage = notificationsDB != null && notificationsDB.Count() < pageSize ? true : false;
            return modelList;
        }

        public NotificationDto? GetNotification(int id)
        {
            NotificationDto? model = null;
            var notificationDB = _unitOfWork.Notifications.FirstOrDefault(c => !c.IsDeleted && c.Id == id);
            if (notificationDB != null)
            {
                model = Mapping(notificationDB);
            }
            return model;
        }

        public NotificationDto CreateOrEditNotification(NotificationDto model)
        {
            TblNotifications? notificationDB = null;
            if (model.Id > 0)
            {
                notificationDB = _unitOfWork.Notifications.FirstOrDefault(c => !c.IsDeleted && c.Id == model.Id);
                if (notificationDB != null)
                {
                    _mapper.Map(model, notificationDB);
                }
            }
            else
            {
                notificationDB = _mapper.Map<TblNotifications>(model);
                _unitOfWork.Notifications.Add(notificationDB);
                model.Id = notificationDB.Id;
            }
            _unitOfWork.Complete();
            return model;

        }

        public bool DeleteNotification(int id)
        {
            var notificationDB = _unitOfWork.Notifications.FirstOrDefault(c => c.Id == id);
            int result = 0;
            if (notificationDB != null)
            {
                notificationDB.IsDeleted = true;
                result = _unitOfWork.Complete();
            }
            return result > 0 ? true : false;
        }

        public MobileNotificationTokensDto CreateEditMobileNotificationTokens(MobileNotificationTokensDto model)
        {
            TblMobileNotificationTokens mobileNotificationTokenDB = _unitOfWork.MobileNotificationTokens.FirstOrDefault(x => !x.IsDeleted && x.DeviceId.Equals(model.DeviceId) && x.UserId.Equals(model.UserId));
            if (mobileNotificationTokenDB != null)
            {
                mobileNotificationTokenDB.DeviceToken = model.DeviceToken;
            }
            else
            {
                mobileNotificationTokenDB = _mapper.Map<TblMobileNotificationTokens>(model);
                _unitOfWork.MobileNotificationTokens.Add(mobileNotificationTokenDB);
            }
            _unitOfWork.Complete();
            model.Id = mobileNotificationTokenDB.Id;
            return model;

        }

        public void SetNotificationToUser(NotificationDto model)
        {
            model = CreateOrEditNotification(model);
            if (model.Roles != null)
            {
                model.Roles.ForEach(role =>
                {
                    string roleId = _unitOfWork.AspNetRoles.FirstOrDefault(x => x.Name.Equals(role))?.Id;
                    if (!string.IsNullOrEmpty(roleId))
                    {
                        #region Set Notification To Users
                        var users = _unitOfWork.Accounts.FindList(x => !x.IsDeleted && x.IsActive && x.AspNetUser.AspNetUserRoles.Any(v => v.RoleId == roleId));
                        foreach (var user in users)
                        {
                            TblUserNotifications userNotificationsDB = new TblUserNotifications
                            {
                                NotificationId = model.Id,
                                UserId = user.Id,
                                IsRead = false
                            };
                            _unitOfWork.UserNotifications.Add(userNotificationsDB);
                            _unitOfWork.Complete();
                        }
                        #endregion
                        #region Push Notification
                        var roleMobilesTokens = _unitOfWork.MobileNotificationTokens.FindList(x => !x.IsDeleted && !x.Account.IsDeleted && x.Account.IsActive && x.Account.AspNetUser.AspNetUserRoles.Any(c => c.Role.Name.Equals(role))).ToList();
                        roleMobilesTokens.ForEach(mobileToken =>
                        {
                            NotificationMessageDto notifMessage = new NotificationMessageDto
                            {
                                Message = model.NotificationText,
                                Token = mobileToken.DeviceToken,
                                Data = new { NotificationTypeName = model.NotificationTypeName, Notification = model }
                            };
                            try
                            {
                                PushNotification(notifMessage);
                            }
                            catch (Exception e)
                            {
                            }
                        });
                        #endregion
                    }
                });

            }
        }

        private void PushNotification(NotificationMessageDto model)
        {
            string cloudMessagingServerkey = _configuration["Notification:CloudMessagingServerkey"];
            string cloudMessagingSenderID = _configuration["Notification:CloudMessagingSenderID"];
            string cloudMessagingUrl = _configuration["Notification:CloudMessagingUrl"];

            WebRequest tRequest = WebRequest.Create(cloudMessagingUrl);
            tRequest.Method = "post";
            tRequest.ContentType = "application/json";
            var objNotification = new
            {
                notification = new
                {
                    title = "You have a new notification",
                    body = model.Message,
                    color = "#00812a"
                },
                data = new
                {
                    model.Data
                },
                to = model.Token
            };
            string jsonNotificationFormat = JsonConvert.SerializeObject(objNotification);
            Byte[] byteArray = Encoding.UTF8.GetBytes(jsonNotificationFormat);
            tRequest.Headers.Add(string.Format("Authorization: key={0}", cloudMessagingServerkey));
            tRequest.Headers.Add(string.Format("Sender: id={0}", cloudMessagingSenderID));
            tRequest.ContentLength = byteArray.Length;
            tRequest.ContentType = "application/json";
            Stream dataStream = tRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            WebResponse tResponse = tRequest.GetResponse();
            Stream dataStreamResponse = tResponse.GetResponseStream();
        }

        public NotificationDto Mapping(TblNotifications notificationDB)
        {
            NotificationDto model = new NotificationDto();
            model = _mapper.Map<NotificationDto>(notificationDB);
            return model;
        }
    }
}
