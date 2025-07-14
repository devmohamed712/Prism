using Prism.BL.Dtos;
using Prism.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.BL.Managers.Business.BusinessTests
{
    public interface IBusinessTestsManager
    {
        public List<BusinessTestsDto> GetBusinessTests(int businessId);

        public BusinessTestsDto? GetBusinessTest(int id);

        public BusinessTestsDto CreateOrEditBusinessTest(BusinessTestsDto model);

        public BusinessTestsDto Mapping(TblBusinessTests businessTest);
    }
}
