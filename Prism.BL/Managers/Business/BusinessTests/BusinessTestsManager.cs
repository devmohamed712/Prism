using AutoMapper;
using Microsoft.Extensions.Configuration;
using Prism.BL.Dtos;
using Prism.DAL;
using Prism.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.BL.Managers.Business.BusinessTests
{
    public class BusinessTestsManager : IBusinessTestsManager
    {
        public readonly IMapper _mapper;
        public readonly IConfiguration _configuration;
        public readonly IUnitOfWork _unitOfWork;

        public BusinessTestsManager(IMapper mapper, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public List<BusinessTestsDto> GetBusinessTests(int businessId)
        {
            List<BusinessTestsDto> businessTests = new List<BusinessTestsDto>();
            IEnumerable<TblBusinessTests> businessTestsDB = _unitOfWork.BusinessTests.FindList(x => !x.IsDeleted && x.BusinessId == businessId);
            if (businessTestsDB != null)
            {
                businessTests = _mapper.Map<List<BusinessTestsDto>>(businessTestsDB);
            }
            return businessTests;
        }

        public BusinessTestsDto? GetBusinessTest(int id)
        {
            BusinessTestsDto? model = null;
            var businessTestsDB = _unitOfWork.BusinessTests.FirstOrDefault(c => !c.IsDeleted && c.Id == id);
            if (businessTestsDB != null)
            {
                model = _mapper.Map<BusinessTestsDto>(businessTestsDB);
            }
            return model;
        }

        public BusinessTestsDto CreateOrEditBusinessTest(BusinessTestsDto model)
        {
            TblBusinessTests? businessTestDB = null;
            if (model.Id > 0)
            {
                businessTestDB = _unitOfWork.BusinessTests.FirstOrDefault(c => !c.IsDeleted && c.Id == model.Id);
                if (businessTestDB != null)
                {
                    if (model.IsDeleted)
                    {
                        businessTestDB.IsDeleted = true;
                    }
                    else
                    {
                        _mapper.Map(model, businessTestDB);
                    }
                }
            }
            else
            {
                businessTestDB = _mapper.Map<TblBusinessTests>(model);
                _unitOfWork.BusinessTests.Add(businessTestDB);
                model.Id = businessTestDB.Id;
            }
            _unitOfWork.Complete();
            return model;
        }

        public BusinessTestsDto Mapping(TblBusinessTests businessTest)
        {
            BusinessTestsDto model = new BusinessTestsDto();
            model = _mapper.Map<BusinessTestsDto>(businessTest);
            model.TestTypeName = businessTest.TestType.Name;
            model.TestType = _mapper.Map<TestTypesDto>(businessTest.TestType);
            if (businessTest.TestSubTypeId.HasValue)
            {
                model.TestSubType = _mapper.Map<TestSubTypesDto>(businessTest.TestSubType);
            }
            return model;
        }
    }
}
