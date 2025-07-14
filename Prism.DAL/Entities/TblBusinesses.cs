using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Prism.DAL
{
    [Table("TblBusiness", Schema = "Business")]
    public partial class TblBusinesses
    {
        public TblBusinesses()
        {
            BusinessContacts = new HashSet<TblBusinessContacts>();
            Orders = new HashSet<TblOrders>();
            BusinessAddresses = new HashSet<TblBusinessAddresses>();
            BusinessTests = new HashSet<TblBusinessTests>();
            BusinessSamples = new HashSet<TblBusinessSamples>();
        }

        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

        public virtual ICollection<TblBusinessContacts> BusinessContacts { get; set; }
        public virtual ICollection<TblOrders> Orders { get; set; }
        public virtual ICollection<TblBusinessAddresses> BusinessAddresses { get; set; }
        public virtual ICollection<TblBusinessTests> BusinessTests { get; set; }
        public virtual ICollection<TblBusinessSamples> BusinessSamples { get; set; }
    }
}
