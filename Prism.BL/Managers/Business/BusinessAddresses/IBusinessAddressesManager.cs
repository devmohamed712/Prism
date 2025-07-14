using Prism.BL.Dtos;
using Prism.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.BL.Managers.Business.BusinessAddresses
{
    public interface IBusinessAddressesManager
    {
        public List<BusinessAddressesDto> GetBusinessAddresses(int businessId);

        public BusinessAddressesDto? GetBusinessAddress(int id);

        public BusinessAddressesDto CreateOrEditBusinessAddress(BusinessAddressesDto model, List<LkpAreas> areas);
    }
}
