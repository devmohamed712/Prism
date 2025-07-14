using Prism.BL.Dtos;
using Prism.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.BL.Managers.Business.BasinessSamples
{
    public interface IBusinessSamplesManager
    {
        public List<BusinessSamplesDto> GetBusinessSamples(int businessId);

        public BusinessSamplesDto? GetBusinessSample(int id);

        public BusinessSamplesDto CreateOrEditBusinessSample(BusinessSamplesDto model);

        public BusinessSamplesDto Mapping(TblBusinessSamples businessSample);
    }
}
