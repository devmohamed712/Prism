using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Prism.BL.Dtos
{
    public class BusinessSamplesDto
    {
        public int Id { get; set; }
        [Required]
        public int BusinessId { get; set; }
        [Required]
        public int SampleTypeId { get; set; }
        public bool IsDeleted { get; set; }

        public SampleTypesDto SampleType { get; set; }
    }
}
