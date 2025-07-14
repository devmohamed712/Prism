using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Prism.BL.Dtos
{
    public class AboutUsDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(512)]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class TestTypesDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(512)]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class TestSubTypesDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(512)]
        public string Name { get; set; }
        [Required]
        public int OrderTypeId { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class SampleTypesDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(512)]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class SamplerDocumentTypesDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(512)]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class OrderStatusDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(512)]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class StateDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(512)]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class AreaDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(512)]
        public string Name { get; set; }
        [Required]
        public int StateId { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class GroupTestsDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(512)]
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsDeleted { get; set; }

        public string Status { get; set; }
    }

    public class SampleTestStatusDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(512)]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class NotificationTypesDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(512)]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }
}
