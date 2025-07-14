using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Prism.DAL
{
    [Table("TblSamplerDocuments", Schema = "Sampler")]
    public partial class TblSamplerDocuments
    {
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string AccountId { get; set; }
        [Required]
        public int SamplerDocumentTypeId { get; set; }
        [Required]
        public DateTime ExpireDate { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public string FileRealName { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("AccountId")]
        public virtual TblAccounts Account { get; set; }
        [ForeignKey("SamplerDocumentTypeId")]
        public virtual LkbSamplerDocumentTypes SamplerDocumentType { get; set; }
    }
}
