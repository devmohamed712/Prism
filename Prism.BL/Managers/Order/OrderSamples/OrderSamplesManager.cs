using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Prism.BL.Configrations;
using Prism.BL.Dtos;
using Prism.BL.Managers.Business;
using Prism.BL.Managers.Common;
using Prism.BL.Managers.Notification;
using Prism.BL.Managers.Order.OrderDetails;
using Prism.BL.Managers.Utilities;
using Prism.DAL;
using Prism.Repository;
using QRCodeResults.BL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Prism.BL.Managers.Order.OrderSamples
{
    public class OrderSamplesManager : IOrderSamplesManager
    {
        public readonly IMapper _mapper;
        public readonly IConfiguration _configuration;
        public readonly IUnitOfWork _unitOfWork;
        public readonly IOrderManager _orderManager;
        public readonly IOrderDetailsManager _orderDetailsManager;
        public readonly IUtilitiesManager _utilitiesManager;
        public readonly INotificationManager _notificationManager;
        public readonly ICommonManager _commonManager;

        public OrderSamplesManager(IMapper mapper, IConfiguration configuration, IUnitOfWork unitOfWork, IOrderManager orderManager, IOrderDetailsManager orderDetailsManager, IUtilitiesManager utilitiesManager, INotificationManager notificationManager, ICommonManager commonManager)
        {
            _mapper = mapper;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _orderManager = orderManager;
            _orderDetailsManager = orderDetailsManager;
            _utilitiesManager = utilitiesManager;
            _notificationManager = notificationManager;
            _commonManager = commonManager;
        }

        public OrderSamplesDtoList GetOrderSamples(int pageNumber, int pageSize, int orderId)
        {
            OrderSamplesDtoList modelList = new OrderSamplesDtoList();
            modelList.OrderSamples = new List<OrderSamplesDto>();
            Func<TblOrderSamples, bool> predicates = x => !x.IsDeleted && x.OrderId == orderId;
            IEnumerable<TblOrderSamples> OrderSamplesDB = _unitOfWork.OrderSamples.FindByPage(predicates, pageNumber, pageSize);
            foreach (var orderSampleDB in OrderSamplesDB)
            {
                modelList.OrderSamples.Add(_commonManager.OrderSamplesMapping(orderSampleDB));
            }
            modelList.PageNumber = pageNumber;
            modelList.PageSize = pageSize;
            modelList.IsLastPage = OrderSamplesDB != null && OrderSamplesDB.Count() < pageSize ? true : false;
            return modelList;
        }

        public List<OrderSamplesDto> GetOrderSamples(int orderId = 0, string? samplerId = null)
        {
            List<OrderSamplesDto> orderSamples = new List<OrderSamplesDto>();
            Func<TblOrderSamples, bool> predicates = x => !x.IsDeleted && !x.Order.IsDeleted && !x.Order.IsCanceled;
            if (orderId > 0)
            {
                predicates = predicates.AndAlso(x => x.OrderId == orderId);
            }
            if (!string.IsNullOrEmpty(samplerId))
            {
                predicates = predicates.AndAlso(x => x.Order.SamplerId.Equals(samplerId) && x.Order.OrderDetails.LastOrDefault().OrderStatus.Name.Equals(OrderStatus.PickedUp));
            }
            var orderSamplesDB = _unitOfWork.OrderSamples.FindList(predicates);
            if (orderSamplesDB != null)
            {
                orderSamples = _mapper.Map<List<OrderSamplesDto>>(orderSamplesDB);
            }
            return orderSamples;
        }

        public OrderSamplesDto CreateOrEditOrderSample(OrderSamplesDto model, IFormFileCollection files, string orderSampleFilesPath)
        {
            TblOrderSamples? orderSampleDB = null;
            if (model.Id > 0)
            {
                orderSampleDB = _unitOfWork.OrderSamples.FirstOrDefault(c => !c.IsDeleted && c.Id == model.Id);
                if (orderSampleDB != null)
                {
                    _mapper.Map(model, orderSampleDB);
                    _unitOfWork.Complete();
                    orderSampleDB.Order.LabTechFilledFormsCount = orderSampleDB.Order.LabTechFilledFormsCount.HasValue ? orderSampleDB.Order.LabTechFilledFormsCount.Value + 1 : 1;
                    _unitOfWork.Complete();
                    _orderManager.CompleteOrder(orderSampleDB.OrderId, false);
                }
            }
            else
            {
                orderSampleDB = _mapper.Map<TblOrderSamples>(model);
                _unitOfWork.OrderSamples.Add(orderSampleDB);
                _unitOfWork.Complete();
            }
            model.Id = orderSampleDB.Id;
            if (model.Id > 0 && files?.Count() > 0)
            {
                foreach (var file in files)
                {
                    var extension = Path.GetExtension(file.FileName).Remove(0, 1);
                    string fileName = _utilitiesManager.UploadFile(file, extension, orderSampleFilesPath, model.Id.ToString());
                    PropertyInfo prop = _utilitiesManager.GetProperty(model, file.Name);
                    if (null != prop && prop.CanWrite)
                    {
                        prop.SetValue(model, fileName, null);
                    }
                }
                orderSampleDB = _unitOfWork.OrderSamples.FirstOrDefault(c => !c.IsDeleted && c.Id == model.Id);
                if (orderSampleDB != null)
                {
                    _mapper.Map(model, orderSampleDB);
                    _unitOfWork.Complete();
                }
            }
            return model;
        }

        public OrderSamplesDtoList SamplesSearch(int pageNumber, int pageSize, string orderName, int testId, int sampleId, bool pendingSplit, bool pendingForeignMatterTesting, bool pendingWaterActivity, bool totalYeastAndMoldCount, bool totalColiform, bool eColi, bool salmonella, bool aspergillus, bool pendingPesticidesTesting, bool pendingMetalTesting, bool pendingPotencyTesting, bool pendingTerpensTesting)
        {
            OrderSamplesDtoList modelList = new OrderSamplesDtoList();
            modelList.OrderSamples = new List<OrderSamplesDto>();
            Func<TblOrderSamples, bool> predicates = x => !x.IsDeleted && !x.Order.IsDeleted && !x.Order.IsCanceled;
            if (!string.IsNullOrEmpty(orderName))
            {
                predicates = predicates.AndAlso(x => x.Order.Name.Equals(orderName));
            }
            if (testId > 0)
            {
                predicates = predicates.AndAlso(x => x.TestTypeId == testId);
            }
            if (sampleId > 0)
            {
                predicates = predicates.AndAlso(x => x.Id == sampleId);
            }
            if (pendingSplit)
            {
                predicates = predicates.AndAlso(x => !x.IsSplit);
            }
            if (pendingForeignMatterTesting || pendingWaterActivity || totalYeastAndMoldCount || totalColiform || eColi || salmonella || aspergillus || pendingPesticidesTesting || pendingMetalTesting || pendingPotencyTesting || pendingTerpensTesting)
            {
                predicates = predicates.AndAlso(x => x.IsSplit);
                if (pendingForeignMatterTesting)
                {
                    predicates = predicates.AndAlso(x =>
                    !x.OrderSampleTests.Any(c => c.Test.Name.Equals(SampleGroupTests.ForeignMatter) && c.SampleTestStatus.Name.Equals(SampleTestStatus.Completed)));
                }
                if (pendingWaterActivity)
                {
                    predicates = predicates.AndAlso(x =>
                    !x.OrderSampleTests.Any(c => c.Test.Name.Equals(SampleGroupTests.WaterActivity) && c.SampleTestStatus.Name.Equals(SampleTestStatus.Completed)) &&
                    x.OrderSampleTests.Any(c => c.Test.Name.Equals(SampleGroupTests.ForeignMatter) && c.SampleTestStatus.Name.Equals(SampleTestStatus.Completed)));
                }
                if (totalYeastAndMoldCount)
                {
                    predicates = predicates.AndAlso(x =>
                    !x.OrderSampleTests.Any(c => c.Test.Name.Equals(SampleGroupTests.TotalYeastMoldCount) && c.SampleTestStatus.Name.Equals(SampleTestStatus.Completed)) &&
                    x.OrderSampleTests.Any(c => c.Test.Name.Equals(SampleGroupTests.WaterActivity) && c.SampleTestStatus.Name.Equals(SampleTestStatus.Completed)) &&
                    x.OrderSampleTests.Any(c => c.Test.Name.Equals(SampleGroupTests.ForeignMatter) && c.SampleTestStatus.Name.Equals(SampleTestStatus.Completed)));
                }
                if (totalColiform)
                {
                    predicates = predicates.AndAlso(x =>
                 !x.OrderSampleTests.Any(c => c.Test.Name.Equals(SampleGroupTests.TotalColiform) && c.SampleTestStatus.Name.Equals(SampleTestStatus.Completed)) &&
                 x.OrderSampleTests.Any(c => c.Test.Name.Equals(SampleGroupTests.WaterActivity) && c.SampleTestStatus.Name.Equals(SampleTestStatus.Completed)) &&
                 x.OrderSampleTests.Any(c => c.Test.Name.Equals(SampleGroupTests.ForeignMatter) && c.SampleTestStatus.Name.Equals(SampleTestStatus.Completed)));
                }
                if (eColi)
                {
                    predicates = predicates.AndAlso(x =>
                 !x.OrderSampleTests.Any(c => c.Test.Name.Equals(SampleGroupTests.EColi) && c.SampleTestStatus.Name.Equals(SampleTestStatus.Completed)) &&
                 x.OrderSampleTests.Any(c => c.Test.Name.Equals(SampleGroupTests.WaterActivity) && c.SampleTestStatus.Name.Equals(SampleTestStatus.Completed)) &&
                 x.OrderSampleTests.Any(c => c.Test.Name.Equals(SampleGroupTests.ForeignMatter) && c.SampleTestStatus.Name.Equals(SampleTestStatus.Completed)));
                }
                if (salmonella)
                {
                    predicates = predicates.AndAlso(x =>
                 !x.OrderSampleTests.Any(c => c.Test.Name.Equals(SampleGroupTests.Salmonella) && c.SampleTestStatus.Name.Equals(SampleTestStatus.Completed)) &&
                 x.OrderSampleTests.Any(c => c.Test.Name.Equals(SampleGroupTests.WaterActivity) && c.SampleTestStatus.Name.Equals(SampleTestStatus.Completed)) &&
                 x.OrderSampleTests.Any(c => c.Test.Name.Equals(SampleGroupTests.ForeignMatter) && c.SampleTestStatus.Name.Equals(SampleTestStatus.Completed)));
                }
                if (aspergillus)
                {
                    predicates = predicates.AndAlso(x =>
                 !x.OrderSampleTests.Any(c => c.Test.Name.Equals(SampleGroupTests.Aspergillus) && c.SampleTestStatus.Name.Equals(SampleTestStatus.Completed)) &&
                 x.OrderSampleTests.Any(c => c.Test.Name.Equals(SampleGroupTests.WaterActivity) && c.SampleTestStatus.Name.Equals(SampleTestStatus.Completed)) &&
                 x.OrderSampleTests.Any(c => c.Test.Name.Equals(SampleGroupTests.ForeignMatter) && c.SampleTestStatus.Name.Equals(SampleTestStatus.Completed)));
                }
                if (pendingPesticidesTesting)
                {
                    predicates = predicates.AndAlso(x =>
                    !x.OrderSampleTests.Any(c => c.Test.Name.Equals(SampleGroupTests.PestisideTesting) && c.SampleTestStatus.Name.Equals(SampleTestStatus.Completed)) &&
                    x.OrderSampleTests.Any(c => c.Test.Name.Equals(SampleGroupTests.WaterActivity) && c.SampleTestStatus.Name.Equals(SampleTestStatus.Completed)) &&
                    x.OrderSampleTests.Any(c => c.Test.Name.Equals(SampleGroupTests.ForeignMatter) && c.SampleTestStatus.Name.Equals(SampleTestStatus.Completed)));
                }
                if (pendingMetalTesting)
                {
                    predicates = predicates.AndAlso(x =>
                    !x.OrderSampleTests.Any(c => c.Test.Name.Equals(SampleGroupTests.MetalTesting) && c.SampleTestStatus.Name.Equals(SampleTestStatus.Completed)) &&
                    x.OrderSampleTests.Any(c => c.Test.Name.Equals(SampleGroupTests.WaterActivity) && c.SampleTestStatus.Name.Equals(SampleTestStatus.Completed)) &&
                    x.OrderSampleTests.Any(c => c.Test.Name.Equals(SampleGroupTests.ForeignMatter) && c.SampleTestStatus.Name.Equals(SampleTestStatus.Completed)));
                }
                if (pendingPotencyTesting)
                {
                    predicates = predicates.AndAlso(x =>
                    !x.OrderSampleTests.Any(c => c.Test.Name.Equals(SampleGroupTests.PotencyTesting) && c.SampleTestStatus.Name.Equals(SampleTestStatus.Completed)) &&
                    x.OrderSampleTests.Any(c => c.Test.Name.Equals(SampleGroupTests.WaterActivity) && c.SampleTestStatus.Name.Equals(SampleTestStatus.Completed)) &&
                    x.OrderSampleTests.Any(c => c.Test.Name.Equals(SampleGroupTests.ForeignMatter) && c.SampleTestStatus.Name.Equals(SampleTestStatus.Completed)));
                }
                if (pendingTerpensTesting)
                {
                    predicates = predicates.AndAlso(x =>
                    !x.OrderSampleTests.Any(c => c.Test.Name.Equals(SampleGroupTests.TerpensTesting) && c.SampleTestStatus.Name.Equals(SampleTestStatus.Completed)) &&
                    x.OrderSampleTests.Any(c => c.Test.Name.Equals(SampleGroupTests.WaterActivity) && c.SampleTestStatus.Name.Equals(SampleTestStatus.Completed)) &&
                    x.OrderSampleTests.Any(c => c.Test.Name.Equals(SampleGroupTests.ForeignMatter) && c.SampleTestStatus.Name.Equals(SampleTestStatus.Completed)));
                }
            }
            IEnumerable<TblOrderSamples> samplesDB = _unitOfWork.OrderSamples.FindByPage(predicates, pageNumber, pageSize);
            foreach (var sampleDB in samplesDB)
            {
                modelList.OrderSamples.Add(_commonManager.OrderSamplesMapping(sampleDB));
            }
            modelList.PageNumber = pageNumber;
            modelList.IsLastPage = samplesDB != null && samplesDB.Count() < pageSize ? true : false;
            return modelList;
        }

        public OrderDto AcceptOrderSamples(int orderId)
        {
            OrderDto model = new OrderDto();
            var orderStatus = _unitOfWork.OrderStatus.FindList(x => !x.IsDeleted);
            var order = _unitOfWork.Order.FirstOrDefault(x => !x.IsDeleted && x.Id == orderId);
            if (order != null)
            {
                int statusId = orderStatus.FirstOrDefault(x => x.Name.Equals(OrderStatus.SampleAccepted)).Id;
                _orderDetailsManager.CreateOrderDetails(order.Id, statusId, DateTime.UtcNow);
                model = _orderManager.Mapping(order);
            }
            return model;
        }

        public OrderSamplesDto SplitSample(int id, bool isSplit, string role, string userId)
        {
            OrderSamplesDto model = new OrderSamplesDto();
            var orderStatus = _unitOfWork.OrderStatus.FindList(x => !x.IsDeleted);
            var sampleStatus = _unitOfWork.SampleTestStatus.FindList(x => !x.IsDeleted);
            var sample = _unitOfWork.OrderSamples.FirstOrDefault(x => !x.IsDeleted && x.Id == id);
            var user = _unitOfWork.Accounts.FirstOrDefault(x => x.Id == userId);
            if (sample != null)
            {
                sample.IsSplit = isSplit;
                if (isSplit)
                {
                    if (role.Equals(Roles.LabTechnician))
                    {
                        sample.LabTechId = userId;
                    }
                    sample.StatusId = sampleStatus.FirstOrDefault(x => x.Name.Equals(SampleTestStatus.Started)).Id;
                    sample.Progress = Convert.ToDecimal(100) / Convert.ToDecimal(12);
                    if (!sample.Order.OrderDetails.Any(x => x.OrderStatus.Name.Equals(OrderStatus.TestingStarted)))
                    {
                        int statusId = orderStatus.FirstOrDefault(x => x.Name.Equals(OrderStatus.TestingStarted)).Id;
                        _orderDetailsManager.CreateOrderDetails(sample.OrderId, statusId, DateTime.UtcNow);
                    }
                    var notificationTypes = _unitOfWork.NotificationTypes.FindList(x => !x.IsDeleted);
                    NotificationDto notification = new NotificationDto()
                    {
                        NotificationText = "Sample of order " + sample.Order.Name + " was split into two groups by " + _utilitiesManager.ToTitleCase(user.Name) + "",
                        NotificationTypeId = notificationTypes.FirstOrDefault(x => x.Name.Equals(NotificationTypes.TestingSplitIntoTwoGroups)).Id,
                        Roles = new List<string>() { Roles.Admin, Roles.LabAssistant },
                        DateTime = DateTime.UtcNow,
                        NotificationTypeName = NotificationTypes.TestingSplitIntoTwoGroups
                    };
                    _notificationManager.SetNotificationToUser(notification);
                }
                else
                {
                    sample.StatusId = null;
                    sample.Progress = null;
                }
                _unitOfWork.Complete();
                model = _commonManager.OrderSamplesMapping(sample);
            }
            return model;
        }

        public List<SamplerDocumentsDto> GetSamplerDocuments(string userId)
        {
            List<SamplerDocumentsDto> lst = new List<SamplerDocumentsDto>();
            var samplerDocumentsDB = _unitOfWork.SamplerDocuments.FindList(x => !x.IsDeleted && x.AccountId.Equals(userId));
            foreach (var samplerDocumentDB in samplerDocumentsDB)
            {
                lst.Add(_mapper.Map<SamplerDocumentsDto>(samplerDocumentDB));
            }
            return lst;
        }

        public SamplerDocumentsDto CreateOrEditSamplerDocument(SamplerDocumentsDto model)
        {
            TblSamplerDocuments? sampleDocumentDB = null;
            if (model.Id > 0)
            {
                sampleDocumentDB = _unitOfWork.SamplerDocuments.FirstOrDefault(c => !c.IsDeleted && c.Id == model.Id);
                if (sampleDocumentDB != null)
                {
                    _mapper.Map(model, sampleDocumentDB);
                }
            }
            else
            {
                sampleDocumentDB = _mapper.Map<TblSamplerDocuments>(model);
                _unitOfWork.SamplerDocuments.Add(sampleDocumentDB);
            }
            _unitOfWork.Complete();
            model.Id = sampleDocumentDB.Id;
            return model;
        }

        public List<SamplerDocumentsDto> CreateOrEditSamplerDocFiles(IFormFileCollection files, AccountDto model, string samplerDocsPath)
        {
            List<SamplerDocumentTypesDto> lst = _commonManager.GetSamplerDocumentTypesList();
            foreach (var file in files)
            {
                var extension = Path.GetExtension(file.FileName).Remove(0, 1);
                string samplerDocType = file.Name.Substring(0, file.Name.IndexOf("_"));
                string expireDate = file.Name.Substring(file.Name.IndexOf("_") + 1);
                string fileName = _utilitiesManager.UploadFile(file, extension, samplerDocsPath);
                SamplerDocumentsDto? docModel = null;
                if (!string.IsNullOrEmpty(fileName))
                {
                    if (!string.IsNullOrEmpty(model.Id) && model.SamplerDocuments.Any(c => c.SamplerDocumentType == samplerDocType))
                    {
                        var doc = model.SamplerDocuments.FirstOrDefault(x => x.SamplerDocumentType == samplerDocType);
                        doc.ExpireDate = doc.ExpireDate;
                        doc.FileName = fileName;
                        doc.FileRealName = file.FileName;
                    }
                    else
                    {
                        docModel = new SamplerDocumentsDto
                        {
                            AccountId = model.Id,
                            SamplerDocumentTypeId = lst.FirstOrDefault(x => x.Name.Equals(samplerDocType)).Id,
                            ExpireDate = DateTime.Parse(expireDate),
                            FileName = fileName,
                            FileRealName = file.FileName
                        };
                        model.SamplerDocuments.Add(docModel);
                    }
                }
            }
            return model.SamplerDocuments;
        }

        public int GetSamplesWithStatus(int type, int durationType = 0)
        {
            Func<TblOrderSamples, bool> predicates = x => !x.IsDeleted && !x.Order.IsDeleted && !x.Order.IsCanceled;
            if (type == (int)StatisticsTypes.SamplesCollected || type == (int)StatisticsTypes.SamplesHasBeenTested)
            {
                predicates = predicates.AndAlso(x => x.Order.OrderDetails.OrderByDescending(o => o.Id).FirstOrDefault().DateTime >= DateTime.UtcNow.AddDays(durationType == 1 ? -7 : -30));
                if (type == (int)StatisticsTypes.SamplesCollected)
                {
                    predicates = predicates.AndAlso(x => x.Order.OrderDetails.OrderByDescending(o => o.Id).FirstOrDefault().OrderStatus.Name.Equals(OrderStatus.PickedUp));
                }
                else
                {
                    predicates = predicates.AndAlso(x => x.Order.OrderDetails.OrderByDescending(o => o.Id).FirstOrDefault().OrderStatus.Name.Equals(OrderStatus.TestingStarted) && x.Progress == 100);
                }
            }
            else if (type == (int)StatisticsTypes.SamplesInProcess)
            {
                predicates = predicates.AndAlso(x => x.Order.OrderDetails.OrderByDescending(o => o.Id).FirstOrDefault().OrderStatus.Name.Equals(OrderStatus.TestingStarted));
            }
            else if (type == (int)StatisticsTypes.SamplesHasBeenReceived)
            {
                predicates = predicates.AndAlso(x => x.Order.OrderDetails.OrderByDescending(o => o.Id).FirstOrDefault().OrderStatus.Name.Equals(OrderStatus.SampleAccepted));
            }
            else if (type == (int)StatisticsTypes.SamplesHasBeenFinished)
            {
                predicates = predicates.AndAlso(x => x.Order.OrderDetails.OrderByDescending(o => o.Id).FirstOrDefault().OrderStatus.Name.Equals(OrderStatus.ReleaseOfCOA));
            }
            int orderSamplesDB = _unitOfWork.OrderSamples.Count(predicates);
            return orderSamplesDB;
        }
    }
}
