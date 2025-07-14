using AutoMapper;
using Microsoft.Extensions.Configuration;
using Prism.BL.Dtos;
using Prism.BL.Managers.Notification;
using Prism.BL.Managers.Order.OrderDetails;
using Prism.BL.Managers.Utilities;
using Prism.DAL;
using Prism.Repository;
using QRCodeResults.BL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.BL.Managers.Order.OrderSamplesTests
{
    public class OrderSamplesTestsManager : IOrderSamplesTestsManager
    {
        public readonly IMapper _mapper;
        public readonly IConfiguration _configuration;
        public readonly IUnitOfWork _unitOfWork;
        public readonly IOrderManager _orderManager;
        public readonly IOrderDetailsManager _orderDetailsManager;
        public readonly IUtilitiesManager _utilitiesManager;
        public readonly INotificationManager _notificationManager;

        public OrderSamplesTestsManager(IMapper mapper, IConfiguration configuration, IUnitOfWork unitOfWork, IOrderManager orderManager, IOrderDetailsManager orderDetailsManager, IUtilitiesManager utilitiesManager, INotificationManager notificationManager)
        {
            _mapper = mapper;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _orderManager = orderManager;
            _orderDetailsManager = orderDetailsManager;
            _utilitiesManager = utilitiesManager;
            _notificationManager = notificationManager;
        }

        public List<GroupTestsDto> GetGroupTestsList(int sampleId = 0)
        {
            List<GroupTestsDto> lkpGroupTestsLst = new List<GroupTestsDto>();
            var lkpGroupTestsLstDB = _unitOfWork.GroupTests.FindList(c => !c.IsDeleted);
            if (lkpGroupTestsLstDB != null)
            {
                foreach (var test in lkpGroupTestsLstDB)
                {
                    GroupTestsDto model = new GroupTestsDto();
                    model = _mapper.Map<GroupTestsDto>(test);
                    if (sampleId > 0)
                    {
                        model.Status = _unitOfWork.OrderSampleTests.FindList(x => !x.IsDeleted && x.OrderSampleId == sampleId && x.TestId == test.Id)?.LastOrDefault()?.SampleTestStatus?.Name;
                    }
                    lkpGroupTestsLst.Add(model);
                }
            }
            return lkpGroupTestsLst;
        }

        public OrderSampleTestsDto AddTest(OrderSampleTestsDto model)
        {
            model.DateTime = DateTime.UtcNow;
            TblOrderSampleTests OrderSampleTestsDB = _mapper.Map<TblOrderSampleTests>(model);
            _unitOfWork.OrderSampleTests.Add(OrderSampleTestsDB);
            _unitOfWork.Complete();
            var status = _unitOfWork.SampleTestStatus.FirstOrDefault(x => x.Id == model.StatusId);
            TblOrderSamples? sample = null;
            var sampleTest = _unitOfWork.OrderSampleTests.FirstOrDefault(x => x.Id == OrderSampleTestsDB.Id);
            var notificationTypes = _unitOfWork.NotificationTypes.FindList(x => !x.IsDeleted);
            NotificationDto notification = new NotificationDto()
            {
                NotificationText = "Test " + sampleTest.Test.Name + " was " + status.Name + " for sample " + sampleTest.OrderSample.Id + " of order " + sampleTest.OrderSample.Order.Name,
                NotificationTypeId = notificationTypes.FirstOrDefault(x => x.Name.Equals(status.Name.Equals(SampleTestStatus.Started) ? NotificationTypes.ASpecificTestIsStarted :
                    status.Name.Equals(SampleTestStatus.Failed) ? NotificationTypes.ASpecificTestIsFailed :
                    NotificationTypes.ASpecificTestIsCompleted)).Id,
                Roles = new List<string>() { Roles.Admin, Roles.LabAssistant },
                DateTime = DateTime.UtcNow,
                NotificationTypeName = status.Name.Equals(SampleTestStatus.Started) ? NotificationTypes.ASpecificTestIsStarted :
                    status.Name.Equals(SampleTestStatus.Failed) ? NotificationTypes.ASpecificTestIsFailed :
                    NotificationTypes.ASpecificTestIsCompleted
            };
            _notificationManager.SetNotificationToUser(notification);

            if (status.Name.Equals(SampleTestStatus.Completed))
            {
                sample = _unitOfWork.OrderSamples.FirstOrDefault(x => !x.IsDeleted && x.Id == OrderSampleTestsDB.OrderSampleId);
                decimal percentage = Convert.ToDecimal(100) / Convert.ToDecimal(12);
                sample.Progress = sample.Progress + percentage;
                if (sample.Progress >= 99)
                {
                    sample.Progress = 100;
                    sample.StatusId = model.StatusId;
                }
                _unitOfWork.Complete();
                _orderManager.CompleteOrder(sample.OrderId, false);
            }
            model.Id = OrderSampleTestsDB.Id;
            model.TestStatus = _mapper.Map<SampleTestStatusDto>(status);
            return model;

        }
    }
}
