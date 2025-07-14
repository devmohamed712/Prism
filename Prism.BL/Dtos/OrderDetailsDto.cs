using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Prism.BL.Dtos
{
    public class OrderDetailsDto
    {
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int StatusId { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
    }
}
