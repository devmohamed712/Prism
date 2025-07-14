using AutoMapper;
using Prism.BL.Dtos;
using Prism.DAL;
using Prism.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.BL.Managers.Business
{
    public interface IBusinessManager
    {
        public BusinessDtoList GetBusinesses(int pageNumber, int pageSize);

        public List<BusinessDto> GetBusinesses();

        public BusinessDto? GetBusiness(int id);

        public BusinessDto CreateOrEditBusiness(BusinessDto model);

        public bool DeleteOrRestoreBusiness(int id, bool isDelete);

        public bool ChangeMRlicenseStatus(int id, bool status);

        public BusinessDto Mapping(TblBusinesses businessDB);
    }
}
