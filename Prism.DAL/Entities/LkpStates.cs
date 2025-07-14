using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Prism.DAL
{
    [Table("LkpStates", Schema = "lkp")]
    public partial class LkpStates
    {
        public LkpStates()
        {
            LkpAreas = new HashSet<LkpAreas>();
        }
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(512)]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<LkpAreas> LkpAreas { get; set; }
    }
}
