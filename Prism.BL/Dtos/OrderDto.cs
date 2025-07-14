using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Prism.BL.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        [StringLength(128)]
        public string Name { get; set; }
        [Required]
        public int BusinessId { get; set; }
        public string SamplerId { get; set; }
        public int StatusId { get; set; }
        public DateTime DateTime { get; set; }
        [Required]
        public int EstimateNumber { get; set; }
        public int? CompanyOrIndividual { get; set; }
        [StringLength(512)]
        public string AdultUse { get; set; }
        [StringLength(512)]
        public string Med { get; set; }
        public bool IsCanceled { get; set; }
        public bool IsDeleted { get; set; }
        public int? LabTechFilledFormsCount { get; set; }

        public string SamplerName { get; set; }
        public string StatusName { get; set; }
        public int? CurrentSampleForm { get; set; }

        public BusinessDto Business { get; set; }
        public List<BusinessContactsDto> BusinessContacts { get; set; }
        public List<OrderSamplesDto> OrderSamples { get; set; }
    }

    public class OrderDtoList
    {
        public OrderDtoList()
        {
            Orders = new List<OrderDto>();
        }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool IsLastPage { get; set; }
        public bool IsSamplerHasOrder { get; set; }
        public List<OrderDto> Orders { get; set; }
    }
}
