using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Prism.BL.Dtos
{
    public class OrderSampleTestsDto
    {
        public int Id { get; set; }
        [Required]
        public int OrderSampleId { get; set; }
        [Required]
        public int TestId { get; set; }
        public DateTime DateTime { get; set; }
        public int? StatusId { get; set; }
        public bool IsDeleted { get; set; }
        public string? LabTechId { get; set; }

        public OrderSamplesDto OrderSample { get; set; }
        public GroupTestsDto Test { get; set; }
        public SampleTestStatusDto TestStatus { get; set; }
    }

    public class OrderSampleTestsDtoList
    {
        public OrderSampleTestsDtoList()
        {
            OrderSampleTests = new List<OrderSampleTestsDto>();
        }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool IsLastPage { get; set; }
        public List<OrderSampleTestsDto> OrderSampleTests { get; set; }
    }
}
