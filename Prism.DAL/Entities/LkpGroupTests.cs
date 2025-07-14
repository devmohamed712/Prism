using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Prism.DAL
{
    [Table("LkpGroupTests", Schema = "lkp")]
    public partial class LkpGroupTests
    {
        public LkpGroupTests()
        {
            OrderSampleTests = new HashSet<TblOrderSampleTests>();
        } 
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(512)]
        public string Name { get; set; }
        [Required]
        [StringLength(512)]
        public string Type { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<TblOrderSampleTests> OrderSampleTests { get; set; }
    }
}
