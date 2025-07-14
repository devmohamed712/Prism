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

namespace Prism.BL.Managers.Business.BusinessContacts
{
    public class BusinessContactsManager : IBusinessContactsManager
    {
        public readonly IMapper _mapper;
        public readonly IConfiguration _configuration;
        public readonly IUnitOfWork _unitOfWork;

        public BusinessContactsManager(IMapper mapper, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public List<BusinessContactsDto> GetBusinessContacts(int businessId)
        {
            List<BusinessContactsDto> businessContacts = new List<BusinessContactsDto>();
            IEnumerable<TblBusinessContacts> businessContactsDB = _unitOfWork.BusinessContacts.FindList(x => !x.IsDeleted && x.BusinessId == businessId);
            if (businessContactsDB != null)
            {
                businessContacts = _mapper.Map<List<BusinessContactsDto>>(businessContactsDB);
            }
            return businessContacts;
        }

        public BusinessContactsDto? GetBusinessContact(int id)
        {
            BusinessContactsDto? model = null;
            var businessContactDB = _unitOfWork.Business.FirstOrDefault(c => !c.IsDeleted && c.Id == id);
            if (businessContactDB != null)
            {
                model = _mapper.Map<BusinessContactsDto>(businessContactDB);
            }
            return model;
        }

        public BusinessContactsDto CreateOrEditBusinessContact(BusinessContactsDto model)
        {
            TblBusinessContacts? businessContactDB = null;
            if (model.Id > 0)
            {
                businessContactDB = _unitOfWork.BusinessContacts.FirstOrDefault(c => !c.IsDeleted && c.Id == model.Id);
                if (businessContactDB != null)
                {
                    if (model.IsDeleted)
                    {
                        businessContactDB.IsDeleted = true;
                    }
                    else
                    {
                        _mapper.Map(model, businessContactDB);
                    }
                }
            }
            else
            {
                businessContactDB = _mapper.Map<TblBusinessContacts>(model);
                _unitOfWork.BusinessContacts.Add(businessContactDB);
                model.Id = businessContactDB.Id;
            }
            _unitOfWork.Complete();
            return model;
        }
    }
}
