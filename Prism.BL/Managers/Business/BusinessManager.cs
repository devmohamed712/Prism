using AutoMapper;
using Microsoft.Extensions.Configuration;
using Prism.BL.Dtos;
using Prism.BL.Managers.Business.BasinessSamples;
using Prism.BL.Managers.Business.BusinessAddresses;
using Prism.BL.Managers.Business.BusinessContacts;
using Prism.BL.Managers.Business.BusinessTests;
using Prism.DAL;
using Prism.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.BL.Managers.Business
{
    public class BusinessManager : IBusinessManager
    {
        public readonly IMapper _mapper;
        public readonly IConfiguration _configuration;
        public readonly IUnitOfWork _unitOfWork;
        public readonly IBusinessContactsManager _businessContactsManager;
        public readonly IBusinessAddressesManager _businessAddressesManager;
        public readonly IBusinessTestsManager _businessTestsManager;
        public readonly IBusinessSamplesManager _businessSamplesManager;

        public BusinessManager(IMapper mapper, IConfiguration configuration, IUnitOfWork unitOfWork, IBusinessContactsManager businessContactsManager, IBusinessAddressesManager businessAddressesManager, IBusinessTestsManager businessTestsManager, IBusinessSamplesManager businessSamplesManager)
        {
            _mapper = mapper;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _businessContactsManager = businessContactsManager;
            _businessAddressesManager = businessAddressesManager;
            _businessTestsManager = businessTestsManager;
            _businessSamplesManager = businessSamplesManager;
        }

        public BusinessDtoList GetBusinesses(int pageNumber, int pageSize)
        {
            BusinessDtoList modelList = new BusinessDtoList();
            modelList.Businesses = new List<BusinessDto>();
            IEnumerable<TblBusinesses> businessesDB = _unitOfWork.Business.FindByPage(x => true, pageNumber, pageSize);
            foreach (var businessDB in businessesDB)
            {
                modelList.Businesses.Add(Mapping(businessDB));
            }
            modelList.PageNumber = pageNumber;
            modelList.PageSize = pageSize;
            modelList.IsLastPage = businessesDB != null && businessesDB.Count() < pageSize ? true : false;
            return modelList;
        }

        public List<BusinessDto> GetBusinesses()
        {
            List<BusinessDto> Businesses = new List<BusinessDto>();
            IEnumerable<TblBusinesses> businessesDB = _unitOfWork.Business.FindList(x => !x.IsDeleted);
            if (businessesDB != null)
            {
                Businesses = _mapper.Map<List<BusinessDto>>(businessesDB);
            }
            return Businesses;
        }

        public BusinessDto? GetBusiness(int id)
        {
            BusinessDto? model = null;
            var businessDB = _unitOfWork.Business.FirstOrDefault(c => !c.IsDeleted && c.Id == id);
            if (businessDB != null)
            {
                model = Mapping(businessDB);
            }
            return model;
        }

        public BusinessDto CreateOrEditBusiness(BusinessDto model)
        {
            TblBusinesses? businessDB = null;
            if (model.Id > 0)
            {
                businessDB = _unitOfWork.Business.FirstOrDefault(c => !c.IsDeleted && c.Id == model.Id);
                if (businessDB != null)
                {
                    _mapper.Map(model, businessDB);
                }
            }
            else
            {
                businessDB = _mapper.Map<TblBusinesses>(model);
                _unitOfWork.Business.Add(businessDB);
                model.Id = businessDB.Id;
            }
            _unitOfWork.Complete();

            if (model.Contacts != null)
            {
                model.Contacts.ForEach(contactModel =>
                {
                    contactModel.BusinessId = model.Id;
                    _businessContactsManager.CreateOrEditBusinessContact(contactModel);
                });
            }
            if (model.Addresses != null)
            {
                List<LkpAreas> areas = _unitOfWork.Areas.FindList(x => !x.IsDeleted).ToList();
                model.Addresses.ForEach(addressModel =>
                {
                    addressModel.BusinessId = model.Id;
                    _businessAddressesManager.CreateOrEditBusinessAddress(addressModel, areas);
                });
            }
            if (model.Tests != null)
            {
                model.Tests.ForEach(testModel =>
                {
                    testModel.BusinessId = model.Id;
                    _businessTestsManager.CreateOrEditBusinessTest(testModel);
                });
            }
            if (model.Samples != null)
            {
                model.Samples.ForEach(sampleModel =>
                {
                    sampleModel.BusinessId = model.Id;
                    _businessSamplesManager.CreateOrEditBusinessSample(sampleModel);
                });
            }
            return model;
        }

        public bool DeleteOrRestoreBusiness(int id, bool isDelete)
        {
            var businessDB = _unitOfWork.Business.FirstOrDefault(c => c.Id == id);
            int result = 0;
            if (businessDB != null)
            {
                businessDB.IsDeleted = isDelete;
                result = _unitOfWork.Complete();
            }
            return result > 0 ? true : false;
        }

        public bool ChangeMRlicenseStatus(int id, bool status)
        {
            var businessDB = _unitOfWork.Business.FirstOrDefault(c => !c.IsDeleted && c.Id == id);
            if (businessDB != null)
            {
                _unitOfWork.Complete();
                return true;
            }
            return false;
        }

        public BusinessDto Mapping(TblBusinesses businessDB)
        {
            BusinessDto model = new BusinessDto();
            IEnumerable<TblBusinessContacts> businessContacts = businessDB.BusinessContacts.Where(c => !c.IsDeleted);
            IEnumerable<TblBusinessAddresses> businessAddresses = businessDB.BusinessAddresses.Where(c => !c.IsDeleted);
            model = _mapper.Map<BusinessDto>(businessDB);
            model.OrdersCount = businessDB.Orders != null ? businessDB.Orders.Count(c => !c.IsDeleted) : 0;
            model.Contacts = _mapper.Map<List<BusinessContactsDto>>(businessContacts);
            model.Addresses = _mapper.Map<List<BusinessAddressesDto>>(businessAddresses);
            model.Tests = new List<BusinessTestsDto>();
            model.Samples = new List<BusinessSamplesDto>();
            var businessTests = businessDB.BusinessTests.Where(c => !c.IsDeleted);
            if (businessTests != null)
            {
                foreach (var businessTest in businessTests)
                {
                    model.Tests.Add(_businessTestsManager.Mapping(businessTest));
                }
            }
            var orderSamples = businessDB.BusinessSamples.Where(c => !c.IsDeleted);
            if (orderSamples != null)
            {
                foreach (var orderSample in orderSamples)
                {
                    model.Samples.Add(_businessSamplesManager.Mapping(orderSample));
                }
            }
            return model;
        }
    }
}
