using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Prism.DAL
{
    [Table("LkpOrderSubTypes ", Schema = "lkp")]
    public partial class LkpTestSubTypes
    {
        public LkpTestSubTypes()
        {
            BusinessTests = new HashSet<TblBusinessTests>();
            OrderSamples = new HashSet<TblOrderSamples>();
        }

        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(512)]
        public string Name { get; set; }
        [Required]
        public int OrderTestTypeId { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("OrderTestTypeId")]
        public virtual LkpTestTypes OrderTestType { get; set; }
        public virtual ICollection<TblBusinessTests> BusinessTests { get; set; }
        public virtual ICollection<TblOrderSamples> OrderSamples { get; set; }
    }
}
