using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Prism.DAL
{
    [Table("LkbSamplerDocumentTypes", Schema = "lkp")]
    public partial class LkbSamplerDocumentTypes
    {
        public LkbSamplerDocumentTypes()
        {
            SamplerDocuments = new HashSet<TblSamplerDocuments>();
        }

        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(512)]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<TblSamplerDocuments> SamplerDocuments { get; set; }
    }
}
