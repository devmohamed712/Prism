using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Prism.BL.Dtos
{
    public class BusinessTestsDto
    {
        public int Id { get; set; }
        public int BusinessId { get; set; }
        [Required]
        public int TestTypeId { get; set; }
        public int? TestSubTypeId { get; set; }
        public bool IsDeleted { get; set; }

        public string TestTypeName { get; set; }
        public TestTypesDto TestType { get; set; }
        public TestSubTypesDto TestSubType { get; set; }
    }
}
