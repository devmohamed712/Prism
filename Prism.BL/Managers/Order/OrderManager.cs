using AutoMapper;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Prism.BL.Configrations;
using Prism.BL.Dtos;
using Prism.BL.Managers.Business;
using Prism.BL.Managers.Business.BasinessSamples;
using Prism.BL.Managers.Business.BusinessAddresses;
using Prism.BL.Managers.Business.BusinessContacts;
using Prism.BL.Managers.Business.BusinessTests;
using Prism.BL.Managers.Common;
using Prism.BL.Managers.Notification;
using Prism.BL.Managers.Order.OrderDetails;
using Prism.BL.Managers.User;
using Prism.BL.Managers.Utilities;
using Prism.DAL;
using Prism.Repository;
using QRCodeResults.BL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.BL.Managers.Order
{
    public class OrderManager : IOrderManager
    {
        public readonly IMapper _mapper;
        public readonly IConfiguration _configuration;
        public readonly IUnitOfWork _unitOfWork;
        public readonly ICommonManager _commonManager;
        public readonly IBusinessManager _businessManager;
        public readonly IOrderDetailsManager _orderDetailsManager;
        public readonly INotificationManager _notificationManager;
        public readonly IUtilitiesManager _utilitiesManager;
        public readonly IUserManager _userManager;

        public OrderManager(IMapper mapper, IConfiguration configuration, IUnitOfWork unitOfWork, ICommonManager commonManager, IBusinessManager businessManager, IOrderDetailsManager orderDetailsManager, INotificationManager notificationManager, IUtilitiesManager utilitiesManager, IUserManager userManager)
        {
            _mapper = mapper;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _commonManager = commonManager;
            _businessManager = businessManager;
            _orderDetailsManager = orderDetailsManager;
            _notificationManager = notificationManager;
            _utilitiesManager = utilitiesManager;
            _userManager = userManager;
        }

        public OrderDtoList GetOrders(int pageNumber, int pageSize, List<int>? statusIds = null, string? userId = null, string? role = null)
        {
            OrderDtoList modelList = new OrderDtoList();
            modelList.Orders = new List<OrderDto>();
            List<OrderStatusDto> orderStatus = _commonManager.GetOrderStatusList();
            Func<TblOrders, bool> predicates = x => !x.IsDeleted && !x.IsCanceled;
            if (!string.IsNullOrEmpty(role))
            {
                if (!string.IsNullOrEmpty(role) && role.Equals(Roles.Sampler) && !string.IsNullOrEmpty(userId))
                {
                    if (!statusIds.Contains(orderStatus.FirstOrDefault(x => x.Name.Equals(OrderStatus.Placed)).Id))
                    {
                        predicates = predicates.AndAlso(x => x.SamplerId != null && x.SamplerId.Equals(userId));
                    }
                }
                else if (!string.IsNullOrEmpty(role) && role.Equals(Roles.QA) && !string.IsNullOrEmpty(userId))
                {
                    predicates = predicates.AndAlso(x => x.QAId == null || (x.QAId != null && x.QAId.Equals(userId)));
                }
            }
            if (statusIds != null)
            {
                predicates = predicates.AndAlso(x => x.OrderDetails.Count() > 0 && statusIds.Contains(x.OrderDetails.LastOrDefault().StatusId));
            }
            IEnumerable<TblOrders> ordersDB = _unitOfWork.Order.FindByPage(predicates, pageNumber, pageSize);
            foreach (var orderDB in ordersDB)
            {
                modelList.Orders.Add(Mapping(orderDB, role, userId));
            }
            if (!string.IsNullOrEmpty(role) && role.Equals(Roles.Sampler) && !string.IsNullOrEmpty(userId) && pageNumber == 1)
            {
                foreach (var statusId in statusIds)
                {
                    OrderStatusDto selectedStatus = orderStatus.FirstOrDefault(x => x.Id == statusId);
                    if (!selectedStatus.Name.Equals(OrderStatus.Accepted))
                    {
                        modelList.IsSamplerHasOrder = _unitOfWork.Order.Any(x => !x.IsDeleted && !x.IsCanceled && x.Sampler != null && x.SamplerId.Equals(userId) && ((x.OrderDetails.LastOrDefault().OrderStatus.Name.Equals(OrderStatus.Accepted) || x.OrderDetails.LastOrDefault().OrderStatus.Name.Equals(OrderStatus.PickedUp))));
                    }
                }
            }
            modelList.PageNumber = pageNumber;
            modelList.IsLastPage = ordersDB != null && ordersDB.Count() < pageSize ? true : false;
            return modelList;
        }

        public OrderDto GetOrder(int id, string? role = null, string? userId = null)
        {
            OrderDto model = new OrderDto();
            var orderDB = _unitOfWork.Order.FirstOrDefault(c => !c.IsDeleted && c.Id == id);
            if (orderDB != null)
            {
                model = Mapping(orderDB, role, userId);
            }
            return model;
        }

        public OrderDto CreateOrEditOrder(OrderDto model)
        {
            TblOrders? orderDB = null;
            if (model.Id > 0)
            {
                orderDB = _unitOfWork.Order.FirstOrDefault(c => !c.IsDeleted && c.Id == model.Id);
                if (orderDB != null)
                {
                    _mapper.Map(model, orderDB);
                }
            }
            else
            {
                orderDB = _mapper.Map<TblOrders>(model);
                _unitOfWork.Order.Add(orderDB);
            }
            _unitOfWork.Complete();
            if (model.Id == 0)
            {
                List<OrderStatusDto> statusLst = _commonManager.GetOrderStatusList();
                int statusId = statusLst.FirstOrDefault(x => x.Name.Equals("Placed")).Id;
                DateTime currentTime = DateTime.UtcNow;
                orderDB.Name = SetOrderName(orderDB.Id, currentTime);
                _orderDetailsManager.CreateOrderDetails(orderDB.Id, statusId, currentTime);
                var notificationTypes = _unitOfWork.NotificationTypes.FindList(x => !x.IsDeleted);
                NotificationDto notification = new NotificationDto()
                {
                    NotificationText = "Order " + orderDB.Name + " is created and awaiting to be picked up",
                    NotificationTypeId = notificationTypes.FirstOrDefault(x => x.Name.Equals(NotificationTypes.ANewOrderIsCreated)).Id,
                    Roles = new List<string>() { Roles.Sampler },
                    DateTime = DateTime.UtcNow,
                    NotificationTypeName = NotificationTypes.ANewOrderIsCreated
                };
                _notificationManager.SetNotificationToUser(notification);
                BackgroundJob.Schedule(() => GetUnAcceptedOrder(orderDB.Id), TimeSpan.FromMinutes(1));
            }
            model.Id = orderDB.Id;
            return model;

        }

        private string SetOrderName(int orderId, DateTime currentTime)
        {
            var newOrderDB = _unitOfWork.Order.FirstOrDefault(x => x.Id == orderId);
            if (newOrderDB != null)
            {
                string firstPart = newOrderDB.Business.EntityName.Substring(0, 2);
                string secPart = currentTime.ToString("dd/MM/yyyy");
                newOrderDB.Name = firstPart.ToUpper() + "-" + secPart + "-" + newOrderDB.Id.ToString();
                _unitOfWork.Complete();
                return newOrderDB.Name;
            }
            return string.Empty;
        }

        public bool Delete(int id)
        {
            var orderDB = _unitOfWork.Order.FirstOrDefault(c => !c.IsDeleted && c.Id == id);
            int result = 0;
            if (orderDB != null)
            {
                orderDB.IsDeleted = true;
                result = _unitOfWork.Complete();
            }
            return result > 0 ? true : false;

        }

        public bool Cancel(int id)
        {
            var orderDB = _unitOfWork.Order.FirstOrDefault(c => !c.IsDeleted && c.Id == id);
            int result = 0;
            if (orderDB != null)
            {
                orderDB.IsCanceled = true;
                result = _unitOfWork.Complete();
            }
            return result > 0 ? true : false;
        }

        public OrderDto? GetUnAcceptedOrder(int orderId)
        {
            OrderDto? model = null;
            var orderDB = _unitOfWork.Order.FirstOrDefault(c => !c.IsDeleted && !c.IsCanceled && c.Id == orderId && c.OrderDetails.LastOrDefault().OrderStatus.Name.Equals(OrderStatus.Placed));
            if (orderDB != null)
            {
                model = Mapping(orderDB);
                var notificationTypes = _unitOfWork.NotificationTypes.FindList(x => !x.IsDeleted);
                NotificationDto notification = new NotificationDto()
                {
                    NotificationText = "The order with ID " + orderDB.Name + " was not accepted by anyone",
                    NotificationTypeId = notificationTypes.FirstOrDefault(x => x.Name.Equals(NotificationTypes.OrderIsNottAcceptedByAnySamplerWithinTwoHours)).Id,
                    Roles = new List<string>() { Roles.Admin, Roles.LabAssistant },
                    DateTime = DateTime.UtcNow,
                    OrderId = orderId,
                    NotificationTypeName = NotificationTypes.OrderIsNottAcceptedByAnySamplerWithinTwoHours
                };
                _notificationManager.SetNotificationToUser(notification);
            }
            return model;
        }

        public OrderDto? GetOrderBySamplerId(string samplerId, bool isConfirmationPage = false)
        {
            OrderDto? model = null;
            var orderDB = _unitOfWork.Order.FirstOrDefault(x => !x.IsDeleted && !x.IsCanceled && x.Sampler != null && x.SamplerId.Equals(samplerId)
            && ((x.OrderDetails.LastOrDefault().OrderStatus.Name.Equals(OrderStatus.Accepted) ||
            x.OrderDetails.LastOrDefault().OrderStatus.Name.Equals(OrderStatus.PickedUp) ||
            (x.OrderDetails.LastOrDefault().OrderStatus.Name.Equals(OrderStatus.DroppedOff) && isConfirmationPage)
            )));
            if (orderDB != null)
            {
                model = Mapping(orderDB, Roles.Sampler, samplerId);
            }
            return model;
        }

        public bool AcceptOrder(int orderId, string samplerId, decimal latitude, decimal longitude)
        {
            var orderStatus = _unitOfWork.OrderStatus.FindList(x => !x.IsDeleted);
            var orderDB = _unitOfWork.Order.FirstOrDefault(c => !c.IsDeleted && !c.IsCanceled && c.Id == orderId);
            if (orderDB != null)
            {
                orderDB.SamplerId = samplerId;
                _unitOfWork.Complete();
                int statusId = orderStatus.FirstOrDefault(x => x.Name.Equals(OrderStatus.Accepted)).Id;
                _orderDetailsManager.CreateOrderDetails(orderDB.Id, statusId, DateTime.UtcNow);
                TblAccounts sampler = _unitOfWork.Accounts.FirstOrDefault(x => !x.IsDeleted && x.IsActive && x.Id == samplerId);
                if (orderDB.Id > 0)
                {
                    var notificationTypes = _unitOfWork.NotificationTypes.FindList(x => !x.IsDeleted);
                    NotificationDto notification = new NotificationDto()
                    {
                        NotificationText = "Order " + orderDB.Name + " was accepted by " + _utilitiesManager.ToTitleCase(sampler.Name) + "",
                        NotificationTypeId = notificationTypes.FirstOrDefault(x => x.Name.Equals(NotificationTypes.DriverAcceptsAnOrder)).Id,
                        Roles = new List<string>() { Roles.Admin, Roles.LabAssistant },
                        DateTime = DateTime.UtcNow,
                        NotificationTypeName = NotificationTypes.DriverAcceptsAnOrder
                    };
                    _notificationManager.SetNotificationToUser(notification);
                    SamplerTracksDto track = new SamplerTracksDto
                    {
                        OrderId = orderId,
                        DateTime = DateTime.UtcNow,
                        Latitude = latitude,
                        Longitude = longitude,
                        StatusId = statusId,
                        IsDeleted = false
                    };
                    _userManager.AddSamplerTrack(track, samplerId);
                    return true;
                }
            }
            return false;
        }

        public bool PickUpOrDropOffOrder(int orderId, string samplerId, bool isPickedUp)
        {
            var orderStatus = _unitOfWork.OrderStatus.FindList(x => !x.IsDeleted);
            var orderDB = _unitOfWork.Order.FirstOrDefault(c => !c.IsDeleted && !c.IsCanceled && c.Id == orderId && c.SamplerId.Equals(samplerId));
            if (orderDB != null)
            {
                int statusId = orderStatus.FirstOrDefault(x => x.Name.Equals(isPickedUp ? OrderStatus.PickedUp : OrderStatus.DroppedOff)).Id;
                _orderDetailsManager.CreateOrderDetails(orderDB.Id, statusId, DateTime.UtcNow);
                if (!isPickedUp)
                {
                    TblAccounts sampler = _unitOfWork.Accounts.FirstOrDefault(x => !x.IsDeleted && x.IsActive && x.Id == samplerId);
                    var notificationTypes = _unitOfWork.NotificationTypes.FindList(x => !x.IsDeleted);
                    NotificationDto notification = new NotificationDto()
                    {
                        NotificationText = "Order " + orderDB.Name + " was dropped By " + _utilitiesManager.ToTitleCase(sampler.Name) + "",
                        NotificationTypeId = notificationTypes.FirstOrDefault(x => x.Name.Equals(NotificationTypes.OrderDroppedOffByASampler)).Id,
                        Roles = new List<string>() { Roles.Admin, Roles.LabAssistant, Roles.LabTechnician },
                        DateTime = DateTime.UtcNow,
                        NotificationTypeName = NotificationTypes.OrderDroppedOffByASampler
                    };
                    _notificationManager.SetNotificationToUser(notification);
                }
                return true;
            }
            return false;
        }

        public void CompleteOrder(int orderId, bool isOrderEmpty)
        {
            var orderStatus = _unitOfWork.OrderStatus.FindList(x => !x.IsDeleted);
            if (!isOrderEmpty)
            {
                bool isntCompleted = _unitOfWork.Order.Any(x => x.Id == orderId &&
                (!x.LabTechFilledFormsCount.HasValue ||
                (x.LabTechFilledFormsCount.HasValue && x.LabTechFilledFormsCount.Value < x.EstimateNumber) ||
                 x.OrderSamples.Any(c => !c.IsDeleted && (!c.Progress.HasValue || (c.Progress.HasValue && c.Progress < 100)))
                ));
                if (!isntCompleted)
                {
                    var order = _unitOfWork.Order.FirstOrDefault(x => !x.IsDeleted && !x.IsCanceled && x.Id == orderId);
                    int statusId = orderStatus.FirstOrDefault(x => x.Name.Equals(OrderStatus.TestingCompleted)).Id;
                    _orderDetailsManager.CreateOrderDetails(order.Id, statusId, DateTime.UtcNow);
                    var notificationTypes = _unitOfWork.NotificationTypes.FindList(x => !x.IsDeleted);
                    NotificationDto notification = new NotificationDto()
                    {
                        NotificationText = "Testing is completed for order " + order.Name,
                        NotificationTypeId = notificationTypes.FirstOrDefault(x => x.Name.Equals(NotificationTypes.TestingIsCompletedForSamplesOfASpecificOrder)).Id,
                        Roles = new List<string>() { Roles.Admin, Roles.LabAssistant, Roles.QA },
                        DateTime = DateTime.UtcNow,
                        NotificationTypeName = NotificationTypes.TestingIsCompletedForSamplesOfASpecificOrder
                    };
                    _notificationManager.SetNotificationToUser(notification);
                }
            }
            else
            {
                var order = _unitOfWork.Order.FirstOrDefault(x => !x.IsDeleted && !x.IsCanceled && x.Id == orderId);
                int statusId = orderStatus.FirstOrDefault(x => x.Name.Equals(OrderStatus.TestingCompleted)).Id;
                _orderDetailsManager.CreateOrderDetails(order.Id, statusId, DateTime.UtcNow);
            }
        }

        public OrderDto ApproveOrReportOrReleaseOrder(int orderId, string status, string userId)
        {
            OrderDto model = new OrderDto();
            var orderStatus = _unitOfWork.OrderStatus.FindList(x => !x.IsDeleted);
            var orderDB = _unitOfWork.Order.FirstOrDefault(c => !c.IsDeleted && !c.IsCanceled && c.Id == orderId);
            if (orderDB != null)
            {
                int statusId = orderStatus.FirstOrDefault(x => x.Name.Equals(status)).Id;
                _orderDetailsManager.CreateOrderDetails(orderDB.Id, statusId, DateTime.UtcNow);
                if (status == OrderStatus.ResultsApproved)
                {
                    orderDB.QAId = userId;
                }
                _unitOfWork.Complete();
                var user = _unitOfWork.Accounts.FirstOrDefault(x => x.Id == userId);
                var notificationTypes = _unitOfWork.NotificationTypes.FindList(x => !x.IsDeleted);
                string text = status.Equals(OrderStatus.ResultsApproved) ? "Order " + orderDB.Name + " was approved by " :
                    status.Equals(OrderStatus.ResultsReported) ? "Order " + orderDB.Name + " was reported to the state " :
                    status.Equals(OrderStatus.ReleaseOfCOA) ? "COA was released for order " + orderDB.Name + " " : "";
                NotificationDto notification = new NotificationDto()
                {
                    NotificationText = text + "by " + _utilitiesManager.ToTitleCase(user.Name),
                    NotificationTypeId = notificationTypes.FirstOrDefault(x => x.Name.Equals(
                        status.Equals(OrderStatus.ResultsApproved) ? NotificationTypes.ApproveOrder :
                        status.Equals(OrderStatus.ResultsReported) ? NotificationTypes.ResultsReportedToState :
                        NotificationTypes.ReleaseOfCOA)).Id,
                    Roles = new List<string>() { Roles.Admin, Roles.LabAssistant },
                    DateTime = DateTime.UtcNow,
                    NotificationTypeName = status.Equals(OrderStatus.ResultsApproved) ? NotificationTypes.ApproveOrder :
                        status.Equals(OrderStatus.ResultsReported) ? NotificationTypes.ResultsReportedToState :
                        NotificationTypes.ReleaseOfCOA
                };
                _notificationManager.SetNotificationToUser(notification);
                model = Mapping(orderDB);
            }
            return model;
        }

        public OrderDto Mapping(TblOrders orderDB, string? role = null, string? userId = null)
        {
            OrderDto model = new OrderDto();
            model = _mapper.Map<OrderDto>(orderDB);
            model.Business = _businessManager.Mapping(orderDB.Business);
            model.StatusName = orderDB.OrderDetails.LastOrDefault().OrderStatus.Name;
            model.SamplerName = orderDB.Sampler != null ? orderDB.Sampler.Name : null;
            var currentStatus = orderDB.OrderDetails.LastOrDefault();
            model.StatusId = currentStatus.StatusId;
            model.DateTime = currentStatus.DateTime;
            if (orderDB.Business.BusinessContacts != null)
            {
                model.BusinessContacts = _mapper.Map<List<BusinessContactsDto>>(orderDB.Business.BusinessContacts);
            }
            if (!string.IsNullOrEmpty(orderDB.SamplerId))
            {
                var orderSamples = orderDB.OrderSamples?.Where(x => !x.IsDeleted);
                if (orderSamples != null && orderSamples.Count() > 0)
                {
                    model.OrderSamples = new List<OrderSamplesDto>();
                    foreach (var orderSample in orderSamples)
                    {
                        model.OrderSamples.Add(_commonManager.OrderSamplesMapping(orderSample));
                    }
                }
            }
            if (!string.IsNullOrEmpty(role))
            {
                int orderSamples = orderDB.OrderSamples.Count(x => !x.IsDeleted);
                if (role.Equals(Roles.Sampler) && !string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(orderDB.SamplerId) && userId.Equals(orderDB.SamplerId))
                {
                    model.CurrentSampleForm = orderDB.EstimateNumber == orderSamples ? 0 : orderSamples + 1;
                }
                else if (role.Equals(Roles.LabTechnician))
                {
                    model.CurrentSampleForm = orderDB.LabTechFilledFormsCount.HasValue ? (orderDB.EstimateNumber == orderDB.LabTechFilledFormsCount ? 0 : orderDB.LabTechFilledFormsCount + 1) : 1;
                }
            }
            return model;
        }
    }
}
