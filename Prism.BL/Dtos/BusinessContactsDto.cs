using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Prism.BL.Dtos
{
    public class BusinessContactsDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        [Required]
        [StringLength(255)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(255)]
        public string LastName { get; set; }
        [Required]
        [StringLength(255)]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(255)]
        public string EmailAddress { get; set; }
        [Required]
        public int AboutUsId { get; set; }
        public int BusinessId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
