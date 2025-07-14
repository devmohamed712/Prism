using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Prism.DAL
{
    [Table("LkpSampleTypes", Schema = "lkp")]
    public partial class LkpSampleTypes
    {
        public LkpSampleTypes()
        {
            BusinessSamples = new HashSet<TblBusinessSamples>();
            OrderSamples = new HashSet<TblOrderSamples>();
        }

        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(512)]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<TblBusinessSamples> BusinessSamples { get; set; }
        public virtual ICollection<TblOrderSamples> OrderSamples { get; set; }
    }
}
