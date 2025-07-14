using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Prism.DAL
{
    [Table("LkpAboutUs", Schema = "lkp")]
    public partial class LkpAboutUs
    {
        public LkpAboutUs()
        {
            BusinessContacts = new HashSet<TblBusinessContacts>();
        }

        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(512)]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<TblBusinessContacts> BusinessContacts { get; set; }
    }
}
