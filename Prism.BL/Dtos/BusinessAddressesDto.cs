using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Prism.BL.Dtos
{
    public class BusinessAddressesDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(512)]
        public string Address { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 12)")]
        public decimal Latitude { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 12)")]
        public decimal Longitude { get; set; }
        [StringLength(512)]
        public string Country { get; set; }
        public int? AreaId { get; set; }
        public string Zipcode { get; set; }
        public int BusinessId { get; set; }
        [Required]
        public bool IsMain { get; set; }
        public bool IsDeleted { get; set; }

        public string AreaName { get; set; }
        public string StateName { get; set; }
    }
}
