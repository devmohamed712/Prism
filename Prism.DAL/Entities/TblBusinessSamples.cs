using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Prism.DAL
{
    [Table("TblBusinessSamples", Schema = "Business")]
    public partial class TblBusinessSamples
    {
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int BusinessId { get; set; }
        [Required]
        public int SampleTypeId { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("BusinessId")]
        public virtual TblBusinesses Business { get; set; }
        [ForeignKey("SampleTypeId")]
        public virtual LkpSampleTypes SampleType { get; set; }
    }
}
