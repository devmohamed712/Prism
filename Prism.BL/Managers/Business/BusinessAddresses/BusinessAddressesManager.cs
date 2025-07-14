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

namespace Prism.BL.Managers.Business.BusinessAddresses
{
    public class BusinessAddressesManager : IBusinessAddressesManager
    {
        public readonly IMapper _mapper;
        public readonly IConfiguration _configuration;
        public readonly IUnitOfWork _unitOfWork;

        public BusinessAddressesManager(IMapper mapper, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public List<BusinessAddressesDto> GetBusinessAddresses(int businessId)
        {
            List<BusinessAddressesDto> businessAddresses = new List<BusinessAddressesDto>();
            IEnumerable<TblBusinessAddresses> businessAddressesDB = _unitOfWork.BusinessAddresses.FindList(x => !x.IsDeleted && x.BusinessId == businessId);
            if (businessAddressesDB != null)
            {
                businessAddresses = _mapper.Map<List<BusinessAddressesDto>>(businessAddressesDB);
            }
            return businessAddresses;
        }

        public BusinessAddressesDto? GetBusinessAddress(int id)
        {
            BusinessAddressesDto? model = null;
            var businessAddressDB = _unitOfWork.BusinessAddresses.FirstOrDefault(c => !c.IsDeleted && c.Id == id);
            if (businessAddressDB != null)
            {
                model = _mapper.Map<BusinessAddressesDto>(businessAddressDB);
            }
            return model;
        }

        public BusinessAddressesDto CreateOrEditBusinessAddress(BusinessAddressesDto model, List<LkpAreas> areas)
        {
            TblBusinessAddresses? businessAddressDB = null;
            if (model.Id > 0)
            {
                businessAddressDB = _unitOfWork.BusinessAddresses.FirstOrDefault(c => !c.IsDeleted && c.Id == model.Id);
                if (businessAddressDB != null)
                {
                    if (model.IsDeleted)
                    {
                        businessAddressDB.IsDeleted = true;
                    }
                    else
                    {
                        _mapper.Map(model, businessAddressDB);
                    }
                }
            }
            else
            {
                model.AreaId = areas.FirstOrDefault(x => x.Name.Equals(model.AreaName) && x.LkpStates.Name.Equals(model.StateName)).Id;
                businessAddressDB = _mapper.Map<TblBusinessAddresses>(model);
                _unitOfWork.BusinessAddresses.Add(businessAddressDB);
                model.Id = businessAddressDB.Id;
            }
            _unitOfWork.Complete();
            return model;
        }
    }
}
