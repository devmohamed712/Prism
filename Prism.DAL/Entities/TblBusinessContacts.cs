using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Prism.DAL
{
    [Table("TblBusinessContacts", Schema = "Business")]
    public partial class TblBusinessContacts
    {
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        [Required]
        public int BusinessId { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("AboutUsId")]
        public virtual LkpAboutUs AboutUs { get; set; }
        [ForeignKey("BusinessId")]
        public virtual TblBusinesses Business { get; set; }
    }
}
