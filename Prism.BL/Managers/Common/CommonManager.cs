using AutoMapper;
using Microsoft.Extensions.Configuration;
using Prism.BL.Dtos;
using Prism.DAL;
using Prism.Repository;
using QRCodeResults.BL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.BL.Managers.Common
{
    public class CommonManager : ICommonManager
    {
        public readonly IMapper _mapper;
        public readonly IConfiguration _configuration;
        public readonly IUnitOfWork _unitOfWork;

        public CommonManager(IMapper mapper, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public List<AboutUsDto> GetAboutUsList()
        {
            List<AboutUsDto> lkpAboutUsLst = new List<AboutUsDto>();
            var lkpAboutUsLstDB = _unitOfWork.AboutUs.FindList(c => !c.IsDeleted);
            if (lkpAboutUsLstDB != null)
            {
                lkpAboutUsLst = _mapper.Map<List<AboutUsDto>>(lkpAboutUsLstDB);
            }
            return lkpAboutUsLst;
        }

        public List<TestTypesDto> GetTestTypesList()
        {
            List<TestTypesDto> lkpOrderTypesLst = new List<TestTypesDto>();
            var lkpOrderTypesLstDB = _unitOfWork.TestTypes.FindList(c => !c.IsDeleted);
            if (lkpOrderTypesLstDB != null)
            {
                lkpOrderTypesLst = _mapper.Map<List<TestTypesDto>>(lkpOrderTypesLstDB);
            }
            return lkpOrderTypesLst;
        }

        public List<TestSubTypesDto> GetTestSubTypesList()
        {
            List<TestSubTypesDto> lkpOrderSubTypesLst = new List<TestSubTypesDto>();
            var lkpOrderSubTypesLstDB = _unitOfWork.TestSubTypes.FindList(c => !c.IsDeleted);
            if (lkpOrderSubTypesLstDB != null)
            {
                lkpOrderSubTypesLst = _mapper.Map<List<TestSubTypesDto>>(lkpOrderSubTypesLstDB);
            }
            return lkpOrderSubTypesLst;
        }

        public List<SampleTypesDto> GetSampleTypesList()
        {
            List<SampleTypesDto> lkpSampleTypesLst = new List<SampleTypesDto>();
            var lkpSampleTypesLstDB = _unitOfWork.SampleTypes.FindList(c => !c.IsDeleted);
            if (lkpSampleTypesLstDB != null)
            {
                lkpSampleTypesLst = _mapper.Map<List<SampleTypesDto>>(lkpSampleTypesLstDB);
            }
            return lkpSampleTypesLst;
        }

        public List<SamplerDocumentTypesDto> GetSamplerDocumentTypesList()
        {
            List<SamplerDocumentTypesDto> lkpSamplerDocumentTypesLst = new List<SamplerDocumentTypesDto>();
            var lkpSamplerDocumentTypesDB = _unitOfWork.SamplerDocumentTypes.FindList(c => !c.IsDeleted);
            if (lkpSamplerDocumentTypesDB != null)
            {
                lkpSamplerDocumentTypesLst = _mapper.Map<List<SamplerDocumentTypesDto>>(lkpSamplerDocumentTypesDB);
            }
            return lkpSamplerDocumentTypesLst;
        }

        public List<OrderStatusDto> GetOrderStatusList()
        {
            List<OrderStatusDto> lkpOrderStatusLst = new List<OrderStatusDto>();
            var lkpOrderStatusLstDB = _unitOfWork.OrderStatus.FindList(c => !c.IsDeleted);
            if (lkpOrderStatusLstDB != null)
            {
                lkpOrderStatusLst = _mapper.Map<List<OrderStatusDto>>(lkpOrderStatusLstDB);
            }
            return lkpOrderStatusLst;
        }

        public List<SampleTestStatusDto> GetSampleTestStatusList()
        {
            List<SampleTestStatusDto> lkpSampleTestStatusLst = new List<SampleTestStatusDto>();
            var lkpSampleTestStatusLstDB = _unitOfWork.SampleTestStatus.FindList(c => !c.IsDeleted);
            if (lkpSampleTestStatusLstDB != null)
            {
                lkpSampleTestStatusLst = _mapper.Map<List<SampleTestStatusDto>>(lkpSampleTestStatusLstDB);
            }
            return lkpSampleTestStatusLst;
        }

        public OrderSamplesDto OrderSamplesMapping(TblOrderSamples orderSampleDB)
        {
            OrderSamplesDto model = new OrderSamplesDto();
            model = _mapper.Map<OrderSamplesDto>(orderSampleDB);
            model.SampleOrder = _mapper.Map<OrderDto>(orderSampleDB.Order);
            var currentStatus = orderSampleDB.Order.OrderDetails.LastOrDefault();
            model.SampleOrder.StatusId = currentStatus.StatusId;
            model.SampleOrder.DateTime = currentStatus.DateTime;
            model.SampleOrder.StatusName = currentStatus.OrderStatus.Name;
            model.SampleTest = _mapper.Map<TestTypesDto>(orderSampleDB.TestType);
            model.TestStatus = _mapper.Map<SampleTestStatusDto>(orderSampleDB.SampleTestStatus);
            var GroupAComTests = orderSampleDB.OrderSampleTests.Where(x => !x.IsDeleted && x.Test != null && x.Test.Type.Equals("Group A") && x.SampleTestStatus != null && x.SampleTestStatus.Name.Equals(SampleTestStatus.Completed));
            var GroupBComTests = orderSampleDB.OrderSampleTests.Where(x => !x.IsDeleted && x.Test != null && x.Test.Type.Equals("Group B") && x.SampleTestStatus != null && x.SampleTestStatus.Name.Equals(SampleTestStatus.Completed));
            if (GroupAComTests != null)
            {
                model.GroupA = "";
                int index = 1;
                foreach (var test in GroupAComTests)
                {
                    model.GroupA = model.GroupA + test.Test.Name +
                        (index == GroupAComTests.Count() ? "" : " , ");
                    index++;
                }
            }
            if (GroupBComTests != null)
            {
                model.GroupB = "";
                int index = 1;
                foreach (var test in GroupBComTests)
                {
                    model.GroupB = model.GroupB + test.Test.Name +
                        (index == GroupBComTests.Count() ? "" : " , ");
                    index++;
                }
            }
            return model;
        }
    }
}
