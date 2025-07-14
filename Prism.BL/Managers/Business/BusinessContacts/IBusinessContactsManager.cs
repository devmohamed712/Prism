using Prism.BL.Dtos;
using Prism.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.BL.Managers.Business.BusinessContacts
{
    public interface IBusinessContactsManager
    {
        public List<BusinessContactsDto> GetBusinessContacts(int businessId);

        public BusinessContactsDto? GetBusinessContact(int id);

        public BusinessContactsDto CreateOrEditBusinessContact(BusinessContactsDto model);
    }
}
