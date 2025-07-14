using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Prism.BL.Dtos
{
    public class BusinessDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string EntityName { get; set; }
        [Required]
        [StringLength(255)]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(255)]
        public string EmailAddress { get; set; }
        [Required]
        [StringLength(255)]
        public string FacilityLicensesNumber { get; set; }
        public int? LicenseTypes { get; set; }
        [StringLength(255)]
        public string MedicalLicense { get; set; }
        [StringLength(255)]
        public string RecreationalLicense { get; set; }
        public bool IsDeleted { get; set; }

        public List<BusinessContactsDto> Contacts { get; set; }
        public List<BusinessAddressesDto> Addresses { get; set; }
        public List<BusinessTestsDto> Tests { get; set; }
        public List<BusinessSamplesDto> Samples { get; set; }
        [NotMapped]
        public int OrdersCount { get; set; }
    }

    public class BusinessDtoList
    {
        public BusinessDtoList()
        {
            Businesses = new List<BusinessDto>();
        }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool IsLastPage { get; set; }
        public List<BusinessDto> Businesses { get; set; }
    }
}
