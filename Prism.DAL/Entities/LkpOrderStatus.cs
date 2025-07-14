using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Prism.DAL
{
    [Table("LkpOrderStatus", Schema = "lkp")]
    public class LkpOrderStatus
    {
        public LkpOrderStatus()
        {
            SamplerTracks = new HashSet<TblSamplerTracks>();
            OrderDetails = new HashSet<TblOrderDetails>();
        }
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(512)]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<TblSamplerTracks> SamplerTracks { get; set; }
        public virtual ICollection<TblOrderDetails> OrderDetails { get; set; }
    }
}
