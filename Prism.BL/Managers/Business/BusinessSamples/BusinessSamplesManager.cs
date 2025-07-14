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

namespace Prism.BL.Managers.Business.BasinessSamples
{
    public class BusinessSamplesManager : IBusinessSamplesManager
    {
        public readonly IMapper _mapper;
        public readonly IConfiguration _configuration;
        public readonly IUnitOfWork _unitOfWork;

        public BusinessSamplesManager(IMapper mapper, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public List<BusinessSamplesDto> GetBusinessSamples(int businessId)
        {
            List<BusinessSamplesDto> businessSamples = new List<BusinessSamplesDto>();
            IEnumerable<TblBusinessSamples> businessSamplesDB = _unitOfWork.BusinessSamples.FindList(x => !x.IsDeleted && x.BusinessId == businessId);
            if (businessSamplesDB != null)
            {
                businessSamples = _mapper.Map<List<BusinessSamplesDto>>(businessSamplesDB);
            }
            return businessSamples;
        }

        public BusinessSamplesDto? GetBusinessSample(int id)
        {
            BusinessSamplesDto? model = null;
            var businessSampleDB = _unitOfWork.BusinessSamples.FirstOrDefault(c => !c.IsDeleted && c.Id == id);
            if (businessSampleDB != null)
            {
                model = _mapper.Map<BusinessSamplesDto>(businessSampleDB);
            }
            return model;
        }

        public BusinessSamplesDto CreateOrEditBusinessSample(BusinessSamplesDto model)
        {
            TblBusinessSamples? businessSampleDB = null;
            if (model.Id > 0)
            {
                businessSampleDB = _unitOfWork.BusinessSamples.FirstOrDefault(c => !c.IsDeleted && c.Id == model.Id);
                if (businessSampleDB != null)
                {
                    if (model.IsDeleted)
                    {
                        businessSampleDB.IsDeleted = true;
                    }
                    else
                    {
                        _mapper.Map(model, businessSampleDB);
                    }
                }
            }
            else
            {
                businessSampleDB = _mapper.Map<TblBusinessSamples>(model);
                _unitOfWork.BusinessSamples.Add(businessSampleDB);
                model.Id = businessSampleDB.Id;
            }
            _unitOfWork.Complete();
            return model;
        }

        public BusinessSamplesDto Mapping(TblBusinessSamples businessSample)
        {
            BusinessSamplesDto model = new BusinessSamplesDto();
            model = _mapper.Map<BusinessSamplesDto>(businessSample);
            model.SampleType = _mapper.Map<SampleTypesDto>(businessSample.SampleType);
            return model;
        }
    }
}
