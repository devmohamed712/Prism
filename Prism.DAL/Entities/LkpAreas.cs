using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Prism.DAL
{
    [Table("LkpAreas", Schema = "lkp")]
    public partial class LkpAreas
    {
        public LkpAreas() 
        {
            BusinessAddresses = new HashSet<TblBusinessAddresses>();
        }
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(512)]
        public string Name { get; set; }
        [Required]
        public int StateId { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("StateId")]
        public virtual LkpStates LkpStates { get; set; }
        public virtual ICollection<TblBusinessAddresses> BusinessAddresses { get; set; }
    }
}
